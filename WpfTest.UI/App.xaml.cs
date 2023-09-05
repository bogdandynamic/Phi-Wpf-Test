using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WpfTest.Application.Interfaces;
using WpfTest.Infrastructure.Services;
using WpfTest.UI.ViewModels;
using WpfTest.UI.Views;

namespace WpfTest.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
	public static ServiceProvider ServiceProvider { get; private set; } = null!;

	public App()
	{
		ServiceCollection serviceCollection = new();
		ConfigureServices(serviceCollection);
		ServiceProvider = serviceCollection.BuildServiceProvider();

		AppDomain.CurrentDomain.UnhandledException += (_, args) =>
		{
			ICustomLogger? logger = ServiceProvider.GetService<ICustomLogger>();
			logger?.Error(args.ExceptionObject as Exception, "An error happened");
		};
	}

	private static void ConfigureServices(IServiceCollection services)
	{
		services.AddSingleton<MainWindow>();
		services.AddSingleton<ICustomLogger>(_ => new CustomLogger());
		services.AddSingleton<IHackerNewsApiService>(_ => new HackerNewsApiService(Constants.HackerNewsApiUrl));

		services.AddSingleton<MainWindowViewModel>();
	}
    
	private void OnStartup_Custom(object sender, StartupEventArgs e)
	{
		MainWindow? mainWindow = ServiceProvider.GetService<MainWindow>();
		mainWindow?.Show();
	}
}
