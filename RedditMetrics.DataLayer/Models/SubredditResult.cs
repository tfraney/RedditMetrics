using RedditMetrics.DataLayer.Interfaces;
using System.Text.Json.Serialization;
using CONST = RedditMetrics.DataLayer.FunctionConstants.CommonField;

namespace RedditMetrics.DataLayer.Models
{
    public class SubredditResult : ISubredditResult
    {
       
        [JsonIgnore]
        public required string SubRedditName { get; set; }

        [JsonPropertyName(CONST.STATUS)]
        [JsonIgnore]
        public int Status { get; set; } = -1;

        [JsonPropertyName(CONST.MESSAGE)]
        [JsonIgnore]
        public string? Message { get; set; }

        [JsonPropertyName(CONST.LOG)]
        [JsonIgnore]
        public string? Log { get; set; }

        [JsonPropertyName(CONST.KIND)]
        public required string Kind { get; set; }

        [JsonPropertyName(CONST.DATA)]
        public PostResult? Data { get; set; }
  
    }
}
