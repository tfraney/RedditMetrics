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


namespace RedditMetricsFuncs
{
    public class NewPost(ILogger<NewPost> log, IProducerWrapper producer)
    {
        private static readonly string _action = QUERYCONST.NEWACTION;
        private readonly ILogger<NewPost> _log = log;       
        private readonly IProducerWrapper _producer = producer;

        [Function(nameof(NewPost))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, CONST.GET, CONST.POST, Route = null)] HttpRequestData req)
        {
            using IRedditReader reader = new RedditReader();
            HeaderData result = await ReaderFunction.ExecuteSubRedditReader(req, _log, _producer, reader, _action);
            return new OkObjectResult(result);
        }
    }
}
