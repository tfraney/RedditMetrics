
using RedditMetrics.DataLayer.Interfaces;
using System.Text.Json.Serialization;

using CONST = RedditMetrics.DataLayer.FunctionConstants.ReditTokenFields;

namespace RedditMetrics.DataLayer.Models
{
    public class RedditToken : IRedditToken
    {
        [JsonPropertyName(CONST.ACCESSTOKEN)]
        public string? Access_Token { get; set; }

        [JsonPropertyName(CONST.EXPIRESIN)]
        public int Expires_In { get; set; } = 86400;

        [JsonPropertyName(CONST.REFRESHTOKEN)]
        public string? Refresh_Token { get; set; }
    }
}
