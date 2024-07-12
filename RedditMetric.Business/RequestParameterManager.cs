using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using QUERYCONST = RedditMetrics.DataLayer.FunctionConstants.Query;

namespace RedditMetrics.Business
{
    public class RequestParameterManager : IDisposable
    {
        private bool disposedValue;

        public string Name { get; set; } = string.Empty;
        public string Count { get; } = 100.ToString();
        public string? Before { get; }
        public string? After { get; }   
        public string Authdata { get; } = string.Empty;
        public string Login { get; } = string.Empty;

        public RequestParameterManager(HttpRequestData req)
        {
            dynamic? data = null;

            if (req != null)
            {
                try
                {
                    string requestBody = new StreamReader(req.Body).ReadToEnd();
                    data = requestBody != null ? JsonConvert.DeserializeObject(requestBody) : default;
                }
                catch { }

                Name = req?.Query[QUERYCONST.SUBREDDITNAME] ?? data?.name ?? string.Empty;
                Count = req?.Query[QUERYCONST.COUNT] ?? data?.count ?? Count;
                Before = req?.Query[QUERYCONST.PAGEBEFORE] ?? data?.before;
                After = req?.Query[QUERYCONST.PAGEAFTER] ?? data?.after;
                Authdata = req?.Query[QUERYCONST.AUTHDATA] ?? data?.authdata ?? string.Empty;
                Login = req?.Query[QUERYCONST.LOGIN] ?? data?.code ?? string.Empty;               
            }           
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {               
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
