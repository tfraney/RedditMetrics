using Microsoft.Extensions.Logging;
using RedditMetrics.DataLayer.Models;
using System.Net.Http.Headers;
using RedditMetrics.DataLayer.Interfaces;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

using COMMON = RedditMetrics.DataLayer.FunctionConstants.CommonMessages;
using QUERY = RedditMetrics.DataLayer.FunctionConstants.Query;
using HEADER = RedditMetrics.DataLayer.FunctionConstants.HeaderField;
using System.Net.Http;




namespace RedditMetrics.Business
{

    public class RedditApiConsumer(ILogger logger, string mainAPI, string nextApi, string newTopAPI, string authAPI)
    {
        private  readonly ILogger _logger = logger;     
        private readonly string _mainAPI = mainAPI;
        private readonly string _nextApi = nextApi;
        private readonly string _authAPI = authAPI;
        private readonly string _newTopAPI = newTopAPI;   

        private static string? HeaderInfo(HttpHeaders? headers, string key) => headers?.FirstOrDefault(x => x.Key == key).Value.FirstOrDefault();




        public async Task<(SubredditResult?, IHeaderData)> ExecuteRequestAsync(string subreddit, string action, int count,  
                                                                                        string? token = null, string? before = null, string? after = null)
        {
            SubredditResult? record = default;
            IHeaderData header = new HeaderData()  { Action = action, Message = @"Failed", SubRedditName = subreddit, TokenData = new TokenHeader()   };


            HttpResponseMessage response;
            int tries = 0;

            _logger.LogInformation(@"Client HTTP to Functions: {sub} {action} Read.", subreddit, action);
            try
            {
                var api = action == QUERY.NEWACTION && !string.IsNullOrEmpty(before) ? new Uri(string.Format(_newTopAPI, subreddit, action, count, before)) :
                action == QUERY.NEWACTION && !string.IsNullOrEmpty(after) ? new Uri(string.Format(_nextApi, subreddit, action, count, after)) :
                                                                           new Uri(string.Format(_mainAPI, subreddit, action, count));

                using var client = new HttpClient();
                string value = @"Metrics - Thomas Franey";

                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = api
                };

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(string.IsNullOrWhiteSpace(token)? COMMON.BASIC : COMMON.BEARER, token);

                httpRequestMessage.Method = HttpMethod.Get;              
                httpRequestMessage.Headers.TryAddWithoutValidation(@"User-Agent", value);
                httpRequestMessage.Headers.TryAddWithoutValidation(@"Accept", "*/*");
                httpRequestMessage.Headers.TryAddWithoutValidation(@"Connection", "keep-alive");              
                               
             
                do
                {
                    response = await client.SendAsync(httpRequestMessage);                  
                    int num = tries + 1;
                    tries = num;
                }
                while ((response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.BadGateway || response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.GatewayTimeout) && tries < 20);

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    var wait = response.Headers.FirstOrDefault(x => x.Key == HEADER.XRATELIMITRESET).Value?.FirstOrDefault() ?? 400.ToString();
                    return (new SubredditResult()
                    {
                        Kind = wait,
                        Status = -1,
                        Log = @"Need to delay",
                        Message = response.RequestMessage?.ToString() ?? @"TooManyRequests",
                        SubRedditName = subreddit
                    }, header);
                }

                string text = response.IsSuccessStatusCode ?  await response.Content.ReadAsStringAsync(): string.Empty;
                
