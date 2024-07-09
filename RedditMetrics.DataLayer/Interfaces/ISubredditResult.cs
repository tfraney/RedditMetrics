namespace RedditMetrics.DataLayer.Interfaces
{
    public interface ISubredditResult 
    {
        string SubRedditName { get; set; }
        int Status { get; set; }      
       string? Message { get; set; }
       string? Log { get; set; }
       string Kind { get; set; }        
    
    }
}
