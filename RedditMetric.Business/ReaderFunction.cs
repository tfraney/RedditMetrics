using Newtonsoft.Json;
using RedditMetrics.DataLayer.Models;
using RedditMetrics.DataLayer.Interfaces;
using Microsoft.Extensions.Logging;
using ERRORS = RedditMetrics.DataLayer.FunctionConstants.ErrorMessages;
using MESSAGES = RedditMetrics.DataLayer.FunctionConstants.Messages;
using COMMON = RedditMetrics.DataLayer.FunctionConstants.CommonMessages;

namespace RedditMetrics.Business
{
    public class ReaderFunction
    {       
        public static async Task<HeaderData?> ExecuteSubRedditReader(RequestParameterManager req, ILogger logger, IProducerWrapper producer,
                                                                     IRedditReader reader,  string action, string topic, string logString)  
        {
            ISubredditResult? queuedMessage = null;
            string sub = req.Name ?? string.Empty;

            _ = int.TryParse(req.Count, out int cnt);

            IHeaderData? result = new HeaderData() { SubRedditName = sub, Action = action, Status = -2, Message = ERRORS.NOSERVICE };

            logger.LogInformation(COMMON.LOGMESSAGE, string.Format(logString, sub, req.Count, AppendPagingToLog(req.Before, req.After)));
            try
            {
                (queuedMessage, result) = reader == null? (null,result) :
                                                           await reader.ReadQuery(logger, sub, logString, action, req.Authdata, cnt,req.Before,req.Before != null? null : req.After);                
            }
            catch (Exception ex)
            {              
                logger.LogError(COMMON.LOGMESSAGE, $"{logString} \n {COMMON.ERROR}: {ex.Message}");
                result = new HeaderData() { Action = action, SubRedditName = sub, Status = -2, Message = ex.Message };
            }

            if (!string.IsNullOrEmpty(topic) && queuedMessage != null)
            {
                try { await producer.WriteMessage(topic, JsonConvert.SerializeObject((SubredditResult)queuedMessage)); }
                catch { result = new HeaderData() { Status = -3, Message = ERRORS.KAFKAFAILURE, Action = action, SubRedditName = sub }; }
            }

            return (HeaderData?) result;
        }

        public static async Task<IHeaderData?> PostAuthToReader(RequestParameterManager req, ILogger logger, IRedditReader reader, string logString)
        {          
            IHeaderData? result = new HeaderData() { SubRedditName = MESSAGES.AUTHLOG, Action = MESSAGES.AUTHLOG, Status = -2, Message = ERRORS.NOSERVICE };
            string sub = req.Name ?? string.Empty;

            if (reader == null) return result;
            try
            {

                logger.LogInformation(COMMON.LOGMESSAGE, logString);
                if (!string.IsNullOrWhiteSpace(req?.Login) && !string.IsNullOrWhiteSpace(req?.Authdata))
                {
                    result = reader == null ? result : await reader.TryAuth(logger, sub, req.Login, req.Authdata);
                }

            }
            catch (Exception ex)
            {
                logString = $"{logString} \n {COMMON.ERROR}: {ex.Message}";
                logger.LogError(COMMON.LOGMESSAGE, logString);
                result = new HeaderData() { Action = sub, SubRedditName = sub, Status = -2, Message = ex.Message };
            }
            return result;
        }


        private static string AppendPagingToLog( string? before, string? after) {
            return (!string.IsNullOrEmpty(after)) ? string.Format(MESSAGES.NEXTPAGELOG, after) :
                   (!string.IsNullOrEmpty(before)) ? string.Format(MESSAGES.LATESTLOG,  before) :
                    string.Empty;               
       }      
    }
}
