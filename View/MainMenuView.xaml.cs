using System;
using System.Windows;

namespace TableStrategy
{
	/// <summary> Окно главного меню </summary>
	public partial class MainMenuView : Window
	{
		private GameSerialization _gameSerialization;

		public MainMenuView()
		{
			InitializeComponent();
			_gameSerialization = new GameSerialization();
			LoadButton.IsEnabled = (_gameSerialization.LoadCheck());
		}

		private void Window_Closed(object sender, EventArgs e) => Environment.Exit(0);

		private void NewGameButton_Click(object sender, RoutedEventArgs e)
		{
			GameManager newGame = new GameManager();
			GameView gameView = new GameView(newGame);
			this.Hide();
			gameView.Show();
		}

		private void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			GameManager loadGame = _gameSerialization.Load();
			GameView gameView = new GameView(loadGame);
			this.Hide();
			gameView.Show();
		}
	}
}
