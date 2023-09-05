using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfTest.UI;
using WpfTest.UI.ViewModels;
using Xunit;

namespace WpfTest.Tests.Functional;

public class MainWindowTests
{
	[UIFact]
	public void RefreshShouldLoadBestStories()
	{
		// Preparing fixture for running in synchronization context
		Thread thread = new(() =>
		{
			App application = new();
			application.InitializeComponent();
			application.Dispatcher.InvokeAsync(() =>
			{
				_TestApplication(application).ConfigureAwait(false);
			}, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
			application.Run();
		});

		thread.SetApartmentState(ApartmentState.STA);
		thread.Start();
		thread.Join();
	}

	private static async Task _TestApplication(System.Windows.Application application)
	{
		// ARRANGE
		Window? window = application.MainWindow;
		MainWindowViewModel? viewModel = (MainWindowViewModel?)window?.DataContext;

		// ACT
		// Wait for initial loading of best stories
		await Task.Delay(TimeSpan.FromSeconds(5));
		int loadedStoriesCount = viewModel?.BestStories.Count ?? 0;
		viewModel?.RefreshCommand.Execute(null);
		await Task.Delay(TimeSpan.FromSeconds(5));
		int refreshedStoriesCount = viewModel?.BestStories.Count ?? 0;
		window?.Close();

		// ASSERT
		Assert.NotNull(viewModel);
		Assert.NotNull(window);
		Assert.NotEqual(0, loadedStoriesCount);
		Assert.NotEqual(0, refreshedStoriesCount);
		Assert.Equal(loadedStoriesCount, refreshedStoriesCount);
	}
}