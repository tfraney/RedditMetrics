
namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IRedditToken
    {      
        string? Access_Token { get; set; }    
        int Expires_In { get; set; }     
        string? Refresh_Token { get; set; }
    }
}
