using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedditMetrics.DataLayer.Models;
using System.Net.Http.Headers;

namespace RedditMetrics.Business
{
    
    public class ApiConsumer<T>(ILogger logger, string host) : IDisposable
    {
        private readonly ILogger _logger = logger;
        private readonly string _host = host;

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing) {  }
                disposedValue = true;
            }
        }

        public async Task<(T? content, HttpResponseMessage msg)> GetAsync(string action, string path, string? token = null)
        {
            T? record = default;
            
            using var client = new HttpClient();          
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(@"application/json"));          

            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Add(@"Authorizaion", $"bearer {token}");                
            }

            HttpResponseMessage response = await client.GetAsync(new Uri($"{_host}/{path}"));
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    _logger.LogInformation(@"API Calls to functions to {host} : {action} => Read Successfully.", _host, action);
                    var res = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<ApiValue<T>>(res);                    
                    if (data != null) record = data.Value;
                }
                catch (Exception ex)
                {
                    _logger.LogError(@"API Calls to functions to {host} : {action} => Error building {type} object from content read: {ex}",
                                     _host, action, nameof(T), ex.Message);
                }
            }            
            return (record, response);
        }
         public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
