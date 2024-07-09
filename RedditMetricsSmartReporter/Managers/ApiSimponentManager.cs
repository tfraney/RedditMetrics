using RedditMetricsSmartReporter.Simponents;

//maps all the simple APis in one static class for clean setup.
namespace RedditMetricsSmartReporter.Managers
{
    public static class ApiSimponentManager
    {
        public static void MapAll(WebApplication app)
        {
            BucketListSimponent.MapGet(app);
            MetricsListSimponent.MapGet(app);
        }
    }
}
