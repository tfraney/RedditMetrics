using RedditMetrics.DataLayer.Interfaces;
using System.Text.Json.Serialization;
using COMMON = RedditMetrics.DataLayer.FunctionConstants.CommonField;
using CONST = RedditMetrics.DataLayer.FunctionConstants.CommonMessages;
namespace RedditMetrics.DataLayer.Models
{
    public class TokenHeader : ITokenHeader
    {
        [JsonPropertyName(COMMON.BASICAUTH)]
        public string BasicAuth { get; set; } = string.Empty;

        [JsonPropertyName(COMMON.MAXEXPIRETIME)]
        public int MaxExpireTime { get; set; } = 86400;

        [JsonPropertyName(COMMON.TOKEN)]
        public string Token { get; set; } = CONST.BASIC;

        [JsonPropertyName(COMMON.TOKEN_STAMP)]
        public DateTime TokenStamp { get; set; } = DateTime.MinValue;

        [JsonPropertyName(COMMON.REFRESH_TOKEN)]
        public string? RefreshToken { get; set; } = string.Empty;

        [JsonIgnore]    
        public int TimeLeft { get => MaxExpireTime - (Token.Equals(CONST.BASIC) ? 0 : DateTime.Now.Subtract(TokenStamp).Seconds); }

        [JsonIgnore]      
        public string? ReturnedToken { get => !string.IsNullOrWhiteSpace(Token)? 
                                             Token : Token == null? null : string.Empty ; }
    }
}
