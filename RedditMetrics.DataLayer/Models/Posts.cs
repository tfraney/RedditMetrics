using System.Text.Json.Serialization;
using COMMONCONST = RedditMetrics.DataLayer.FunctionConstants.CommonField;
using RESCONST = RedditMetrics.DataLayer.FunctionConstants.PostResultFields;
using POSTCONST = RedditMetrics.DataLayer.FunctionConstants.SinglePostField;

namespace RedditMetrics.DataLayer.Models
{
    public class PostResult 
    {
        [JsonPropertyName(RESCONST.AFTER)]
        public string? After { get; set; }

        [JsonPropertyName(RESCONST.BEFORE)]
        public string? Before { get; set; }

        [JsonPropertyName(RESCONST.DIST)]
        public int Dist { get; set; } = 0;

        [JsonPropertyName(COMMONCONST.CHILDREN)]
        public Post[]? Children { get; set; }
    }

    public class Post
    {
        [JsonPropertyName(COMMONCONST.KIND)]
        public string? Kind { get; set; }

        [JsonPropertyName(COMMONCONST.DATA)]
        public PostData? Data { get; set; }
    }

    
    public class PostData 
    {
        [JsonPropertyName(POSTCONST.POSTNAME)]
        public required string Name { get; set; }

        [JsonPropertyName(POSTCONST.AUTHORNAME)]
        public required string Author { get; set; }

        [JsonPropertyName(POSTCONST.TITLE)]
        public string? Title { get; set; }

        [JsonPropertyName(POSTCONST.DOWNS)]
        public required int Downs { get; set; } = 0;

        [JsonPropertyName(POSTCONST.COMMENTS)]
        public int Comments { get; set; } = 0;

        [JsonPropertyName(POSTCONST.URL)]
        public required string Url { get; set; }

        [JsonPropertyName(POSTCONST.UPS)]
        public required int Ups { get; set; } = 0;

        [JsonPropertyName(POSTCONST.AUTHORID)]
        public required string Author_FullName { get; set; }

        [JsonPropertyName(POSTCONST.CREATEDDATE)]
        public decimal Created_UTC { get; set; } = 0;
    }
}
