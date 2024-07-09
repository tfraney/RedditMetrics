using RedditMetrics.Business;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;

namespace RedditMetricsSmartReporter.Managers
{
    public class NewBucketManager(IMetricData metrics) : BucketManager
    {
        private readonly object thisLock = new();
        public Dictionary<string, Dictionary<string, (string,int)>> AuthorCount { get; } = [];
      
        public override string Topic { get => RedditMetrics.DataLayer.FunctionConstants.POSTS_NEW; }

        public IAuthorStackItem[]? GetHighestPostingAuthors(string subreddit)
        {
            if (AuthorCount.TryGetValue(subreddit, out Dictionary<string, (string,int)>? value))
            {
                return value.OrderByDescending(x => x)?.Select(x => new AuthorStackItem()
                {
                     Author_Name = x.Value.Item1,
                     Author = x.Key,
                     Posts = x.Value.Item2
                })?.ToArray();
            }
            else return [];
        }

        public override async Task PopulateBucket(ILogger logger, ISubredditResult result)
        {
            var subreddit = result.SubRedditName;
            logger.LogInformation(@"Stacking New Bucket-{subreddit}", subreddit);

            if (await BuildStack(result) && (Stack.Items[subreddit]?.Length ?? 0) != 0)
            {

                logger.LogInformation(@"Successful: Adding New Metrics {subreddit}", subreddit);
                var topmost = Stack.Items[subreddit].First();

                metrics.AddNewSubReddit(subreddit, new MetricItem()
                {
                    Name = @"Latest Post",
                    Description = $"{topmost.Name} : {topmost.Title} - {topmost.Author}",
                    ValueName = @" Posted On",
                    Value = topmost.PostedDate.ToString(),
                });

                lock (thisLock)
                {
                    foreach (var item in Stack.Items[subreddit])
                    {
                        if (!AuthorCount.ContainsKey(subreddit)) AuthorCount.Add(subreddit, []);
                        if (!AuthorCount[subreddit].TryAdd(item.Author, (item.AuthorName, 1)))
                             AuthorCount[subreddit][item.Author] = (AuthorCount[subreddit][item.Author].Item1,
                                                                   AuthorCount[subreddit][item.Author].Item2+1);
                    }
                    if ((AuthorCount[subreddit]?.Keys?.Count ?? 0) > 0)
                    {
                        logger.LogInformation(@"New Authors found for {subreddit}", subreddit);

                        var topmostAuthor = AuthorCount[subreddit].OrderByDescending(x => x.Value.Item2).ThenBy(x => x.Key).First();
                        metrics.AddNewSubReddit(subreddit, new MetricItem()
                        {
                            Name = @"Top Contributer",
                            Description = $"{topmostAuthor.Key}",
                            ValueName = @"Posts",
                            Value = topmostAuthor.Value.Item2.ToString(),
                        });
                    }
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AuthorCount.Clear();              
            }

        }
    }
}
