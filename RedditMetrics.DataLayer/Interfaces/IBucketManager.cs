using Microsoft.Extensions.Logging;

namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IBucketManager : IDisposable
    {
        string Topic { get; }
        Task PopulateBucket(ILogger logger, ISubredditResult result);
    }
}
