using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TableStrategy
{
	[DataContract]
	/// <summary> Игрок </summary>
	public class Player
	{
		[DataMember]
		public string Name { get; private set; }
		[DataMember]
		public int Coins { get; private set; }
		[DataMember]
		public GameSide Side { get; private set; }
		[DataMember]
		public List<GameUnit> GameUnits { get; private set; }

		public Player(GameSide gameSide, string name)
		{
			GameUnits = new List<GameUnit>(0);
			Side = gameSide;

			Coins = 0;
			Name = name;
		}

		public void CoinsTop(int value) => Coins += value;
		public void CoinsSpend(int value) => Coins -= value;
	}
}
