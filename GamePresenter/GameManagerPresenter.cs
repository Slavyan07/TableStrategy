using System;
using System.Windows;
using System.Windows.Controls;

namespace TableStrategy
{
	public class GameManagerPresenter : PresenterBehavior
	{
		private GameManager _game;
		public BattleGridPresenter BattleGridPresenter { get; private set; }

		private string _currentTurnPlanerName;
		private string _currentGameStage;
		private string _player1CoinsCount;
		private string _player2CoinsCount;

		public string CurrentTurnPlayerName
		{ 
			get { return _currentTurnPlanerName; }
			set
			{
				_currentTurnPlanerName = value;
				OnPropertyChanged();
			}
		}
		public string CurrentGameStage
		{
			get { return _currentGameStage; }
			set
			{
				_currentGameStage = value;
				OnPropertyChanged();
			}
		}
		public string Player1CoinsCount
		{
			get { return _player1CoinsCount; }
			set
			{ 
				_player1CoinsCount = value;
				OnPropertyChanged();
			}
		}
		public string Player2CoinsCount
		{
			get { return _player2CoinsCount; }
			set
			{
				_player2CoinsCount = value;
				OnPropertyChanged();
			}
		}

		public GameManagerPresenter(GameManager game, Canvas battleGridRender)
		{
			_game = game;
			BattleGridPresenter = new BattleGridPresenter(battleGridRender, game.GetBattleGrid);
			CurrentGameStage = CurrentStageName();

			EventsLoad();
		}

		private void EventsLoad()
		{
			GameEvents.OnGetDiceBonus += GetDiceResult;
			GameEvents.OnNewRound += PlayerTurnView;
			GameEvents.OnGameEnd += GameEnd;
			GameEvents.OnGameClose += EventsClose;
		}

		private void EventsClose()
		{ 
			GameEvents.OnGetDiceBonus -= GetDiceResult;
			GameEvents.OnNewRound -= PlayerTurnView;
			GameEvents.OnGameEnd -= GameEnd;
			GameEvents.OnGameClose -= EventsClose;
		}

		private int GetDiceResult()
		{
			DicesView dicesView = new DicesView();
			dicesView.ShowDialog();

			if (dicesView.IsWasMixed)
				return dicesView.Result;
			else
				return -1;
		}

		public void SelectCell(int y, int x)
		{
			try
			{
				BattleGridPresenter.SelectCell(y, x, _game.CurrentPlayerTurn, _game.CurrentStage);
				BattleGridPresenter.DrawGrid(_game.CurrentStage);
				PlayerTurnView();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}", "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		public string CurrentStageName()
		{
			switch (_game.CurrentStage)
			{
				//case BattleStage.NONE: return "Ожидание";
				case BattleStage.MOVE: return "Фаза хода";
				case BattleStage.SHOOTING: return "Фаза сражение";

				default: return "";
			}
		}

		public void PlayerTurnView()
		{
			CurrentTurnPlayerName = $"Ход Игрока - {_game.CurrentPlayerTurn.Name}";
			CurrentGameStage = CurrentStageName();

			Player1CoinsCount = $"{_game.GetPlayers[0].Name}: {_game.GetPlayers[0].Coins} ресурсов";
			Player2CoinsCount = $"{_game.GetPlayers[1].Name}: {_game.GetPlayers[1].Coins} ресурсов";
		}

		private void GameEnd(Player winPlayer)
		{
			MessageBox.Show($"Победил игрок: {winPlayer.Name}", "Игра окончена", MessageBoxButton.OK, MessageBoxImage.Information);
		}
	}
}
