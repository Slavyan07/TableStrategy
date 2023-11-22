using System;

namespace TableStrategy
{
	/// <summary> Гнератор случайных чисел </summary>
	public static class GameRandomazer
	{
		private static int _ticks = 1;

		public static int GetOnRange(int a, int b)
		{
			var random = new Random((int)DateTime.Now.Ticks + _ticks);
			int value = random.Next(a, b + 1);
			_ticks++;

			return value;
		}

	}
}
