using System.Threading.Tasks;
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


namespace RedditMetricsFuncs
{
    public  class TopPostTest(ILogger<TopPostTest> log, IProducerWrapper producer)
    {
        private static readonly string _action = QUERYCONST.TOPACTION;
        private static readonly string _logstring = MESSAGES.TOPVOTELOG;

        private readonly ILogger<TopPostTest> _log = log;      
        private readonly IProducerWrapper _producer = producer;

        [Function(nameof(TopPostTest))]
        public async Task<HeaderData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, CONST.GET, CONST.POST, Route = null)] HttpRequestData req)
        {    

            using RequestParameterManager reqData = new(req);
            using IRedditReader reader = new TestRedditReader();
            HeaderData result = await ReaderFunction.ExecuteSubRedditReader(reqData, _log, _producer, reader,_action, CONST.POSTS_TOP, _logstring);
            return result;
        }
    }
}
