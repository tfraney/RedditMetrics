using RedditMetrics.Business;
using RedditMetrics.DataLayer.Models;


namespace RedditMetricsOrchestrator.Workers
{
    public class MeasureRedditDataWorker(ILogger logger) : IMeasureRedditDataWorker
    {
        private readonly ILogger _logger = logger;

        public async Task Execute(string api, string topic,string cnt, string subreddit, string clienttoken, CancellationToken cancelToken)
        {
            int failedTries = 0;
            _logger.LogInformation(@"Orchestrating metrics for subreddit {subreddit} - {topic}", subreddit,topic);
            int lastDelay = 1;
            while (!cancelToken.IsCancellationRequested && failedTries < 20)
            {
                string? failed = null;
                using var x = new ApiConsumer<HeaderData>(_logger, api);
                var tokenize = !string.IsNullOrWhiteSpace(clienttoken) ? $"&token={clienttoken}" : string.Empty;

                var (content, msg) = await x.GetAsync(topic, $"api/{topic}?name={subreddit}&count={cnt}{tokenize}");

                if (content != null)
                {
                    if (content.Status < -1)
                    {
                        _logger.LogError(@"Http Request Issule => Worker for {topic} : {sub} : {msg}",
                                           topic, subreddit, content.Message);
                        failed = msg.StatusCode.ToString();
                    }
                    else if (content.Status == -1)
                    {
                        _logger.LogWarning(@"Delay Warning (Too many calls) taking {} seconds to wait => Worker for {topic} : {sub} ",
                                           content.Before, topic, subreddit);
                        Task.Delay(Convert.ToInt32(content.Before) * 1000, cancelToken).Wait(cancelToken);
                    }
                    else
                    {
                        if (content.SecondsDelay > lastDelay)
                            _logger.LogWarning(@"Delay Warning (Thottling) to {time} => Worker for {topic} : {sub} ",
                                              content.SecondsDelay, topic, subreddit);
                        
                        Task.Delay(content.SecondsDelay * 1000, cancelToken).Wait(cancelToken);
                    }
                    lastDelay = content.SecondsDelay;
                }
                else failed = msg?.StatusCode.ToString() ?? @"Empty Http Request";

                if (failed != null)
                {
                    if (failedTries++ == 20)                     
                    {
                        _logger.LogCritical(@"Persistent Error. Closing worker for this Reddit Http Reuest => Worker for {topic} : {sub} - {failure}",
                                           topic, subreddit, failed);                        
                    }
                    else Task.Delay(2500, cancelToken).Wait(cancelToken);
                }
                else failedTries = 0;
            }
        }
    }

    public interface IMeasureRedditDataWorker
    {
        Task Execute(string api, string topic, string cnt, string subreddit, string clienttoken, CancellationToken cancelToken);
    }
}
