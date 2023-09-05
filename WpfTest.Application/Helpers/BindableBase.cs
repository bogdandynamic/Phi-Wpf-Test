using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfTest.Application.Helpers
{
	/// <summary>
	/// Base class for ViewModels
	/// </summary>
	public class BindableBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged = delegate { };

		protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(field, value))
			{
				return false;
			}
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}
