using System.Text.Json.Serialization;
using CONST = RedditMetrics.DataLayer.FunctionConstants.HeaderField;
using COMMON = RedditMetrics.DataLayer.FunctionConstants.CommonField;
using RESCONST = RedditMetrics.DataLayer.FunctionConstants.PostResultFields;
using RedditMetrics.DataLayer.Interfaces;

namespace RedditMetrics.DataLayer.Models
{
    public class HeaderData : IHeaderData
    {
        [JsonPropertyName(CONST.Action)]
        public required string Action { get; set; }

        [JsonPropertyName(CONST.SubRedditName)]
        public required string SubRedditName { get; set; }

        [JsonPropertyName(RESCONST.AFTER)]
        public string? After { get; set; }

        [JsonPropertyName(COMMON.TOKEN)]
        public string? Token { get; set; }

        [JsonPropertyName(RESCONST.BEFORE)]
        public string? Before { get; set; }

        [JsonPropertyName(COMMON.STATUS)]
        public int Status { get; set; } = -1;

        [JsonPropertyName(COMMON.MESSAGE)]
        public required string Message { get; set; }

        [JsonPropertyName(CONST.Remaining)]
        public int Remaining { get; set; } = 100;

        [JsonPropertyName(CONST.Reset)]
        public int Reset { get; set; } = 100;

        [JsonPropertyName(CONST.Seconds)]
        [JsonIgnore]
        public int SecondsDelay { get => (int) Math.Round((double)(Reset + 1) / Remaining) * 5; }
    }
}
