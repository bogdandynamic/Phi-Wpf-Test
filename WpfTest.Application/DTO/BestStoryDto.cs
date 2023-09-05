using System;
using Newtonsoft.Json;
using WpfTest.Application.Helpers;

namespace WpfTest.Application.DTO
{
	public class BestStoryDto
	{
		public long Id { get; set; }

		public string Title { get; set; } = null!;

		[JsonProperty("by")]
		public string PostedBy { get; set; } = null!;

		public int Score { get; set; }

		[JsonConverter(typeof(UnixToDateTimeConverter))]
		[JsonProperty("time")]
		public DateTime DateTime { get; set; }

		public string Url { get; set; } = null!;
	}
}