                if (!string.IsNullOrEmpty(text)) {
                    var result =  (JsonConvert.DeserializeObject<SubredditResult>(text), response.Headers);
                    header.Remaining = (int)Convert.ToDecimal(HeaderInfo(result.Headers, HEADER.XRATELIMITREMAINING) ?? 100.ToString());
                    header.Reset = (int)Convert.ToDecimal(HeaderInfo(result.Headers, HEADER.XRATELIMITRESET) ?? 400.ToString());

                    if (result.Item1?.Data != null)
                        record = new SubredditResult()
                        {
                            Kind = @"Listing",
                            Log = string.Empty,
                            SubRedditName = subreddit,
                            Status = 0,
                            Message = @"OK",
                            Data = result.Item1.Data
                        };

                    else if (result.Item1 != null)
                    {
                        header.Message = result.Item1.Message ?? result.Item1.Log ?? @"Unknown Error";
                        header.Status = result.Item1?.Status ?? -2;
                        header.Before = result.Item1?.Kind ?? string.Empty;
                        return (result.Item1, header);
                    }
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
                    }, header);
                }
            }
            catch (Exception x)
            {
                return (new SubredditResult() { Kind = @"Error", Status = -2, Log = x.Message, Message = @"(500) Http Error on Reddit.", SubRedditName = string.Empty }, header);
            }
            return (record, header);
        }   
        

        public async Task<RedditToken?> ExecuteOAuthRequestAsync(string basicauth, string clientID, string logindata)
        {

            int tries = 0;
            HttpResponseMessage response;

            _logger.LogInformation(@"Client HTTP to Functions: Auth Read.");

            try
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    RequestUri = new Uri(_authAPI)
                };             

                httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(COMMON.BASIC, basicauth);

                httpRequestMessage.Method = HttpMethod.Post;
                string value = @"Metrics - Thomas Franey";
                httpRequestMessage.Headers.TryAddWithoutValidation(@"User-Agent", value);
                httpRequestMessage.Headers.TryAddWithoutValidation(@"Accept", "*/*");
                httpRequestMessage.Headers.TryAddWithoutValidation(@"Connection", "keep-alive");
                if (!string.IsNullOrEmpty(logindata) && !string.IsNullOrEmpty(basicauth))
                {
                    var login = Encoding.UTF8.GetString(Convert.FromBase64String(logindata))?.Split(';');
                    WritePostBody(httpRequestMessage, new
                    {
                        grant_type = @"password",
                        username = login?[0] ?? string.Empty,
                        password = login?[1] ?? string.Empty
                    });
                }

                do
                {
                    using var client = new HttpClient();
                    response = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead);
                    int num = tries + 1;
                    tries = num;
                }
                while ((response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.BadGateway || response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.GatewayTimeout) && tries < 20);

              
                string text = response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false) : string.Empty;

                if (!string.IsNullOrEmpty(text))
                {
                    return JsonConvert.DeserializeObject<RedditToken>(text);
                }
                _logger.LogError(@"Error Authetication for {clientID}",clientID);
                              
            }
            catch (Exception x)
            {
                _logger.LogError(@"Error Authetication for {clientID}: {error}", clientID, x.Message);               
            }
            return null;
        }
            

         private void WritePostBody(HttpRequestMessage request, object data, params string[] additionalFields)
        {
            PropertyInfo[] properties = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            List<KeyValuePair<string, string>> list = [];
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                string key = (propertyInfo.GetCustomAttributes(typeof(RedditAPINameAttribute), inherit: false).FirstOrDefault() 
                                  is not RedditAPINameAttribute redditAPINameAttribute) ? propertyInfo.Name : redditAPINameAttribute.Name;
                string value = Convert.ToString(propertyInfo.GetValue(data, null)) ?? string.Empty;
                list.Add(new KeyValuePair<string, string>(key, value));
            }

            for (int j = 0; j < additionalFields.Length; j += 2)
            {
                string value2 = Convert.ToString(additionalFields[j + 1]);
                list.Add(new KeyValuePair<string, string>(additionalFields[j], value2));
            }

            request.Content = new StringContent(string.Join(@"&", list.Select((KeyValuePair<string, string> c) => $"{c.Key}={WebUtility.UrlEncode(c.Value)}")), Encoding.UTF8, @"application/x-www-form-urlencoded");
        }
    }


 }
