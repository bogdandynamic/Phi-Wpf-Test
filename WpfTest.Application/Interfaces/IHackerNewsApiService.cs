using System.Collections.Generic;
using System.Threading.Tasks;
using WpfTest.Application.DTO;

namespace WpfTest.Application.Interfaces
{
	/// <summary>
	/// Interface for HackerNewsApiService
	/// </summary>
	public interface IHackerNewsApiService
	{
		/// <summary>
		/// Get the best stories Ids list
		/// </summary>
		/// <returns>List/array of long values</returns>
		Task<IList<long>> GetBestStoriesIds();

		/// <summary>
		/// Get a story by Id
		/// </summary>
		/// <param name="storyId">The story Id</param>
		/// <returns>The deserialized story, null if not found by Id</returns>
		Task<BestStoryDto?> GetBestStory(long storyId);
	}
}
