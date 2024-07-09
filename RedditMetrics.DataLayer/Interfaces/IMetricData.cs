
namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IMetricData : IDisposable
    {
        Dictionary<string, IMetricSubRedditData> MetricList { get; }
        public void AddNewSubReddit(string name, IMetricItem item);     
       
    }

    public interface IMetricSubRedditData : IDisposable
    {
        Dictionary<string, IMetricItem> Items { get; }

        public void AddItem(IMetricItem value);
    }

    public interface IMetricItem
    {
        string Name { get; set; }
        string Description { get; set; }
        string ValueName { get; set; }
        string Value { get; set; }

    }
}
