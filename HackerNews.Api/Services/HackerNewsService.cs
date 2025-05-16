using HackerNews.Api.Interfaces;
using HackerNews.Api.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace HackerNews.Api.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HackerNewsService> _logger;

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache, ILogger<HackerNewsService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<Story>> GetTopStoriesAsync()
        {
            if (_cache.TryGetValue("topstories", out List<Story> cachedStories))
            {
                _logger.LogInformation("Returned stories from cache.");
                return cachedStories;
            }

            var idsResponse = await _httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/topstories.json?print=pretty");
            var ids = JsonConvert.DeserializeObject<List<int>>(idsResponse);

            var result = new ConcurrentBag<Story>();

            await Parallel.ForEachAsync(ids, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (id, _) =>
            {
                try
                {
                    var storyJson = await _httpClient.GetStringAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json?print=pretty");
                    var story = JsonConvert.DeserializeObject<Story>(storyJson);

                    if (!string.IsNullOrEmpty(story?.Url))
                    {
                        result.Add(story);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Failed to fetch story {id}: {ex.Message}");
                }
            });

            var finalList = result.OrderByDescending(s => s.Score).ToList();

            _cache.Set("topstories", finalList, TimeSpan.FromMinutes(5));
            return finalList;
        }
    }
}
