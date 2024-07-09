using Microsoft.Extensions.Logging;

namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IConsumerWrapper : IDisposable
    {
        Task ExecuteAsync(CancellationToken token);
    }
}
