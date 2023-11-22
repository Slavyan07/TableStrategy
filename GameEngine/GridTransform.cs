using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TableStrategy
{
	[DataContract]
	/// <summary> Позиция на игровой сетке </summary>
	public struct GridPoint
	{
		[DataMember]
		public int X { get; private set; }
		[DataMember]
		public int Y { get; private set; }

		public GridPoint()
		{
			X = 0;
			Y = 0;
		}

		public GridPoint(int x, int y)
		{
			X = y;
			Y = x;
		}

		public static GridPoint operator +(GridPoint point1, GridPoint point2) => new GridPoint(point1.X + point2.X, point1.Y + point2.Y);
		public static GridPoint operator -(GridPoint point1, GridPoint point2) => new GridPoint(point1.X - point2.X, point1.Y - point2.Y);

		public static bool operator !=(GridPoint point1, GridPoint point2) => point1.X != point2.X || point1.Y != point2.Y;
		public static bool operator ==(GridPoint point1, GridPoint point2) => point1.X == point2.X && point1.Y == point2.Y;
	}

	[DataContract]
	/// <summary> Элемент управления положением на игровой сетке </summary>
	public class GridTransform
	{
		[DataMember]
		public GridPoint Position { get; private set; }

		public GridTransform(GridPoint point)
		{
			Position = point;
		}

		public void SetPosition(GridPoint point)
		{
			if (point.X < 0)
				throw new Exception("X - значение не может быть отрицательным");

			if (point.Y < 0)
				throw new Exception("Y - значение не может быть отрицательным");

			Position = point;
		}
		public List<GridPoint> CellsAround(int distance)
		{
			List<GridPoint> list = new List<GridPoint>(0);

			int startY = Position.Y - distance < 0 ? 0 : Position.Y - distance;
			int startX = Position.X - distance < 0 ? 0 : Position.X - distance;

			for (int y = startY; y < Position.Y + distance + 1; y++)
			{
				for (int x = startX; x < Position.X + distance + 1; x++)
				{
					list.Add(new GridPoint(y, x));
				}
			}

			return list;
		}

		public List<GridPoint> CellAroundCircle(int distance)
		{
			List<GridPoint> list = CellsAround(distance);

			int offsetUp = distance;
			int offsetDown = distance;

			for (int y = Position.Y - distance; y < Position.Y; y++)
			{
				for (int x = Position.X - distance; x < Position.X - distance + offsetUp; x++)
					list.Remove(new GridPoint(y, x));

				for (int x = Position.X + distance; x > Position.X + distance - offsetUp; x--)
					list.Remove(new GridPoint(y, x));

				offsetUp--;
			}

			for (int y = Position.Y + distance; y > Position.Y; y--)
			{
				for (int x = Position.X - distance; x < Position.X - distance + offsetDown; x++)
					list.Remove(new GridPoint(y, x));

				for (int x = Position.X + distance; x > Position.X + distance - offsetDown; x--)
					list.Remove(new GridPoint(y, x));

				offsetDown--;
			}

			return list;
		}
	}
}
