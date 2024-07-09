
namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IProducerWrapper : IDisposable
    {
        Task WriteMessage(string topic, string message);       
    }
}
