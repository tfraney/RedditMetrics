
using Microsoft.Extensions.Logging;

namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IRedditReader : IDisposable
    {        
        Task<(ISubredditResult?, IHeaderData?)> ReadQuery(ILogger logger, string subreddit, string log,
                                                          string action, string authdata, int cnt, string? before, string? after);

        Task<IHeaderData> TryAuth(ILogger logger, string client_name, string loginData, string basicauth);
    }   
}
