namespace RedditMetrics.DataLayer.Interfaces
{
    public interface IHeaderData 
    {      
        public string Action { get; set; }      
        public string SubRedditName { get; set; }     
        public string? After { get; set; } 
        string? Before { get; set; } 
        int Status { get; set; }     
        string Message { get; set; }
        ITokenHeader? TokenData { get; set; }        
        int Remaining { get; set; }     
        int Reset { get; set; }     
        int MilisecondsDelay { get; }
        string? TokenReturned { get; }
    }
}
