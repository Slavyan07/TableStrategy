using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TableStrategy
{
	/// <summary> Тип ячейки на карте </summary>
	public enum MapCell
    {
        NONE,
		MOUNTAIN,
		RESOURCE_SOURCE,
		DEFEND_BOOST,
	}

	[DataContract]
	/// <summary> Игровая карта </summary>
	public class GameMap
	{
		[DataMember]
		private int _width;
		[DataMember]
		private int _height;
		[DataMember]
		private MapCell[,] _map;

		public GameMap(int width, int height, int mountainsCount, int defendCellsCount, int resoursesCellsCount)
		{
			_width = height;
			_height = width;
			MapCreate(width, height);
			_map = MapFill(_map, mountainsCount, defendCellsCount, resoursesCellsCount);
		}

		private MapCell[,] MapCreate(int width, int height)
		{ 
			_map = new MapCell[height, width];

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					_map[i, j] = MapCell.NONE;
				}
			}

			return _map;
		}

		private MapCell[,] CellsCreate(MapCell[,] map, MapCell cellType, int count)
		{
			while (count-- > 0)
			{
				int x = GameRandomazer.GetOnRange(1, _width - 2);
				int y = GameRandomazer.GetOnRange(0, _height - 1);

				if (_map[x, y] == MapCell.NONE)
					_map[x, y] = cellType;
			}

			return map;
		}

		/// <summary> Генерация других ячеек на карте </summary>
		private MapCell[,] MapFill(MapCell[,] map, int mountainsCount, int defendCellsCount, int resoursesCellsCount)
		{
			_map = CellsCreate(map, MapCell.MOUNTAIN, mountainsCount);
			_map = CellsCreate(map, MapCell.DEFEND_BOOST, defendCellsCount);
			_map = CellsCreate(map, MapCell.RESOURCE_SOURCE, resoursesCellsCount);

			return map;
		}

		public MapCell GetCell(int x, int y)
		{
			try
			{
				return _map[y, x];
			}
			catch
			{
				throw new Exception("Заданной точки не существует на карте");
			}
		}

		public MapCell GetCell(GridPoint position) => GetCell(position.Y, position.X);

		public List<(MapCell, GridPoint)> GetFullMap()
		{
			List<(MapCell, GridPoint)> fullMap = new List<(MapCell, GridPoint)>(0);

			for (int y = 0; y < _height; y++)
			{
				for (int x = 0; x < _width; x++)
				{
					fullMap.Add((_map[x, y], new GridPoint(y, x)));
				}
			}

			return fullMap;
		}
	}
}
