using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace TableStrategy
{
	public enum BattleStage
	{ 
		MOVE,
		SHOOTING,
	}

	[DataContract]
	/// <summary> Игровое поле, на котором юниты могут распологаться и сражаться </summary>
	public class BattleGrid : GameGrid
	{
		public const int PlayerUnitsCountLimit = 5;
		public const int MountainCellsCount = 7;
		public const int ResourseCellCount = 4;
		public const int DefendCellCount = 5;

		public const int DefendCellBonus = 1;

		[DataMember]
		/// <summary> Игровая карта </summary>
		private GameMap _gameMap;
		[DataMember]
		/// <summary> Список выделеных на сетке ячеек </summary>
		private List<GridPoint> _selectedCells;

		public GridPoint[] SelectedCells => _selectedCells.ToArray();
		public GameMap GameMap => _gameMap;
		/// <summary> Юнит выбранный игроком на сетке </summary>
		public GameUnit? SelectedUnit { get; private set; }
		/// <summary> Событие при завершении юнитом хода </summary>
		public Action OnUnitTurnEnd { get; set; }

		public BattleGrid(int height, int width) : base(height, width)
		{
			_selectedCells = new List<GridPoint>(0);
			_gameMap = new GameMap(width, height, MountainCellsCount, DefendCellCount, ResourseCellCount);

			GameEvents.OnNewRound += RestartUnitsStates;
		}

		/// <summary> Сбросить состояние юнитов </summary>
		public void RestartUnitsStates()
		{
			foreach (var unit in _gameUnits)
			{
				unit.UnitRestart();
			}
		}
		/// <summary> Очистить выделеных на сетке ячеек </summary>
		public void ClearSelect() => _selectedCells.Clear();

		/// <summary> Список ячеек которые выделенный юнит может атаковать </summary>
		public List<GridPoint> CanAttackCells()
		{
			if (SelectedUnit == null)
				throw new Exception("Юнит не выбран");

			if(!SelectedUnit.CanAttack)
				return new List<GridPoint>(0);

			List<GridPoint> attackRangePoints = SelectedUnit.UnitTransform.CellsAround(SelectedUnit.Info.Range);
			List<GridPoint> list = new List<GridPoint>(0);

			foreach (GridPoint point in attackRangePoints)
			{ 
				if(_gameUnits.Any(u => u.Alive && u.GameSide != SelectedUnit.GameSide && u.UnitTransform.Position == point))
					list.Add(point);
			}

			return list;
		}
		/// <summary> Список ячеек на которые выделенный юнит может сходить </summary>
		public List<GridPoint> CanMoveCells() 
		{
			if (SelectedUnit == null)
				throw new Exception("Юнит не выбран");

			if(!SelectedUnit.CanMove)
				return new List<GridPoint>(0);

			List<GridPoint> gridPoints = SelectedUnit.UnitTransform.CellAroundCircle(SelectedUnit.Info.Speed);
			gridPoints = GridPointsLimit(gridPoints);

			gridPoints.RemoveAll(p => _gameUnits.Any(unit => unit.UnitTransform.Position == p && unit.Alive));

			var letList = _gameMap.GetFullMap();
			gridPoints.RemoveAll(p => letList.Any(cell => cell.Item2 == p && cell.Item1 == MapCell.MOUNTAIN));

			return gridPoints;
		}

		/// <summary> Зачисленее ресуорсов-бонусов игрокам </summary>
		private void UnitCellBonus(GameUnit unit)
		{
			unit.DefendBonus = (_gameMap.GetCell(unit.UnitTransform.Position) == MapCell.DEFEND_BOOST) ? DefendCellBonus : 0;
		}

		/// <summary> Выбор юнита который принадлежит игроку </summary>
		private void SelectOwnUnit(GameUnit unit,  BattleStage battleStage)
		{
			SelectedUnit = unit;

			if (battleStage == BattleStage.MOVE)
				_selectedCells = CanMoveCells();
			else
				_selectedCells = CanAttackCells();
		}
		/// <summary> Выбор юнита который не принадлежит игроку </summary>
		private void SelectOtherUnit(GameUnit unit, Player currentPlayerTurn, BattleStage battleStage)
		{
			if (SelectedUnit == null || SelectedUnit.GameSide != currentPlayerTurn.Side)
			{
				SelectedUnit = unit;
				return;
			}

			if (battleStage != BattleStage.SHOOTING)
				return;

			if (_selectedCells.Any(p => unit.UnitTransform.Position == p))
			{
				int diceResult = GameEvents.SendDiceBonus();

				if (diceResult == -1)
					throw new Exception("Необходимо бросить кубики");

				if (diceResult < 4)
				{
					unit.DisableAttack();
					ClearSelect();
					throw new Exception("Промах");
				}

				SelectedUnit.Attack(unit);
				OnUnitTurnEnd.Invoke();
				ClearSelect();
			}
			else
			{
				SelectedUnit = unit;
				ClearSelect();
			}
		}
		/// <summary> Выбор ячейки, на которой нет юнитов </summary>
		private void SelectNullCell(GridPoint position, BattleStage battleStage)
		{
			if (battleStage != BattleStage.MOVE)
			{
				_selectedCells.Clear();
				return;
			}

			if (!_selectedCells.Any(p => p == position))
				return;

			SelectedUnit.Move(position);
			UnitCellBonus(SelectedUnit);
			OnUnitTurnEnd.Invoke();
			ClearSelect();
		}

		/// <summary> Выбор ячейки игроком </summary>
		public void SelectCell(int x, int y, Player currentPlayerTurn, BattleStage battleStage)
		{
			GridPoint position = new GridPoint(x, y);
			if (position.X >= Height)
				throw new Exception("X - больше размера сетки");
			if (position.Y >= Width)
				throw new Exception("Y - больше размера сетки");

			foreach (GameUnit unit in _gameUnits)
				unit.NullState();

			if (!UnitOnCellCheck(position))
			{
				SelectNullCell(position, battleStage);
				return;
			}

			GameUnit selectedUnit = GetFromPosition(position);

			if (selectedUnit.GameSide == currentPlayerTurn.Side)
				SelectOwnUnit(selectedUnit, battleStage);
			else
				SelectOtherUnit(selectedUnit, currentPlayerTurn, battleStage);
		}
	}
}
