using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TableStrategy
{
	public class GameShopPresenter
	{
		private const int TextDrawOffsetX = 51;
		private const int TextDrawOffsetY = 50;

		private Canvas _renderCanvas;
		private GameUnitShop _model;

		private GameGrid _grid;
		private GameGridPresenter _gridPresenter;

		private Label _coinsCountText;

		public GameShopPresenter(GameUnitShop model, Canvas canvas, Button[] buttons, Label coinsText) 
		{ 
			_model = model;
			_renderCanvas = canvas;

			_grid = new GameGrid(1, model.GetUnitArray.Length);
			_coinsCountText = coinsText;

			for (int i = 0; i < buttons.Length; i++)
				buttons[i].Visibility = Visibility.Hidden;

			for (int i = 0; i < model.GetUnitArray.Length; i++)
				buttons[i].Visibility = Visibility.Visible;

			LoadUnitsToGrid();
			_gridPresenter = new GameGridPresenter(canvas, _grid);
		}

		private void LoadUnitsToGrid()
		{
			for (int i = 0; i < _model.GetUnitArray.Length; i++)
			{
				GameUnitInfo info = _model.GetUnitArray[i];
				_grid.CreateUnit(info, _model.Player.Side, new GridPoint(i, 0));
			}
		}
		private void DrawText()
		{
			for (int i = 0; i < _model.GetUnitArray.Length; i++)
			{
				GameUnitInfo info = _model.GetUnitArray[i];

				TextBlock healthText = new TextBlock();
				healthText.Text = $"{info.Name} Цена: {info.Price}";

				healthText.Width = 200;
				healthText.Height = 20;

				healthText.Foreground = new SolidColorBrush(_model.Player.Coins >= info.Price ? Color.FromRgb(0, 255, 0) : Color.FromRgb(255, 0, 0));

				float left = TextDrawOffsetX;
				float top = 0 + TextDrawOffsetY * i;

				Canvas.SetLeft(healthText, left);
				Canvas.SetTop(healthText, top);

				_renderCanvas.Children.Add(healthText);
			}
		}

		public void Draw()
		{
			_gridPresenter.DrawGrid(BattleStage.MOVE);
			DrawText();
			_coinsCountText.Content = $"Количество ресурсов: {_model.Player.Coins}";
		}

		public void BuyUnit(int index)
		{
			try
			{
				_model.BuyUnit(index);
				Draw();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
