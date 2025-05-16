using Moq;
using Moq.Protected;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using HackerNews.Api.Models;
using HackerNews.Api.Services;
using Newtonsoft.Json;

public class HackerNewsServiceTests
{
    private HackerNewsService CreateService(List<int> storyIds, List<Story> storyObjects)
    {
        var responseQueue = new Queue<HttpResponseMessage>();

        responseQueue.Enqueue(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(storyIds))
        });

        foreach (var story in storyObjects)
        {
            responseQueue.Enqueue(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(story))
            });
        }

        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(() => responseQueue.Dequeue());

        var httpClient = new HttpClient(mockHandler.Object);
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        return new HackerNewsService(httpClient, memoryCache, NullLogger<HackerNewsService>.Instance);
    }

    [Fact]
    public async Task GetTopStoriesAsync_ReturnsStoriesWithUrlOnly()
    {
        var service = CreateService(
            new List<int> { 1, 2 },
            new List<Story>
            {
                new Story { Id = 1, Title = "With URL", Url = "http://story.com" },
                new Story { Id = 2, Title = "No URL", Url = null }
            });

        var stories = await service.GetTopStoriesAsync();

        Assert.Single(stories);
        Assert.Equal("With URL", stories.First().Title);
    }

    [Fact]
    public async Task GetTopStoriesAsync_UsesCacheIfAvailable()
    {
        var story = new Story { Id = 1, Title = "Cached Story", Url = "http://cached.com" };
        var service = CreateService(
            new List<int> { 1 },
            new List<Story> { story });

        var result1 = await service.GetTopStoriesAsync();
        var result2 = await service.GetTopStoriesAsync();

        Assert.Single(result2);
        Assert.Equal("Cached Story", result2.First().Title);
    }

    [Fact]
    public async Task GetTopStoriesAsync_ReturnsEmpty_WhenNoIds()
    {
        var service = CreateService(new List<int>(), new List<Story>());
        var result = await service.GetTopStoriesAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTopStoriesAsync_FiltersAllInvalidStories()
    {
        var service = CreateService(
            new List<int> { 1, 2 },
            new List<Story>
            {
                new Story { Id = 1, Title = "Story 1", Url = null },
                new Story { Id = 2, Title = "Story 2", Url = "" }
            });

        var result = await service.GetTopStoriesAsync();
        Assert.Empty(result);
    }
}
