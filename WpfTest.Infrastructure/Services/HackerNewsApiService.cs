using Flurl;
using Flurl.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfTest.Application.DTO;
using WpfTest.Application.Interfaces;

namespace WpfTest.Infrastructure.Services
{
	/// <summary>
	/// Implementation of IHackerNewsApiService using Flurl.Http client.
	/// </summary>
	public class HackerNewsApiService : IHackerNewsApiService
	{
		private readonly string _baseUrl;

		public HackerNewsApiService(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		/// <inheritdoc />
		public async Task<IList<long>> GetBestStoriesIds()
		{
			try
			{
				return await _baseUrl.AppendPathSegment("beststories.json").GetJsonAsync<IList<long>>();
			}
			catch
			{
				return new List<long>();
			}
		}

		/// <inheritdoc />
		public async Task<BestStoryDto?> GetBestStory(long storyId)
		{
			try
			{
				return await _baseUrl.AppendPathSegments("item", $"{storyId}.json").GetJsonAsync<BestStoryDto>();
			}
			catch
			{
				return null;
			}
		}
	}
}
