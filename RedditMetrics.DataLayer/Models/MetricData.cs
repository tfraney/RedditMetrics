using RedditMetrics.DataLayer.Interfaces;
using System.Linq;

namespace RedditMetrics.DataLayer.Models
{
    public class MetricData : IMetricData
    {
        private readonly object thisLock = new();
        private bool disposedValue;

        public Dictionary<string, IMetricSubRedditData> MetricList { get; } = [];      

        public void AddNewSubReddit(string name, IMetricItem item)
        {
            lock (thisLock)
            {
                if (!MetricList.ContainsKey(name)) MetricList.Add(name, new MetricSubRedditData());
                MetricList[name].AddItem(item);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var data in MetricList.Values) data.Dispose();
                   MetricList.Clear();
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

    public class MetricSubRedditData : IMetricSubRedditData
    {
        private readonly object _thisLock = new();
        private bool disposedValue;
        public Dictionary<string, IMetricItem> Items { get; } = [];

        public void AddItem(IMetricItem value)
        {
            lock (_thisLock)
            {
                if (!Items.TryAdd(value.Name, value)) Items[value.Name] = value;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)  Items.Clear();
            }
            disposedValue = true;
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }       
    }

    public class MetricItem : IMetricItem
    {
        public required string Name { get; set; }
        public required string Description { get; set;}
        public required string ValueName { get; set;}
        public required string Value { get; set;  }

    }
}
