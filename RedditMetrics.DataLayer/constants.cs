namespace RedditMetrics.DataLayer
{
    public static class FunctionConstants
    {
        public const string LISTING = @"Listing";
        public const string GET = @"get";
        public const string POST = @"post";
        public const string DEFAPI = @"oauth.reddit.com";
        public const string APISTR = @"r/{0}/{1}?limit={2}&count={2}";
        public const string AFTER_APISTR = @"r/{0}/{1}?limit={2}&count={2}&after={3}";
        public const string BEFORE_APISTR = @"r/{0}/{1}?limit={2}&count={2}&before={3}";



        public const string LOGIN = @"api/v1/authorize?client_id={0}&response_type=code&state=Metric&redirect_uri={1}&duration=permanent&scope=read";

        public const string POSTS_NEW = @"posts-new";
        public const string POSTS_HOT = @"posts-hot";
        public const string POSTS_TOP = @"posts-top";
        
        public static class ErrorMessages
        {
            public const string ERRNAME = @"Error";
            public const string NOSUBREDDITNAME  = @"SubReddit Name not Supplied.";
            public const string KAFKAFAILURE = @"KafKa Messaging Failure.";
            public const string NOACTION = @"Incorrect Action for subreddit {0}.";
            public const string NOSERVICE = @"SubReddit Reader Service not initialized.";
        }

        public static class Query
        {
            public const string SUBREDDITNAME = @"name";
            public const string PAGEAFTER = @"after";
            public const string PAGEBEFORE = @"before";
            public const string TOKEN = @"token";
            public const string COUNT = @"count";

            public const string NEWACTION = @"new.json";
            public const string TOPACTION = @"top.json";
            public const string HOTACTION = @"hot.json";
        }

        public static class Messages
        {
            public const string LOGINGIN = @"LOGIN IN FOR TOKEN (Authenticating Reddit).";
            public const string TOKENBACK = @"LOGIN Succeeded. Retrieved Token (Authenticating Reddit).";
            public const string UNAUTHORIZED = @"LOGIN Failed (Authentication for Reddit).";
            public const string FAILEDCALL = @"Reddit API call Failed to return data.";

            public const string SUCCESSFUL = @"Successful";
            public const string STARTLOG = @"C# HTTP trigger {0} processed a request.";
            public const string LATESTLOG = @"SubReddit {0} New Post before id: {1}.";
            public const string NEXTPAGELOG = @"SubReddit {0} New Post paging after {1}.";
            public const string NEWLOG = @"SubReddit {0} New Post starting.";
            public const string HOTLOG = @"SubReddit {0} Hot {1} posts.";
            public const string TOPVOTELOG = @"SubReddit {0} Top voted {1} posts.";
        }
        public static class CommonField
        {
            public const string KIND = @"kind";
            public const string DATA = @"data";
            public const string STATUS = @"status";
            public const string MESSAGE = @"message";
            public const string TOKEN = @"token";
            public const string LOG = @"log";
            public const string CHILDREN = @"children";
        }

        public static class HeaderField
        {
            public const string Remaining = @"x-ratelimit-remaining";
            public const string Reset = @"x-ratelimit-reset";
            public const string Seconds = @"estimate_seconds";
            public const string Action = @"action";
            public const string SubRedditName = @"name";
        }


        public static class PostResultFields
        {
            public const string BEFORE = @"before";
            public const string AFTER = @"after";
            public const string DIST = @"dist";
        }
        public static class SinglePostField
        {
            public const string SubRedditName = @"subreddit";

            public const string POSTNAME = @"name";

            public const string AUTHORNAME = @"author";

            public const string TITLE = @"title";

            public const string DOWNS = @"downs";

            public const string COMMENTS = @"num_comments";

            public const string URL = @"url";

            public const string UPS = @"ups";

            public const string AUTHORID = @"author_fullname";        

            public const string CREATEDDATE = @"created_utc";
        }
    }
}
