using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetricsSmartReporter.Managers;

namespace RedditMetricsSmartReporter.Simponents
{
    public static class BucketListSimponent
    {
        public static void MapGet(WebApplication app)
        {

            app.MapGet(@"/api/hot/{name}", Results<Ok<ISubRedditStackItem[]>, NotFound> (HotBucketManager hotBucket, string name) =>
                     hotBucket.Stack.GetBucketList(name) is { } list
                              ? TypedResults.Ok(list)
                              : TypedResults.NotFound()
                      ).WithName(@"GetHottestPosts")
                      .WithOpenApi(x => new OpenApiOperation(x)
                      {
                          Summary = @"Get Hottest Posts by subreddit name.",
                          Description = @"Returns the top 25 hottest rated posts by comments by subreddit.",
                          Tags = new List<OpenApiTag> { new() { Name = @"Reddit Metrics Hottest Post" } }
                      });


            app.MapGet(@"/api/top/{name}", Results<Ok<ISubRedditStackItem[]>, NotFound> (TopBucketManager topBucket, string name) =>
                    topBucket.Stack.GetBucketList(name) is { } list
                             ? TypedResults.Ok(list)
                             : TypedResults.NotFound()
                     ).WithName(@"GetTopPosts")
                       .WithOpenApi(x => new OpenApiOperation(x)
                     {
                          Summary = @"Get the Top rated Posts by subreddit name.",
                          Description = @"Returns the top 25 most voted posts by subreddit.",
                          Tags = new List<OpenApiTag> { new() { Name = @"Reddit Metrics Top Post" } }
                     });

            app.MapGet(@"/api/new/{name}", Results<Ok<ISubRedditStackItem[]>, NotFound> (NewBucketManager newBucket, string name) =>
                   newBucket.Stack.GetBucketList(name) is { } list
                            ? TypedResults.Ok(list)
                            : TypedResults.NotFound()
                    ).WithName(@"GetNewPosts")
                      .WithOpenApi(x => new OpenApiOperation(x)
                      {
                          Summary = @"Get the Latest Posts by subreddit name.",
                          Description = @"Returns the top 25 latest posts by subreddit.",
                          Tags = new List<OpenApiTag> { new() { Name = @"Reddit Metrics Latest Post" } }
                      });

            app.MapGet(@"/api/BestAuthors/{name}", Results<Ok<IAuthorStackItem[]>, NotFound> (NewBucketManager newBucket, string name) =>
                 newBucket.GetHighestPostingAuthors(name) is { } authorlist
                          ? TypedResults.Ok(authorlist)
                          : TypedResults.NotFound()
                  ).WithName(@"GetBestAuthors")
                    .WithOpenApi(x => new OpenApiOperation(x)
                    {
                        Summary = @"Get the highest contributing authors by subreddit name.",
                        Description = @"Returns the top 25 authors posting for subreddit.",
                        Tags = new List<OpenApiTag> { new() { Name = @"Reddit Metrics Author Post" } }
                    });


        }

    }
  
}
