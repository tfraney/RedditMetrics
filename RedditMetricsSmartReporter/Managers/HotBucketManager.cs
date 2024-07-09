using RedditMetrics.Business;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;

namespace RedditMetricsSmartReporter.Managers
{
    public class HotBucketManager(IMetricData metrics) : BucketManager
    {       
        public override string Topic { get => RedditMetrics.DataLayer.FunctionConstants.POSTS_HOT; }

        public override async Task PopulateBucket(ILogger logger, ISubredditResult result)
        {
            var subreddit = result.SubRedditName;
            logger.LogInformation(@"Stacking Hot Bucket-{subreddit}", subreddit);

            if (await BuildStack(result) && (Stack.Items[subreddit]?.Length ?? 0) != 0) {

                logger.LogInformation(@"Successful: Adding Hot Metrics {subreddit}", subreddit);
                var topmost = Stack.Items[subreddit].First();

                metrics.AddNewSubReddit(subreddit, new MetricItem() {
                     Name = @"Hottest Post",                      
                    Description = $"{topmost.Name} - {topmost.Title} - {topmost.Author}",
                     ValueName= @"# of comments",
                     Value = topmost.Comments.ToString(),                    
                });
            }           
        }     
    }
}
