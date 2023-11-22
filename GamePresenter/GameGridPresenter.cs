using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TableStrategy
{
	public class GameGridPresenter
	{
		protected Canvas _renderCanvas;
		protected GameGrid _model;

		public const int CellWidth = 50;
		public const int CellHeight = 50;

		public GameGridPresenter(Canvas canvas, GameGrid model)
		{
			_renderCanvas = canvas;
			_model = model;
		}

		private void DrawLines()
		{
			for (int i = 0; i <= _model.Height; i++)
			{
				Line map_line = new Line();
				map_line.Stroke = System.Windows.Media.Brushes.Black;
				map_line.StrokeThickness = 2;

				map_line.X1 = i * CellWidth;
				map_line.Y1 = 0;

				map_line.X2 = i * CellWidth;
				map_line.Y2 = _model.Width * CellHeight;

				map_line.HorizontalAlignment = HorizontalAlignment.Left;
				map_line.VerticalAlignment = VerticalAlignment.Center;

				_renderCanvas.Children.Add(map_line);
			}

			for (int i = 0; i <= _model.Width; i++)
			{
				Line map_line = new Line();
				map_line.Stroke = System.Windows.Media.Brushes.Black;
				map_line.StrokeThickness = 2;

				map_line.X1 = 0;
				map_line.Y1 = i * CellHeight;

				map_line.X2 = _model.Height * CellWidth;
				map_line.Y2 = i * CellHeight;

				map_line.HorizontalAlignment = HorizontalAlignment.Left;
				map_line.VerticalAlignment = VerticalAlignment.Center;

				_renderCanvas.Children.Add(map_line);
			}
		}
		public virtual void DrawUnit(GameUnit unit)
		{
			int x = unit.UnitTransform.Position.X;
			int y = unit.UnitTransform.Position.Y;

			Image sector = new Image
			{
				Width = CellWidth,
				Height = CellHeight,
				Source = new BitmapImage(new Uri(unit.GetRenderImage())),
				Name = $"Sector{x}_{y}",
			};

			sector.FlowDirection = (unit.GameSide == GameSide.LEFT) ? (FlowDirection.LeftToRight) : (FlowDirection.RightToLeft);

			Canvas.SetTop(sector, y * CellHeight);
			Canvas.SetLeft(sector, x * CellWidth);

			_renderCanvas.Children.Add(sector);
		}
		private void DrawUnitHealth(float x, float y, string health)
		{
			TextBlock healthText = new TextBlock();
			healthText.Text = health;

			healthText.Width = CellWidth;
			healthText.Height = CellHeight;

			healthText.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));

			float top = y * CellHeight;
			float left = x * CellWidth;

			Canvas.SetLeft(healthText, left);
			Canvas.SetTop(healthText, top);

			_renderCanvas.Children.Add(healthText);
		}

		public bool CellClickCheck(int x, int y)
		{
			if (x < 0 || x >= _model.Height)
				return false;

			if (y < 0 || y >= _model.Width)
				return false;

			return true;
		}
		public void DrawNullSelectCell(GridPoint position)
		{
			Image sector = new Image
			{
				Width = CellWidth,
				Height = CellHeight,
				Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\Resources\\NullCell.png")),
				Name = $"Sector{position.X}_{position.Y}",

			};
			Canvas.SetTop(sector, position.Y * CellHeight);
			Canvas.SetLeft(sector, position.X * CellWidth);

			_renderCanvas.Children.Add(sector);
		}
		public virtual void DrawGrid(BattleStage battleStage)
		{
			DrawLines();

			var unitsDrawList = from unit in _model.FullUnitList
						   where unit.Alive || (!unit.Alive && !_model.FullUnitList.Any(u => u.Alive && u.UnitTransform.Position == unit.UnitTransform.Position))
						   select unit;

			foreach (GameUnit unit in unitsDrawList)
			{
				DrawUnit(unit);
				DrawUnitHealth(unit.UnitTransform.Position.X, unit.UnitTransform.Position.Y, unit.HealthString);
			}
		}
	}
}
