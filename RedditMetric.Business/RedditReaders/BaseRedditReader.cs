
using Microsoft.Extensions.Logging;
using RedditMetrics.DataLayer.Interfaces;

namespace RedditMetrics.Business.RedditReaders
{
    public abstract class BaseRedditReader : IRedditReader
    {
        private bool disposedValue;

        public abstract Task<(ISubredditResult?, IHeaderData?)> ReadQuery(ILogger logger, string subreddit, string log,
                                                                               string action, string token, int cnt);
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {             
                disposedValue = true;
            }
        }       

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
