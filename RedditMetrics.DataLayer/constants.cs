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

        public const string AUTH_API = @"AuthAPI";
        public const string READ_API = @"ReadAPI";
        public const string READ_API_NEXT = @"ReadAPINext";
        public const string READ_API_TOPNEW = @"ReadTopNew";

        public static class ErrorMessages
        {
            public const string ERRNAME = @"Error";
            public const string NOSUBREDDITNAME  = @"SubReddit Name not Supplied.";
            public const string KAFKAFAILURE = @"KafKa Messaging Failure.";
            public const string NOACTION = @"Incorrect Action for subreddit {0}.";
            public const string NOSERVICE = @"SubReddit Reader Service not initialized.";

            public const string AUTH_FAILED = @"Failed. Reddit Api = Auth";

            public const string WORKER_CRITICAL_FAILURE = @"Critical Error (500). Closing worker for this Reddit Http Reuest => Worker for {topic} : {sub} - {failure}";
            public const string WORKER_REQUESTISSUE = @"Http Request Issue => Worker for {topic} : {sub} : {msg}";
            public const string WORKER_LIMITEXCEEDED = @"Delay Warning (Too many calls) taking {} seconds to wait => Worker for {topic} : {sub} ";
            public const string WORKER_NOREQUESTDATA = @"Empty Http Request or null data returned from backend.";

            public const string ORCH_WORKER_CONFIG_ERROR = @"Configuration not properly set for worker branch";
            public const string ORCH_CONFIG_ERROR = @"Not All Configuration Settings not set for API or worker executions. Orchestration Stopped.";
            public const string ORCH_CRITICALTASK_ERROR = @"Orchestration Worker for {topic}{sub} critical fulure. {ex}";
        }

        public static class Query
        {
            public const string POST = @"POST";
            public const string GET = @"GET";

            public const string SUBREDDITNAME = @"name";
            public const string PAGEAFTER = @"after";
            public const string PAGEBEFORE = @"before";
            public const string AUTHDATA = @"authdata";
            public const string LOGIN = @"login";      
            public const string COUNT = @"count";

            public const string NEWTOPIC = @"NewPost";
            public const string NEWACTION = @"new.json";
            public const string TOPACTION = @"top.json";
            public const string HOTACTION = @"hot.json";

            public const string ORCH_CONF_BRANCHES = @"myRedditBranches";
            public const string ORCH_CONF_CLIENTS = @"myRedditClients";
            public const string ORCH_CONF_APIHOST = @"apiHost";
            public const string ORCH_APICONF_AUTH = @"apiAuth";
            public const string OTCH_APICONF_ACTION = @"apiAction";
            public const string OTCH_NEWAPICONF_ACTION = @"apiNewAction";
            public const string OTCH_NEXTAPICONF_ACTION = @"apiNextAction";
        }

        public static class Messages
        {
            public const string LOGGINGIN = @"LOGIN IN FOR TOKEN (Authenticating Reddit).";
            public const string TOKENBACK = @"LOGIN Succeeded. Retrieved Token (Authenticating Reddit).";
            public const string UNAUTHORIZED = @"LOGIN Failed (Authentication for Reddit).";
            public const string FAILEDCALL = @"Reddit API call Failed to return data.";

            public const string SUCCESSFUL = @"Successful";
            public const string STARTLOG = @"C# HTTP trigger {0} processed a request.";
            public const string LATESTLOG = @" before id: {0}.";
            public const string NEXTPAGELOG = @" after {0}.";
            public const string NEWLOG = @"SubReddit {0} New {1} Post{2}.";
            public const string HOTLOG = @"SubReddit {0} Hot {1} posts{2}.";
            public const string TOPVOTELOG = @"SubReddit {0} Top voted {1} posts{2}.";
            public const string AUTHLOG = @"Authenticating Reddit Client.";

            public const string AUTH_STARTED = @"Reddit Api Authetication Started. ";

            public const string WORKER_STARTBRANCH_LOG = @"Orchestrating metrics for subreddit {subreddit} - {topic}";
            public const string WORKER_NEW_LOG_COMPLETED = @"Orchestrating metrics for whole list of subreddit {subreddit} - {topic} is completed.";
            public const string WORKER_STARTBRANCH_BEFORE_LOG = @"More metrics for subreddit {subreddit} - {topic} : Newest before {before}";
            public const string WORKER_STARTBRANCH_AFTER_LOG = @"More metrics for subreddit {subreddit} - {topic} : next {cnt} After {after}";
            public const string WORKER_DELAYWARNING_LOG = @"Delay Warning (Thottling) to {time} miliseconds => Worker for {topic} : {sub}";
            public const string WORKER_REC_PROC = @"{topic}{subreddit} - New Records to be processed - {cnt}";

            public const string ORCH_BEGIN_MSG = @"Beginning Orchestration of API Workers.";
            public const string ORCH_BEGIN_WORKER_MSG = @"Worker {topic}{sub} to start at: {time}";
            public const string ORCH_WORKER_RUNNING = @"Workers running at: {time}";
        }
        public static class CommonField
        {
            public const string KIND = @"kind";
            public const string DATA = @"data";
            public const string STATUS = @"status";
            public const string MESSAGE = @"message";
            public const string RESET = @"reset";
            public const string REMAINING = @"remaining";
            public const string TOKEN_STAMP = @"token_date";
            public const string REFRESH_TOKEN = @"refreshtoken";
            public const string TOKEN = @"token";
            public const string MAXEXPIRETIME = @"maxExpireTime";        
            public const string TOKENDATA = @"tokendata";
            public const string BASICAUTH = @"basicauth";
            public const string LOG = @"log";
            public const string CHILDREN = @"children";
        }

        public static class HeaderField
        {
            public const string XRATELIMITREMAINING = @"x-ratelimit-remaining";
            public const string XRATELIMITRESET = @"x-ratelimit-reset";


            public const string SECONDS = @"estimate_seconds";
            public const string ACTIONS = @"action";
            public const string SUBREDDITNAME = @"subRedditName";
        }
        public static class CommonMessages
        {
            public const string AUTH_ACTION = @"Authentication";
            public const string BASIC = @"basic";
            public const string BEARER = @"bearer";
            public const string ERROR = @"Error";
            public const string LOGMESSAGE = @"{message}";
            public const string PRODUCERSERVER = @"PRODUCERSERVER";
        }
        public static class ReditTokenFields
        {
            public const string ACCESSTOKEN = @"access_token";
            public const string EXPIRESIN = @"expires_in";
            public const string REFRESHTOKEN = @"refresh_token";
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
