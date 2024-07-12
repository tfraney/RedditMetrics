using RedditMetrics.Business;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;

using ERRMSG = RedditMetrics.DataLayer.FunctionConstants.ErrorMessages;
using MSG = RedditMetrics.DataLayer.FunctionConstants.Messages;
using COMMON = RedditMetrics.DataLayer.FunctionConstants;

namespace RedditMetricsOrchestrator.Workers
{
    public class MeasureRedditDataWorker(ILogger logger, IApiClientData clientData, string apiHost, string authStr, string actionStr) : IMeasureRedditDataWorker
    {
        private readonly ILogger _logger = logger;
        private readonly IApiClientData _clientData = clientData;

        public async Task Execute(string topic,string cnt, string subreddit, string client, int loop, CancellationToken cancelToken)
        {
            Task.Delay(5000 * loop, cancelToken).Wait(cancelToken);
            int failedTries = 0;
            _logger.LogInformation(MSG.WORKER_STARTBRANCH_LOG, subreddit,topic);
            int lastDelay = 500;
            bool ignoreAuth = false;
            
            while (!cancelToken.IsCancellationRequested && failedTries < 20)
            {
                string? failed = ERRMSG.WORKER_NOREQUESTDATA;
                string[]? authset = [];
                HeaderData? content = null;
                HttpResponseMessage msg = new() {  StatusCode = System.Net.HttpStatusCode.NotFound };


                using var x = new ApiConsumer<HeaderData>(_logger, apiHost);
                var token = _clientData?.GetToken(client, out authset);
                
                if (!ignoreAuth && (token != null) && token == string.Empty && authset != null && authset.Length == 5)
                {                   
                     (content, msg) = await x.GetAsync(COMMON.CommonMessages.AUTH_ACTION, string.Format(authStr, authset[0], authset[1], authset[2]));
                     token = content?.TokenReturned;
                     if (token != null) _clientData?.SetToken(client, token);
                     if ((content?.Status ?? -2) < 0) failedTries = 100; 
                }
                else ignoreAuth = token == null && true;

                //start executing http trigger for data
                if (failedTries <= 20)                 
                      (content, msg) = await x.GetAsync(topic, string.Format(actionStr, subreddit, token, cnt, topic));      

                if (content != null)
                {
                    if (content.Status < -1)
                    {
                        _logger.LogError(ERRMSG.WORKER_REQUESTISSUE, topic, subreddit, content.Message);
                         failed = msg.StatusCode.ToString();
                    }
                    else if (content.Status == -1)
                    {
                        _logger.LogWarning(ERRMSG.WORKER_LIMITEXCEEDED, content.Before, topic, subreddit);
                         Task.Delay(Convert.ToInt32(content.Before) * 1000, cancelToken).Wait(cancelToken);
                    }
                    else
                    {
                        failed = null;
                        if (content.MilisecondsDelay > lastDelay) _logger.LogWarning(MSG.WORKER_DELAYWARNING_LOG,   content.MilisecondsDelay, topic, subreddit);                        
                        Task.Delay(content.MilisecondsDelay, cancelToken).Wait(cancelToken);
                    }
                    lastDelay = content.MilisecondsDelay;
                }
                else failed = msg?.StatusCode.ToString() ?? failed;


                if (failed != null)
                {
                    if (failedTries++ == 20) _logger.LogCritical(ERRMSG.WORKER_CRITICAL_FAILURE,     topic, subreddit, failed);                                            
                    else Task.Delay(2500, cancelToken).Wait(cancelToken);
                }
                else failedTries = 0;
            }
        }
    }


    public interface IMeasureRedditDataWorker
    {
        Task Execute(string topic, string cnt, string subreddit, string client, int loop,  CancellationToken cancelToken);
    }
}
