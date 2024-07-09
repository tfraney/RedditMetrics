using Newtonsoft.Json;
using RedditMetrics.DataLayer.Models;
using RedditMetrics.DataLayer.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;

using QUERYCONST = RedditMetrics.DataLayer.FunctionConstants.Query;
using ERRORS = RedditMetrics.DataLayer.FunctionConstants.ErrorMessages;
using MESSAGES = RedditMetrics.DataLayer.FunctionConstants.Messages;
using CONST = RedditMetrics.DataLayer.FunctionConstants;


namespace RedditMetrics.Business
{
    public class ReaderFunction
    {
        public static async Task<HeaderData?> ExecuteSubRedditReader(HttpRequestData req, ILogger logger, IProducerWrapper producer,
                                                                     IRedditReader reader,  string action)  
        {
            ISubredditResult? queuedMessage = null;
            IHeaderData? result;

            string logMessage = action;
            string topic = GetProducerTopic(action);
            string? name = req?.Query[QUERYCONST.SUBREDDITNAME];
            string? count = req?.Query[QUERYCONST.COUNT];
            string? before = req?.Query[QUERYCONST.PAGEBEFORE];
            string? after = req?.Query[QUERYCONST.PAGEAFTER];
            string? token = req?.Query[QUERYCONST.TOKEN];
            try
            {

                dynamic? data = null;
                if (req != null)
                {
                    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                    data = JsonConvert.DeserializeObject(requestBody);
                }
                name ??= data?.name ?? @"all";
                count ??= data?.count ?? @"100";
                before ??= data?.before;
                after ??= data?.after;
                token ??= data?.token ?? string.Empty;


                logMessage = PrepareLog(action, count, name, before, after);
                if (string.IsNullOrEmpty(logMessage))
                    result = new HeaderData() { Status = -1, Action = action, SubRedditName = name, Message = string.Format(ERRORS.NOACTION, name) };
                else
                {
                    logger.LogInformation(@"{Message}", logMessage);
                    
                    if (reader == null)
                        result = new HeaderData() { SubRedditName = name, Action = action, Status = -1, Message = ERRORS.NOSERVICE };
                    else
                    {
                        (queuedMessage, result) = await reader.ReadQuery(logger, name, logMessage,action, token, Convert.ToInt32(count));
                    }
                }
            }           
            catch (Exception ex)
            {
                logMessage = $"{logMessage} \n Error: {ex.Message}";
                logger.LogError(@"{Message}", logMessage);
                result = new HeaderData() { Action = action, SubRedditName = name ?? string.Empty, Status = -2, Message = ex.Message };
            }

            if (!string.IsNullOrEmpty(topic) && queuedMessage != null) {
                try
                {                   
                    await producer.WriteMessage(topic, JsonConvert.SerializeObject((SubredditResult) queuedMessage));
                }
                catch
                {
                    result = new HeaderData() { Status = -3, Message = ERRORS.KAFKAFAILURE, Action = action, SubRedditName = name ?? string.Empty };
                }
            }
            return (HeaderData?) result;
        }

        protected static string GetProducerTopic(string action) => 
            action switch
            {
                QUERYCONST.HOTACTION => CONST.POSTS_HOT,
                QUERYCONST.TOPACTION => CONST.POSTS_TOP,
                QUERYCONST.NEWACTION => CONST.POSTS_NEW,
                _ => string.Empty
            };        

        private static string PrepareLog(string action, string count, string name, string? before, string? after) =>
            action switch
            {
                QUERYCONST.HOTACTION => string.Format(MESSAGES.HOTLOG, name, count),
                QUERYCONST.TOPACTION => string.Format(MESSAGES.TOPVOTELOG, name, count),
                QUERYCONST.NEWACTION => !string.IsNullOrEmpty(after) ?
                                            string.Format(MESSAGES.NEXTPAGELOG, name, after) :
                                        !string.IsNullOrEmpty(before) ?
                                            string.Format(MESSAGES.LATESTLOG, name, before) :
                                            string.Format(MESSAGES.NEWLOG, name),
                _ => string.Empty,
            };        
    }
}
