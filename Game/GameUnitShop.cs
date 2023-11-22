using System;
using System.Collections.Generic;
using System.Windows;

namespace TableStrategy
{
	/// <summary> Магазин игрока для покупки юнитов </summary>
	public class GameUnitShop
	{
		private List<GameUnitInfo> _unitsShop;
		private GameUnitDatabase _database;
		private BattleGrid _battleGrid;

		public Player Player { get; private set; }

		public GameUnitInfo[] GetUnitArray => _unitsShop.ToArray();

		public GameUnitShop(BattleGrid battleGrid, Player player)
		{
			_database = GameUnitDatabase.GetInstance();
			_battleGrid = battleGrid;
			LoadShop(player.Side);

			Player = player;
		}

		private void LoadShop(GameSide gameSide)
		{
			_unitsShop = gameSide == GameSide.LEFT ? _database.GetFullLeftSideUnitList() : _database.GetFullRightSideUnitList();
		}

		private void PutUnit(GameSide gameSide, GameUnitInfo info)
		{
			int x = (gameSide == GameSide.LEFT) ? 0 : _battleGrid.Height - 1;

			for (int i = 0; i < _battleGrid.Width; i++)
			{
				if (!_battleGrid.UnitOnCellCheck(new GridPoint(i, x)))
				{
					GameUnit gameUnit = new GameUnit(info, gameSide);
					gameUnit.Disable();

					_battleGrid.AddUnit(gameUnit, new GridPoint(i, x));
					return;
				}
			}

			throw new Exception("Не хватает места, для появления новых юнитов");

		}

		public void BuyUnit(int index)
		{
			GameUnitInfo info = GetUnitArray[index];

			if (info.Price > Player.Coins)
				throw new Exception("Недостаточно средств");

			PutUnit(Player.Side, info);
			Player.CoinsSpend(info.Price);
		}
	}
}
