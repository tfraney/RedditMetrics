
namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IApiClientData : IDisposable
    {
        string? GetToken(string clientout,out string[]? set);
       void Init(string[][]? fromCofigData);
       void SetToken(string client, string token);
    }
}
