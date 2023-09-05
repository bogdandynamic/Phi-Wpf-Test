using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using WpfTest.Application.Interfaces;
using WpfTest.UI.ViewModels;

namespace WpfTest.UI.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ICustomLogger _logger;

		public MainWindow(ICustomLogger logger)
		{
			_logger = logger;
			InitializeComponent();

			// Initialize ViewModel through DI container in order to have dependencies injected into it
			DataContext = App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
		}

		protected override void OnContentRendered(EventArgs e)
		{
			base.OnContentRendered(e);

			_logger.Info("MainWindow rendered");
		}
	}
}
