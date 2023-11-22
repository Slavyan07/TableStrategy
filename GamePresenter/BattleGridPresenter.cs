using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TableStrategy
{
	public class BattleGridPresenter : GameGridPresenter
	{

		public BattleGridPresenter(Canvas canvas, BattleGrid model) : base(canvas, model)
		{}

		private BattleGrid GetModel() => (BattleGrid)_model;

		private void DrawSelectedCells(BattleStage battleStage)
		{
			Uri imageSource = battleStage == BattleStage.MOVE ? new Uri(Environment.CurrentDirectory + $"\\Resources\\MoveCell.png") : new Uri(Environment.CurrentDirectory + $"\\Resources\\AttackCell.png");

			foreach (GridPoint selectedCell in GetModel().SelectedCells)
			{ 
				int x = selectedCell.X;
				int y = selectedCell.Y;

				Image sector = new Image
				{
					Width = CellWidth,
					Height = CellHeight,
					Source = new BitmapImage(imageSource),
					Name = $"Sector{x}_{y}",
				};

				Canvas.SetTop(sector, y * CellHeight);
				Canvas.SetLeft(sector, x * CellWidth);

				_renderCanvas.Children.Add(sector);
			}
		}

		public void SelectedCellsNull() => GetModel().ClearSelect();

		public override void DrawGrid(BattleStage battleStage)
		{
			_renderCanvas.Children.Clear();
			DrawMap();
			base.DrawGrid(battleStage);
			DrawSelectedCells(battleStage);

			if (GetModel().SelectedUnit != null)
				DrawNullSelectCell(GetModel().SelectedUnit.UnitTransform.Position);

		}

		private Uri GetMapImage(MapCell mapCell)
		{
			switch (mapCell)
			{
				case MapCell.NONE: return new Uri(Environment.CurrentDirectory + "\\Resources\\GrassCell.png");
				case MapCell.MOUNTAIN: return new Uri(Environment.CurrentDirectory + "\\Resources\\MountainCell.png");
				case MapCell.RESOURCE_SOURCE: return new Uri(Environment.CurrentDirectory + "\\Resources\\ResourceCell.png");
				case MapCell.DEFEND_BOOST: return new Uri(Environment.CurrentDirectory + "\\Resources\\DefendCell.png");

				default: return new Uri("");
			}
		}

		private void DrawMap()
		{
			for (int x = 0; x < _model.Width; x++)
			{
				for (int y = 0; y < _model.Height; y++)
				{
					Image sector = new Image
					{
						Width = CellWidth,
						Height = CellHeight,
						Source = new BitmapImage(GetMapImage(GetModel().GameMap.GetCell(x, y))),
						Name = $"Sector{x}_{y}",
					};

					Canvas.SetTop(sector, x * CellHeight);
					Canvas.SetLeft(sector, y * CellWidth);

					_renderCanvas.Children.Add(sector);
				}
			}
		}

		private void DrawCellColor(GridPoint position, Uri imagePath)
		{
			Image cell = new Image
			{
				Width = CellWidth,
				Height = CellHeight,
				Source = new BitmapImage(imagePath),
				Name = $"Sector{position.X}_{position.Y}",
			};

			Canvas.SetTop(cell, position.Y * CellHeight);
			Canvas.SetLeft(cell, position.X * CellWidth);

			_renderCanvas.Children.Add(cell);
		}

		public override void DrawUnit(GameUnit unit)
		{
			base.DrawUnit(unit);
			GridPoint position = unit.UnitTransform.Position;
			MapCell mapCell = GetModel().GameMap.GetCell(position);
			if (mapCell == MapCell.RESOURCE_SOURCE)
				DrawCellColor(position, new Uri(Environment.CurrentDirectory + "\\Resources\\ResourceCellColor.png"));
			if(mapCell == MapCell.DEFEND_BOOST)
				DrawCellColor(position, new Uri(Environment.CurrentDirectory + "\\Resources\\DefendCellColor.png"));
		}

		public void SelectCell(int x, int y, Player currentPlayerTurn, BattleStage battleStage)
		{
			GetModel().SelectCell(x, y, currentPlayerTurn, battleStage);
			DrawGrid(battleStage);
		}
	}
}
