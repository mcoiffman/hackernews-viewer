using HackerNews.Api.Interfaces;
using HackerNews.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService;

        public StoriesController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Story>>> GetTopStories()
        {
            var stories = await _hackerNewsService.GetTopStoriesAsync();
            return Ok(stories);
        }
    }
}
