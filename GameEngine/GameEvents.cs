using System;

namespace TableStrategy
{
	/// <summary> Обработка игровых событий </summary>
	public class GameEvents
	{
		public static Action<Player>? OnGameEnd { get; set; }
		public static Func<int>? OnGetDiceBonus { get; set; }
		public static Action? OnNewRound { get; set; }
		public static Action? OnGameClose { get; set; }


		public static void SendGameEnd(Player winPlayer) => OnGameEnd?.Invoke(winPlayer);
		public static int SendDiceBonus() => OnGetDiceBonus.Invoke();
		public static void SendNewRound() => OnNewRound?.Invoke();
		public static void SendGameClose() => OnGameClose?.Invoke();
	}
}
