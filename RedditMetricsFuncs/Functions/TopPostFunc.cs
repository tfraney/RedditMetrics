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
    public  class TopPost(ILogger<TopPost> log, IProducerWrapper producer)
    {
        private static readonly string _action = QUERYCONST.TOPACTION;
        private static readonly string _logstring = MESSAGES.TOPVOTELOG;

        private readonly ILogger<TopPost> _log = log;      
        private readonly IProducerWrapper _producer = producer;

        [Function(nameof(TopPost))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, CONST.GET, CONST.POST, Route = null)] HttpRequestData req)
        {
            string MainAPI = Environment.GetEnvironmentVariable(CONST.READ_API);
            string topNextPI = Environment.GetEnvironmentVariable(CONST.READ_API_NEXT);
            string topNewAPI = Environment.GetEnvironmentVariable(CONST.READ_API_TOPNEW);

            using RequestParameterManager reqData = new(req);
            using IRedditReader reader = new RedditReader(MainAPI, topNextPI, topNewAPI, null);
            HeaderData result = await ReaderFunction.ExecuteSubRedditReader(reqData, _log, _producer, reader, _action, CONST.POSTS_TOP, _logstring);
            return new OkObjectResult(result);
        }
    }
}
