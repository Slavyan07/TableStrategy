using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TableStrategy
{
	public class PresenterBehavior : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
