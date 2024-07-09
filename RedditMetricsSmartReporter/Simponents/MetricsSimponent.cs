using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using RedditMetrics.DataLayer.Interfaces;

namespace RedditMetricsSmartReporter.Simponents
{
    public static class MetricsListSimponent
    {
        public static void MapGet(WebApplication app)
        {

            app.MapGet(@"/api/metrics/{name}", Results<Ok<IEnumerable<IMetricItem>>, NotFound> (IMetricData metrics, string name) =>
                     metrics.MetricList.TryGetValue(name, out IMetricSubRedditData? value) && value.Items?.Values is { } metricDict
                              ? TypedResults.Ok(metricDict.Select(x => x))
                              : TypedResults.NotFound()
                      ).WithName(@"GetSubRedditMetrics")
                      .WithOpenApi(x => new OpenApiOperation(x)
                      {
                          Summary = @"Get Latest Metrics by subreddit name.",
                          Description = @"Returns the metric data for a subreddit.",
                          Tags = new List<OpenApiTag> { new() { Name = @"Reddit Metrics Post" } }
                      });
        }
    }  
}
