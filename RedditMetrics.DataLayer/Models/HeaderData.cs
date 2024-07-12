using System.Text.Json.Serialization;
using CONST = RedditMetrics.DataLayer.FunctionConstants.HeaderField;
using COMMON = RedditMetrics.DataLayer.FunctionConstants.CommonField;
using RESCONST = RedditMetrics.DataLayer.FunctionConstants.PostResultFields;
using RedditMetrics.DataLayer.Interfaces;

namespace RedditMetrics.DataLayer.Models
{
    public class HeaderData : IHeaderData
    {
        [JsonPropertyName(CONST.ACTIONS)]
        public required string Action { get; set; }

        [JsonPropertyName(CONST.SUBREDDITNAME)]
        public required string SubRedditName { get; set; }

        [JsonPropertyName(RESCONST.AFTER)]
        public string? After { get; set; }

        [JsonPropertyName(RESCONST.DIST)]
        public int Dist { get; set; } = 0;

        [JsonPropertyName(COMMON.TOKENDATA)]
        public ITokenHeader? TokenData { get; set;  } = new TokenHeader();    

        [JsonPropertyName(RESCONST.BEFORE)]
        public string? Before { get; set; }

        [JsonPropertyName(COMMON.STATUS)]
        public int Status { get; set; } = -2;

        [JsonPropertyName(COMMON.MESSAGE)]
        public required string Message { get; set; }

        [JsonPropertyName(COMMON.REMAINING)]
        public int Remaining { get; set; } = 100;

        [JsonPropertyName(COMMON.RESET)]
        public int Reset { get; set; } = 100;

        
        [JsonIgnore]
        public int MilisecondsDelay { get => (int) Math.Round((double)((Reset*1000) + 1) / (Remaining*1000)) + 50; }
                
        [JsonIgnore]
        public string? TokenReturned { get => TokenData?.ReturnedToken; }
    }
}
