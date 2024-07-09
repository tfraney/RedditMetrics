
using Microsoft.Extensions.Logging;

namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IRedditReader : IDisposable
    {
        Task<(ISubredditResult?, IHeaderData?)> ReadQuery(ILogger logger, string subreddit, string log,
                                                          string action, string token, int cnt);
    }   
}
