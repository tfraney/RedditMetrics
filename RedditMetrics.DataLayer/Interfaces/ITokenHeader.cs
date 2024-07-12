
namespace RedditMetrics.DataLayer.Interfaces
{
    public interface ITokenHeader
    {    
        string BasicAuth { get; set; }
        int MaxExpireTime { get; set; }       
        string Token { get; set; }      
        DateTime TokenStamp { get; set; }       
        string? RefreshToken { get; set; }
        int TimeLeft { get; }
        string? ReturnedToken { get ; }
    }
}
