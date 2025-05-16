using HackerNews.Api.Models;

namespace HackerNews.Api.Interfaces
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<Story>> GetTopStoriesAsync();
    }
}
