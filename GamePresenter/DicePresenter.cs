using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Drawing;

namespace TableStrategy
{
	public class DicePresenter
	{
		public const int Width = 150;
		public const int Height = 150;

		public const int MixCount = 5;
		public const int MixTime = 500;

		private readonly string[] DiceValuesView = new string[6]
		{
			Environment.CurrentDirectory + $"\\Resources\\Dices\\Dice_1.png",
			Environment.CurrentDirectory + $"\\Resources\\Dices\\Dice_2.png",
			Environment.CurrentDirectory + $"\\Resources\\Dices\\Dice_3.png",
			Environment.CurrentDirectory + $"\\Resources\\Dices\\Dice_4.png",
			Environment.CurrentDirectory + $"\\Resources\\Dices\\Dice_5.png",
			Environment.CurrentDirectory + $"\\Resources\\Dices\\Dice_6.png",
		};

		private GameDice _model;
		private Canvas _renderCanvas;

		private Point _position;

		public bool IsWasMixed => _model.IsWasMixed;

		public DicePresenter(GameDice model, Canvas canvas, Point position) 
		{
			_model = model;
			_renderCanvas = canvas;
			_position = position;
		}

		public void Draw()
		{
			int value = _model.Value;

			Image sector = new Image
			{
				Width = Width,
				Height = Height,
				Source = new BitmapImage(new Uri(DiceValuesView[value - 1])),
				Name = $"Dice_{value}",
			};

			Canvas.SetTop(sector, _position.X);
			Canvas.SetLeft(sector, _position.Y);

			_renderCanvas.Children.Add(sector);
		}

		public void DiceMixing() => _model.DiceMix();

	}
}
