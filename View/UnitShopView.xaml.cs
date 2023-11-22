using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TableStrategy
{
	/// <summary> Окно магазина для игроков </summary>
	public partial class UnitShopView : Window
	{
		private GameUnitShop _unitShop;
		private GameShopPresenter _presenter;

		private Button[] _buttons;

		public UnitShopView(Player player, BattleGrid battleGrid)
		{
			InitializeComponent();
			_buttons = new Button[]
			{
				Button1,
				Button2,
				Button3,
				Button4,
				Button5,
			};

			_unitShop = new GameUnitShop(battleGrid, player);
			_presenter = new GameShopPresenter(_unitShop, RenderCanvas, _buttons, CoinsText);
			_presenter.Draw();
		}

		private void RenderCanvas_MouseUp(object sender, MouseButtonEventArgs e)
		{}

		private void Button1_Click(object sender, RoutedEventArgs e) => _presenter.BuyUnit(0);
		private void Button2_Click(object sender, RoutedEventArgs e) => _presenter.BuyUnit(1);
		private void Button3_Click(object sender, RoutedEventArgs e) => _presenter.BuyUnit(2);
		private void Button4_Click(object sender, RoutedEventArgs e) => _presenter.BuyUnit(3);
		private void Button5_Click(object sender, RoutedEventArgs e) => _presenter.BuyUnit(4);
	}
}
