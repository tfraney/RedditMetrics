
using RedditMetrics.DataLayer.Models;
using RedditMetrics.DataLayer.Interfaces;
using CONST = RedditMetrics.DataLayer.FunctionConstants;
using Microsoft.Extensions.Logging;

namespace RedditMetrics.Business.RedditReaders
{
    public class RedditReader : BaseRedditReader
    {
        public override async Task<(ISubredditResult?,IHeaderData?)> ReadQuery(ILogger logger, string subreddit,  string log, 
                                                                               string action, string token, int cnt)
        {
            var msg = $"Success. Reddit Api = {subreddit}-{action}";
            var api = new RedditApiConsumer(logger);
            IHeaderData? headerData;
            SubredditResult? result;
            var status = 0;                  
           
            PostResult? postResult = null;
            try
            {             

                (result, headerData)  = await api.GetAsync(CONST.DEFAPI, token, subreddit,action,cnt);
                postResult = result?.Data;

                var rem = headerData?.Remaining ?? 400;
                var reset = headerData?.Reset??  200;
                token = headerData?.Token ?? token;

                if (postResult != null)
                {
                    headerData = new HeaderData()
                    {
                        Action = action,
                        After = postResult.After,
                        Before = postResult.Before,
                        Message = CONST.Messages.SUCCESSFUL,
                        SubRedditName = subreddit,
                        Remaining = Convert.ToInt32(rem),
                        Reset = Convert.ToInt32(reset),
                        Status = 0,
                        Token = token
                    };
                }
                else return (null, headerData);
            }
            catch (Exception ex)
            {
                status = -2;
                log = $"{log}.\nError ={ex.Message}";
                msg = $"Failed. Reddit Api = {subreddit}-{action}";
                headerData = new HeaderData() { Action = action, Message = msg,SubRedditName = subreddit, Status = -2, Token = token }; 
            }
           
            result = new SubredditResult
            {
                SubRedditName = subreddit,
                Kind = CONST.LISTING,
                Log = log,
                Message = msg,
                Status = status,
                Data = postResult
            };
            return (result,headerData);
        }        
       }
}
