
using RedditMetrics.DataLayer.Models;
using RedditMetrics.DataLayer.Interfaces;
using Microsoft.Extensions.Logging;

using CONST = RedditMetrics.DataLayer.FunctionConstants;
using Microsoft.Extensions.Configuration;

namespace RedditMetrics.Business.RedditReaders
{
    public class RedditReader(string mainAPI, string nextApi, string newTopAPI, string authAPI) : BaseRedditReader()
    {
        public override async Task<(ISubredditResult?,IHeaderData?)> ReadQuery(ILogger logger, string subreddit,  string log, 
                                                                               string action, string authdata, int cnt, string? before, string? after)
        {
            var msg = $"Success. Reddit Api = {subreddit}-{action}";
            var api = new RedditApiConsumer(logger, mainAPI, nextApi, newTopAPI, authAPI);

            IHeaderData? headerData = null;
            SubredditResult? result;
            var status = 0;                  
           
            PostResult? postResult = null;
            try
            {             

                (result, headerData)  = await api.ExecuteRequestAsync(subreddit,action,cnt, authdata ,before, after);
                postResult = result?.Data;                          

                if (postResult != null)
                {
                    headerData = new HeaderData()
                    {
                        Action = action,
                        After = postResult.After,
                        Before = postResult.Before,
                        Message = CONST.Messages.SUCCESSFUL,
                        SubRedditName = subreddit,
                        Remaining = headerData.Remaining,
                        Reset = headerData.Reset,
                        Status = 0,
                        TokenData = headerData?.TokenData ?? new TokenHeader()  
                    };
                }
                else return (null, headerData);
            }
            catch (Exception ex)
            {
                status = -2;
                log = $"{log}.\nError ={ex.Message}";
                msg = $"Failed. Reddit Api = {subreddit}-{action}";
                headerData = new HeaderData() { Action = action, Message = msg,SubRedditName = subreddit, Status = -2, 
                                                TokenData = headerData?.TokenData ?? new TokenHeader() }; 
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

        public override async Task<IHeaderData> TryAuth(ILogger logger, string client_name, string loginData, string basicauth)
        {
            var msg = CONST.Messages.AUTH_STARTED;

            var api = new RedditApiConsumer(logger, mainAPI, nextApi, newTopAPI, authAPI);
            IHeaderData? headerData;                       
            
            try
            {
                IRedditToken? tokens = await api.ExecuteOAuthRequestAsync(basicauth, client_name, loginData);

                if (tokens != null)
                headerData = new HeaderData()
                {
                    Action = CONST.CommonMessages.AUTH_ACTION,
                    Message = msg,
                    SubRedditName = client_name,
                    Remaining = 400,
                    Reset = 100,
                    Status = 0,
                    TokenData = new TokenHeader() { BasicAuth = basicauth, MaxExpireTime = tokens.Expires_In, RefreshToken = tokens.Refresh_Token,
                                                     Token = tokens.Access_Token ?? string.Empty, TokenStamp = DateTime.Now }
                };
                else headerData = new HeaderData()
                {
                    Action = CONST.CommonMessages.AUTH_ACTION,
                    Message = msg,
                    SubRedditName = client_name,
                    Remaining = 400,
                    Reset = 100,
                    Status = 0,
                    TokenData = new TokenHeader()
                    {
                        BasicAuth = basicauth,
                        MaxExpireTime = -1,
                        RefreshToken = string.Empty,
                        Token = string.Empty,
                        TokenStamp = DateTime.Now
                    }
                };


            }
            catch
            {                          
                msg = CONST.ErrorMessages.AUTH_FAILED;
                headerData = new HeaderData()
                {
                    Action = CONST.CommonMessages.AUTH_ACTION,
                    Message = msg,
                    SubRedditName = client_name,
                    Status = -2,
                    TokenData = new TokenHeader()
                };
            }
            return headerData;
        }


    }
}
