using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WpfTest.Application.DTO;
using WpfTest.Application.Helpers;
using WpfTest.Application.Interfaces;

namespace WpfTest.UI.ViewModels;

public class MainWindowViewModel : BindableBase
{
	private readonly IHackerNewsApiService _hackerNewsApiService;
	private readonly ICustomLogger _logger;
		
	public RelayCommand RefreshCommand { get; set;} 
	public RelayCommand OpenUrlCommand { get; set; }

	public ObservableCollection<BestStoryDto> BestStories { get; set; } = new();

	private bool _loadingStories;

	public MainWindowViewModel(
		IHackerNewsApiService hackerNewsApiService,
		ICustomLogger customLogger)
	{
		_hackerNewsApiService = hackerNewsApiService;
		_logger = customLogger;
		RefreshCommand = new RelayCommand(OnRefreshBestStories, CanRefreshBestStories);
		OpenUrlCommand = new RelayCommand(OnOpenUrlCommand, CanOpenUrlCommand);
		
		OnRefreshBestStories();
	}

	private async void OnRefreshBestStories(object? o = null) { 
		_loadingStories = true;

		BestStories.Clear();
			
		try
		{
			await foreach (BestStoryDto? story in GetBestStories().ConfigureAwait(false))
			{
				if (story is not null)
				{
					// Because the HTTP calls are run on a different thread, we MUST synchronize context
					// and get help from the Dispatcher (main UI thread)
					System.Windows.Application.Current.Dispatcher.Invoke(() =>
					{
						InsertIntoOrderedCollection(BestStories, (a, b) => b.DateTime.CompareTo(a.DateTime), story);
					});
				}
			}
		}
		catch (Exception e)
		{
			_logger.Error(e, "Could not load the best stories");
		}
		finally
		{
			_loadingStories = false;
		}
	}

	/// <summary>
	/// Because the stories come as IAsyncEnumerable they can be unordered by DateTime
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="collection"></param>
	/// <param name="comparison"></param>
	/// <param name="modelToInsert"></param>
	private static void InsertIntoOrderedCollection<T>(ObservableCollection<T> collection, Comparison<T> comparison, T modelToInsert)
	{
		if (collection.Count is 0)
		{
			collection.Add(modelToInsert);
		}
		else
		{
			int index = 0;
			foreach (T model in collection)
			{
				if (comparison(model, modelToInsert) >= 0)
				{
					collection.Insert(index, modelToInsert);
					return;
				}

				index++;
			}

			collection.Insert(index, modelToInsert);
		}
	}

	private bool CanRefreshBestStories(object? o = null) 
	{ 
		return !_loadingStories; 
	}

	/// <summary>
	/// This method returns stories as an async enumerable.
	/// We'll use the await foreach construct for this.
	/// </summary>
	/// <returns></returns>
	private async IAsyncEnumerable<BestStoryDto?> GetBestStories()
	{
		IList<long> topStoryIds = await _hackerNewsApiService.GetBestStoriesIds()
			.ConfigureAwait(false);
			
		List<Task<BestStoryDto?>> getTopStoryTaskList = topStoryIds.Select(_hackerNewsApiService.GetBestStory)
			.ToList();
			
		int storyCount = int.MaxValue;
		while (getTopStoryTaskList.Any() && storyCount-- > 0)
		{
			Task<BestStoryDto?> completedGetStoryTask = await Task.WhenAny(getTopStoryTaskList)
				.ConfigureAwait(false);
			getTopStoryTaskList.Remove(completedGetStoryTask);

			BestStoryDto? story = await completedGetStoryTask.ConfigureAwait(false);
				
			yield return story;
		}
	}

	private void OnOpenUrlCommand(object? parameter)
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = (parameter as string)!,
			UseShellExecute = true
		});
	}

	private bool CanOpenUrlCommand(object? parameter)
	{
		return !string.IsNullOrEmpty(parameter as string);
	}
}