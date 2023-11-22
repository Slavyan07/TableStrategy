using System;
using System.Windows;
using System.Windows.Threading;
using Point = System.Drawing.Point;

namespace TableStrategy
{
	/// <summary> Окно для броска игровых кубиков </summary>
	public partial class DicesView : Window
	{
		public int MixCount = 10;

		private DispatcherTimer _timer = new DispatcherTimer();
		private int _mixCount = 0;

		private DicePresenter _dicePresenter1;
		private DicePresenter _dicePresenter2;

		private GameDice _dice1;
		private GameDice _dice2;

		public bool IsWasMixed => _dice1.IsWasMixed && _dice2.IsWasMixed;
		public int Result { get; private set; }

		public DicesView()
		{
			InitializeComponent();
			_timer.Interval = TimeSpan.FromMilliseconds(50);
			_timer.Tick += new EventHandler(TimerUpdate);
			_dice1 = new GameDice();
			_dice2 = new GameDice();
			_dicePresenter1 = new DicePresenter(_dice1, DrawCanvas, new Point(10, 60));
			_dicePresenter2 = new DicePresenter(_dice2, DrawCanvas, new Point(10, 220));
			_dice1.OnValueChanged += Draw;
			Draw();
		}

		private void TimerUpdate(object sender, EventArgs e) => DiceMixing();
		private void Button_Click(object sender, RoutedEventArgs e) => DiceMixingStart();

		private void DiceMixingStart()
		{
			DiceButton.IsEnabled = false;
			_mixCount = MixCount;
			_timer.Start();
		}
		private void DiceMixing()
		{
			if (_mixCount-- > 0)
			{
				DiceMix();
				return;
			}

			DiceMixiingEnd();
		}
		private void DiceMix()
		{
			_dicePresenter1.DiceMixing();
			_dicePresenter2.DiceMixing();
			Draw();
		}
		private void DiceMixiingEnd()
		{
			_timer.Stop();
			DiceButton.IsEnabled = true;
			Result = _dice1.Value + _dice2.Value;

			this.Hide();
		}

		public void Draw()
		{
			DrawCanvas.Children.Clear();
			_dicePresenter1.Draw();
			_dicePresenter2.Draw();
		}
	}
}
