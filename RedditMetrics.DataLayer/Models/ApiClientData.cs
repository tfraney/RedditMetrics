using RedditMetrics.DataLayer.Interfaces;


namespace RedditMetrics.DataLayer.Models
{
    public class ApiClientData : IApiClientData
    {
        private Dictionary<string, string[]>? _clientInfo;
        private readonly object thisLock = new();
        private bool disposedValue;

        public void Init(string[][]? fromCofigData)
        {
            lock (thisLock)
            {
                _clientInfo = (fromCofigData?.Select((x, y) => (x[0], x)).ToDictionary());
            }

        }

        public string? GetToken(string client, out string[]? set)
        {
            set = null;
            if (!string.IsNullOrWhiteSpace(client)) lock (thisLock)
                {                  
                    return _clientInfo != null && _clientInfo.TryGetValue(client, out string[]? value) &&
                                                    (set = value).Length >= 4 && !string.IsNullOrEmpty(set[3]) ?
                           set[3] : set != null ? string.Empty : null;
                }
            return null;

        }
        public void SetToken(string client, string token)
        {

            if (!string.IsNullOrWhiteSpace(token)) lock (thisLock)
                {
                    if (_clientInfo != null && _clientInfo.TryGetValue(client, out string[]? value) && value.Length >= 4)
                    {
                        value[3] = token;                        
                    }
                }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _clientInfo?.Clear();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
