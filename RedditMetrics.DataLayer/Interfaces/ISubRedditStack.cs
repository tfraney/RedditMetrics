
namespace RedditMetrics.DataLayer.Interfaces
{
    public interface ISubRedditStack : IDisposable
    {
        Dictionary<string, ISubRedditStackItem[]> Items { get; }

        void AddItem(string subreddit, ISubRedditStackItem[] value);
    }

    public interface ISubRedditStackItem
    {
        string Name { get; set; }
        string Author { get; set; }
        string AuthorName { get; set; }
        string Title { get; set; }
        int Votes { get; set; }
        int Comments { get; set; }
        decimal PostedDate { get; set; }
    }

    public interface IAuthorStackItem
    {       
        string Author { get; set; }

        public string Author_Name { get; set; }
        int Posts { get; set; }
    }
}
