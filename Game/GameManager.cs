using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;

namespace TableStrategy
{
	[DataContract]
	/// <summary> Полная логика игры </summary>
	public class GameManager
	{
		public const int MapWidth = 14;
		public const int MapHeight = 10;

		/// <summary> Размер ресурсов игроков на старте игры </summary>
		public const int PlayerStartCoins = 3;

		[DataMember]
		/// <summary> ПровТекущий этап игры </summary>
		private BattleStage _currentStage;
		[DataMember]
		private BattleGrid _battleGrid;
		[DataMember]
		private Player[] _players;
		[DataMember]
		/// <summary> Игрок выполняющий ход в данный момент </summary>
		private int _currentPlayerTurnNum;

		public BattleGrid GetBattleGrid => _battleGrid;

		public BattleStage CurrentStage => _currentStage;
		public Player CurrentPlayerTurn => _players[_currentPlayerTurnNum];
		public Player[] GetPlayers => _players.ToArray();

		public GameManager()
		{
			_currentStage = BattleStage.MOVE;
			_players = new Player[]
			{
				 new Player(GameSide.LEFT, "Левый Игрок"),
				 new Player(GameSide.RIGHT, "Правый Игрок"),
			};

			foreach (Player player in _players)
				player.CoinsTop(PlayerStartCoins);

			PlayerOrder();
			_battleGrid = new BattleGrid(MapWidth, MapHeight);
			LoadEvents();
		}

		public void LoadEvents()
		{
			_battleGrid.OnUnitTurnEnd += UnitTurnEnd;

			GameEvents.OnNewRound += ResourcesBonus;
			GameEvents.OnNewRound += () => StartStage(BattleStage.MOVE);
			GameEvents.OnGameEnd += (Player) => new GameSerialization().DeleteSaveFile();

			GameEvents.OnGameClose += CloseEvents;
		}

		private void CloseEvents()
		{
			_battleGrid.OnUnitTurnEnd -= UnitTurnEnd;

			GameEvents.OnNewRound -= ResourcesBonus;
			GameEvents.OnNewRound -= () => StartStage(BattleStage.MOVE);
			GameEvents.OnGameEnd -= (Player) => new GameSerialization().DeleteSaveFile();
			GameEvents.OnGameClose -= CloseEvents;
		}

		private void UnitTurnEnd()
		{
			if (NextPlayerTurnCheck())
				NextPlayerTurn();
		}
		private void PlayerOrder()
		{ 
			int firstPlayerNum = GameRandomazer.GetOnRange(0, _players.Count() - 1);

			Player player = _players[firstPlayerNum];
			_players[firstPlayerNum] = _players[0];
			_players[0] = player;

			_currentPlayerTurnNum = 0;
		}

		public void NextStage()
		{
			_currentPlayerTurnNum = 0;
			int stagesCount = Enum.GetNames(typeof(BattleStage)).Length;

			if ((int)(CurrentStage) + 1 == stagesCount)
				GameEvents.SendNewRound();
			else
				StartStage((BattleStage)((int)(CurrentStage + 1)));

			GameEndTest();
		}
		public bool NextPlayerTurnCheck()
		{
			List<GameUnit> unitList = _battleGrid.FullUnitList.Where(u => u.GameSide == CurrentPlayerTurn.Side && u.Alive).ToList();

			if (CurrentStage == BattleStage.MOVE)
				unitList = unitList.Where(u => u.CanMove == true).ToList();
			else
				unitList = unitList.Where(u => u.CanAttack == true).ToList();

			return unitList.Count == 0;
		}
		public void NextPlayerTurn()
		{
			_currentPlayerTurnNum++;

			if (_currentPlayerTurnNum == _players.Length)
				NextStage();
		}

		private void ResourcesBonus()
		{
			int player1Bonus = _battleGrid.FullUnitList.Where(u => u.GameSide == _players[0].Side).Count(u => _battleGrid.GameMap.GetCell(u.UnitTransform.Position) == MapCell.RESOURCE_SOURCE);
			int player2Bonus = _battleGrid.FullUnitList.Where(u => u.GameSide == _players[1].Side).Count(u => _battleGrid.GameMap.GetCell(u.UnitTransform.Position) == MapCell.RESOURCE_SOURCE);

			_players[0].CoinsTop(player1Bonus);
			_players[1].CoinsTop(player2Bonus);
		}


		public void StartStage(BattleStage stage) => _currentStage = stage;

		public void GameEndTest()
		{
			if (_battleGrid.FullUnitList.Count(u => u.GameSide == _players[0].Side && u.Alive) == 0 && _players[0].Coins == 0)
				GameEvents.SendGameEnd(_players[1]);

			if (_battleGrid.FullUnitList.Count(u => u.GameSide == _players[1].Side && u.Alive) == 0 && _players[1].Coins == 0)
				GameEvents.SendGameEnd(_players[0]);
		}

	}
}
