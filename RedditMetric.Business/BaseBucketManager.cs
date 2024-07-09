using Microsoft.Extensions.Logging;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;

namespace RedditMetrics.Business
{
    public abstract class BucketManager() : IBucketManager
    {
        private bool disposedValue;
        private readonly object thisLock = new();

        public SubRedditStack Stack { get; } = new SubRedditStack();
        public abstract string Topic { get; }
        public abstract Task PopulateBucket(ILogger logger, ISubredditResult result);       

        protected async Task<bool> BuildStack(ISubredditResult result)
        {
            return await Task.Run(() =>
            {
                SubredditResult res = (SubredditResult)result;
                if (res?.Data?.Children != null && res.Data.Dist > 0 && res.Data.Children.Length != 0)
                {
                    var data = res.Data.Children.Select(x => new SubRedditStackItem()
                    {
                        Comments = x.Data?.Comments ?? 0,
                        Name = x.Data?.Name ?? string.Empty,
                        Title = x.Data?.Title ?? string.Empty,
                        Votes = x.Data?.Ups ?? 0,
                        AuthorName = x.Data?.Author_FullName ?? string.Empty,
                        Author = x.Data?.Author ?? string.Empty,
                        PostedDate = x.Data?.Created_UTC ?? 0,
                    });
                    lock (thisLock)
                    {
                        Stack.AddItem(result.SubRedditName, data.ToArray());
                    }
                    return true;
                }
                return false;
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Stack.Dispose();
                }
                disposedValue = true;
            }
        }

        public  void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
