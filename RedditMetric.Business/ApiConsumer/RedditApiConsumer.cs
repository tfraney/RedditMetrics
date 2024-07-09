using Microsoft.Extensions.Logging;
using RedditMetrics.DataLayer.Models;
using System.Net.Http.Headers;
using RedditSharp;
using RedditMetrics.DataLayer.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;
using CONST = RedditMetrics.DataLayer.FunctionConstants;

namespace RedditMetrics.Business
{
    
    public class RedditApiConsumer(ILogger logger) 
    {
        private readonly ILogger _logger = logger;
        private static readonly HttpClient _httpClient = new();
        private string RootDomain = @"www.reddit.com";      
      
        public async Task<(SubredditResult?, IHeaderData?)> 
            GetAsync(string root, string token, string subreddit, string action, int count)
        {
            SubredditResult? record = default;
            IHeaderData? header = new HeaderData() { Action = action, Message = @"OK", SubRedditName = subreddit, Token = token };

            RootDomain = root;

            _logger.LogInformation(@"Client HTTP to Functions: {sub} {action} Read.", subreddit, action);
         
            var result = await ExecuteRequestAsync(subreddit,() => CreateRequest(new Uri($"https://{RootDomain}/r/{subreddit}/{action}?count={count}&limit={count}"),
                                                                                 @"GET", token));

            header.Remaining = (int) Convert.ToDecimal(result.Item2?.FirstOrDefault(x => x.Key == CONST.HeaderField.Remaining).Value.FirstOrDefault() ?? "100");
            header.Reset = (int) Convert.ToDecimal(result.Item2?.FirstOrDefault(x => x.Key == CONST.HeaderField.Reset).Value.FirstOrDefault() ?? "400");
            if (result.Item1?.Data != null)
            {               
                record = new SubredditResult() { Kind = @"Listing", Log = string.Empty, SubRedditName = subreddit,
                    Status = 0, Message = @"OK", Data = result.Item1.Data
                };
             }
            else if (result.Item1 != null) {
                header.Message = result.Item1.Message ?? result.Item1.Log ?? @"Unknown Error";
                header.Status = result.Item1?.Status ?? -2;
                header.Before = result.Item1?.Kind ?? string.Empty;
                return (result.Item1, header);
            }
            return (record, header);
        }

        private static async Task<(SubredditResult?, HttpHeaders?)> ExecuteRequestAsync(string subreddit, Func<HttpRequestMessage> request)
        {

            int tries = 0;
            HttpResponseMessage response;
            try
            {
                ArgumentNullException.ThrowIfNull(request);
                do
                {

                    response = await _httpClient.SendAsync(request()).ConfigureAwait(continueOnCapturedContext: false);
                    int num = tries + 1;
                    tries = num;
                }
                while ((response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.BadGateway || response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.GatewayTimeout) && tries < 20);
                
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    var wait = response.Headers.FirstOrDefault(x => x.Key == CONST.HeaderField.Reset).Value?.FirstOrDefault() ?? "400";
                    return (new SubredditResult() { Kind = wait, Status = -1, Log = @"Need to delay", 
                                                    Message = response.RequestMessage?.ToString() ?? @"TooManyRequests",
                                                    SubRedditName = subreddit }, null);
                }               

                string text = response.IsSuccessStatusCode ?
                              await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false) : string.Empty;
                JToken jToken;
                if (!string.IsNullOrEmpty(text))
                {
                    jToken = JToken.Parse(text);

                    if (jToken[@"errors"] != null)
                    {
                        throw (jToken[@"errors"]?.ToString() ?? @"400") switch
                        {
                            @"404" => new Exception(@"File Not Found"),
                            @"403" => new Exception(@"Restricted"),
                            _ => new Exception($"Error return by reddit: {jToken["errors"]?[0]?[0]}"),
                        };
                    }
                    else if (text != null) return (JsonConvert.DeserializeObject<SubredditResult>(text), response.Headers);                    
                }
                else
                {
                    return (new SubredditResult()
                    {
                        Kind = @"General Http Error",
                        Status = -2,
                        Log = response.RequestMessage?.ToString() ?? response.ReasonPhrase,
                        Message = $"(400) {response.StatusCode}",
                        SubRedditName = subreddit
                    }, null);
                }
            }
            catch (Exception x)
            {
                return (new SubredditResult() { Kind = @"Error", Status = -2, Log = x.Message, Message = @"(500) Http Error on Reddit.", SubRedditName = string.Empty }, null);
            }
            return (null, null);
        }
        private static  HttpRequestMessage CreateRequest(Uri uri, string method, string token)
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                RequestUri = uri
            };

            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(@"bearer", token);

            httpRequestMessage.Method = new HttpMethod(method);
            string value = @"Metrics - Thomas Franey";
            httpRequestMessage.Headers.TryAddWithoutValidation(@"User-Agent", value);
            return httpRequestMessage;
        }


    }
}
