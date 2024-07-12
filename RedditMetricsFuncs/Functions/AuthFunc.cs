using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;

using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.Business.RedditReaders;
using RedditMetrics.Business;
using RedditMetrics.DataLayer.Models;

using QUERYCONST = RedditMetrics.DataLayer.FunctionConstants.Query;
using CONST = RedditMetrics.DataLayer.FunctionConstants;
using MESSAGES = RedditMetrics.DataLayer.FunctionConstants.Messages;
using System;


namespace RedditMetricsFuncs
{
    public  class Auth(ILogger<TopPost> log)
    {
        private static readonly string _logstring = MESSAGES.AUTHLOG;

        private readonly ILogger<TopPost> _log = log; 

        [Function(nameof(Auth))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, CONST.GET, CONST.POST, Route = null)] HttpRequestData req)
        {
            string authAPI = Environment.GetEnvironmentVariable(CONST.AUTH_API);
            using RequestParameterManager reqData = new(req);
            using IRedditReader reader = new RedditReader(null,null,null, authAPI);
            HeaderData result = (HeaderData) await ReaderFunction.PostAuthToReader(reqData, _log, reader, _logstring);
            return new OkObjectResult(result);
        }
       
    }
}
