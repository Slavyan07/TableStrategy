using System;
using System.Windows;
using System.Windows.Input;

namespace TableStrategy
{
	/// <summary> Игровое окно </summary>
	public partial class GameView : Window
	{
		private GameManager _gameManager;

		private BattleGridPresenter _gridPresenter;
		private GameManagerPresenter _gamePresenter;

		public GameManagerPresenter GamePresenter => _gamePresenter;
		public BattleGridPresenter BattleGridPresenter => _gridPresenter;

		public GameView(GameManager game)
		{
			InitializeComponent();
			_gameManager = game;
			_gamePresenter = new GameManagerPresenter(_gameManager, GameGrid);
			_gridPresenter = new BattleGridPresenter(GameGrid, _gameManager.GetBattleGrid);
			_gamePresenter.PlayerTurnView();

			DataContext = _gamePresenter;

			GameEvents.OnGameEnd += (Player winPlayer) => this.Hide();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_gridPresenter.DrawGrid(_gameManager.CurrentStage);
		}

		private void Window_Closed(object sender, System.EventArgs e)
		{
			MainMenuView menuView = new MainMenuView();

			GameSerialization gameSerialization = new GameSerialization();
			gameSerialization.Save(_gameManager);

			GameEvents.SendGameClose();

			this.Hide();
			menuView.Show();
		}

		private void GridCellsSelected(Point mousePosition)
		{
			GameGrid.Children.Clear();

			int x = (int)(Math.Floor(mousePosition.X) / BattleGridPresenter.CellWidth);
			int y = (int)(Math.Floor(mousePosition.Y) / BattleGridPresenter.CellHeight);

			if (!_gridPresenter.CellClickCheck(x, y))
				return;

			_gamePresenter.SelectCell(y, x);
			_gameManager.GameEndTest();
		}

		private void GameGrid_MouseUp(object sender, MouseButtonEventArgs e)
		{
			Point mousePosition = Mouse.GetPosition(GameGrid);
			GridCellsSelected(mousePosition);
			_gamePresenter.PlayerTurnView();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DicesView dicesView = new DicesView();
			dicesView.ShowDialog();
		}

		private void Skip_Click(object sender, RoutedEventArgs e)
		{
			_gameManager.NextPlayerTurn();
			_gameManager.GetBattleGrid.ClearSelect();
			_gridPresenter.DrawGrid(_gameManager.CurrentStage);
			_gamePresenter.PlayerTurnView();
		}

		private void ShopButton_Click(object sender, RoutedEventArgs e)
		{
			UnitShopView shopView = new UnitShopView(_gameManager.CurrentPlayerTurn, _gameManager.GetBattleGrid);
			shopView.ShowDialog();
			_gridPresenter.DrawGrid(_gameManager.CurrentStage);
			_gamePresenter.PlayerTurnView();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				GameSerialization gameSerialization = new GameSerialization();
				gameSerialization.Save(_gameManager);
				MessageBox.Show("Игра успешно сохранена");
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
        }
    }
}
