using System;

namespace TableStrategy
{
	/// <summary> Игровой кубик </summary>
	public class GameDice
	{
		public int MinValue = 1;
		public int MaxValue = 6;

		public int Value { get; private set; }
		public bool IsWasMixed { get; private set; }

		public Action OnValueChanged { get; set; }

		public GameDice() 
		{
			Value = MinValue;
			IsWasMixed = false;
		}

		public void DiceMix()
		{
			Value = GameRandomazer.GetOnRange(MinValue, MaxValue);
			IsWasMixed = true;
			OnValueChanged?.Invoke();
		}
	}
}
