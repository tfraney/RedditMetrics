using System.Text.Json.Serialization;

namespace RedditMetrics.DataLayer.Models
{
    public class ApiValue<T>
    {
        [JsonPropertyName("value")]
        public T? Value { get; set; }
    }
}
