using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TableStrategy
{
	[DataContract]
	/// <summary> Игровое поле, на котором могут располагаться юниты </summary>
	public class GameGrid
	{
		[DataMember]
		/// <summary> Спиоск юнитов, расположенных на сетке </summary>
		protected List<GameUnit> _gameUnits;

		[DataMember]
		/// <summary> Высота сетки </summary>
		public int Height { get; private set; }
		[DataMember]
		/// <summary> Длина сетки </summary>
		public int Width { get; private set; }

		public List<GameUnit> FullUnitList => _gameUnits;

		public GameGrid(int height, int width) 
		{
			Height = height;
			Width = width;

			_gameUnits = new List<GameUnit>(0);
		}

		private void CorrectPosition(GridPoint position)
		{
			if (position.X >= Height)
				throw new Exception("X - больше размера поля");

			if(position.Y >= Width)
				throw new Exception("Y - больше размера поля");
		}

		public List<GridPoint> GridPointsLimit(List<GridPoint> points)
		{
			for (int i = points.Count - 1; i > 0; i--)
			{
				if (points[i].X >= Height || points[i].Y >= Width)
					points.RemoveAt(i);
			}

			return points;
		}


		public void AddUnit(GameUnit gameUnit) => _gameUnits.Add(gameUnit);
		public void AddUnit(GameUnit gameUnit, GridPoint position)
		{
			CorrectPosition(position);

			if (UnitOnCellCheck(position))
				throw new Exception("На данной позиции уже стоит другой юнит");

			gameUnit.NewTransform(position);
			AddUnit(gameUnit);
		}
		public void CreateUnit(GameUnitInfo unitInfo, GameSide gameSide, GridPoint position)
		{ 
			GameUnit gameUnit = new GameUnit(unitInfo, gameSide);
			AddUnit(gameUnit, position);
		}

		public bool UnitOnCellCheck(GridPoint position) => _gameUnits.Any(u => u.UnitTransform.Position == position && u.Alive == true);
		public GameUnit GetFromPosition(GridPoint position)
		{
			if (!UnitOnCellCheck(position))
				throw new Exception("На данной ячейке нет юнита");

			GameUnit gameUnit = _gameUnits.Find(u => u.UnitTransform.Position == position);
			return gameUnit;
		}

	}
}
