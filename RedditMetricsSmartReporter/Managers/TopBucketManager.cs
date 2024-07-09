
using RedditMetrics.Business;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;

namespace RedditMetricsSmartReporter.Managers
{
    public class TopBucketManager(IMetricData metrics) : BucketManager
    {
        public override string Topic { get => RedditMetrics.DataLayer.FunctionConstants.POSTS_TOP; }

        public override async Task PopulateBucket(ILogger logger, ISubredditResult result)
        {
            var subreddit = result.SubRedditName;
            logger.LogInformation(@"Stacking Top Voted Bucket-{subreddit}", subreddit);

            if (await BuildStack(result) && (Stack.Items[subreddit]?.Length ?? 0) != 0)
            {

                logger.LogInformation(@"Successful: Adding Top Metrics {subreddit}", subreddit);
                var topmost = Stack.Items[subreddit].First();

                metrics.AddNewSubReddit(subreddit, new MetricItem()
                {
                    Name = @"Post with Highest Votes",
                    Description = $"{topmost.Name} - {topmost.Title} - {topmost.Author}",
                    ValueName = @"# of Votes",
                    Value = topmost.Votes.ToString(),
                });
            }
        }
    }
}
