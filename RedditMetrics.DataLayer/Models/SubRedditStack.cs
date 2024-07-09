

using RedditMetrics.DataLayer.Interfaces;

namespace RedditMetrics.DataLayer.Models
{
    public class SubRedditStack : ISubRedditStack
    {
        private readonly object _thisLock = new();
        private bool disposedValue;

        public Dictionary<string,ISubRedditStackItem[]> Items { get; private set; } = [];

        public ISubRedditStackItem[] GetBucketList(string name)
        {
            lock (_thisLock)
            {
                return Items.TryGetValue(name, out ISubRedditStackItem[]? value) ? value : [];
            }
        }

        public void AddItem(string subreddit, ISubRedditStackItem[] value)
        {
            lock (_thisLock)
            {
                if (!Items.TryAdd(subreddit, value)) Items[subreddit] = value;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Items.Clear();
                } 
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

    public class SubRedditStackItem : ISubRedditStackItem
    {
        public required string Name { get; set; }

        public required string AuthorName { get; set; }
        public required string Author { get; set; }
        public required string Title { get; set; }
        public required int  Votes { get; set; }
        public required int Comments { get; set; }
        public required decimal PostedDate { get; set; }

    }

    public class AuthorStackItem:  IAuthorStackItem
    {
        public required string Author { get; set; }
        public required string Author_Name { get; set; }
        public required int Posts { get; set; }
    }
}
