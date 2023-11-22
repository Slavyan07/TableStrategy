using System;
using System.Collections.Generic;

namespace TableStrategy
{
	/// <summary> Инфомация о всех юнитах в игре </summary>
	public class GameUnitDatabase
	{
		private static GameUnitDatabase _instance;

		public static GameUnitDatabase GetInstance()
		{
			if (_instance == null)
				_instance = new GameUnitDatabase();

			return _instance;
		}

		private List<GameUnitInfo> _leftSideGameUnits;
		private List<GameUnitInfo> _rightSideGameUnits;

		public GameUnitDatabase()
		{
			_leftSideGameUnits = new List<GameUnitInfo>()
			{ 
				new GameUnitInfo("Воин", 1, 3, 3, 5, 2, 1, 3, "Warrior"),
				new GameUnitInfo("Копейщик", 2, 3, 3, 8, 2, 1, 3, "Warrior2"),
			};

			_rightSideGameUnits = new List<GameUnitInfo>()
			{ 
				new GameUnitInfo("Скелет", 1, 3, 3, 3, 2, 1, 3, "Sceleton"),
				new GameUnitInfo("Скелет-Лучник", 2, 2, 3, 5, 2, 3, 3, "SkeletonArcher"),
			};
		}

		public GameUnitInfo GetLeftUnit(int id)
		{
			if (id < 0)
				throw new Exception("Индекс не можеть быть меньше 0");

			if(_leftSideGameUnits.Count <= id)
				throw new Exception("Индекс выходит за границу массива");

			return _leftSideGameUnits[id];
		}
		public GameUnitInfo GetLeftUnit()
		{
			int index = GameRandomazer.GetOnRange(0, _leftSideGameUnits.Count - 1);
			return GetLeftUnit(index);
;		}
		public GameUnitInfo GetRightUnit(int id)
		{
			if (id < 0)
				throw new Exception("Индекс не можеть быть меньше 0");

			if (_rightSideGameUnits.Count <= id)
				throw new Exception("Индекс выходит за границу массива");

			return _rightSideGameUnits[id];
		}
		public GameUnitInfo GetRightUnit()
		{
			int index = GameRandomazer.GetOnRange(0, _rightSideGameUnits.Count - 1);
			return GetRightUnit(index);
		}

		public List<GameUnitInfo> GetFullLeftSideUnitList() => _leftSideGameUnits;
		public List<GameUnitInfo> GetFullRightSideUnitList() => _rightSideGameUnits;
	}
}
