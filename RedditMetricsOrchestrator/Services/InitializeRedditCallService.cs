using RedditMetrics.DataLayer.Interfaces;
using RedditMetricsOrchestrator.Workers;

using ERRMSG = RedditMetrics.DataLayer.FunctionConstants.ErrorMessages;
using MSG = RedditMetrics.DataLayer.FunctionConstants.Messages;
using QUERY = RedditMetrics.DataLayer.FunctionConstants.Query;

namespace RedditMetricsOrchestrator.Services
{
    public class InitializeRedditCallService(ILogger<InitializeRedditCallService> logger, IApiClientData clientData,
                                            IConfigurationManager config) : BackgroundService
    {
        private readonly ILogger<InitializeRedditCallService> _logger = logger;
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                //wait for other projects to start
                Task.Delay(1500, stoppingToken).Wait(stoppingToken);

                List<Task> tasks = [];
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(MSG.ORCH_BEGIN_MSG);

                    var myBrnchArray = config.GetSection(QUERY.ORCH_CONF_BRANCHES).Get<string[][]>();
                    clientData.Init(config.GetSection(QUERY.ORCH_CONF_CLIENTS).Get<string[][]>());
                    var host = config.GetValue<string>(QUERY.ORCH_CONF_APIHOST);
                    var authAction = config.GetValue<string>(QUERY.ORCH_APICONF_AUTH);
                    var dataAction = config.GetValue<string>(QUERY.OTCH_APICONF_ACTION);
                    var nextDataAction = config.GetValue<string>(QUERY.OTCH_NEXTAPICONF_ACTION);
                    var beforeDataAction = config.GetValue<string>(QUERY.OTCH_NEWAPICONF_ACTION);

                    if (myBrnchArray != null && myBrnchArray.Length > 0 && !string.IsNullOrWhiteSpace(host) &&
                        clientData != null && !string.IsNullOrWhiteSpace(authAction) && !string.IsNullOrWhiteSpace(dataAction) &&
                        !string.IsNullOrWhiteSpace(nextDataAction) && !string.IsNullOrWhiteSpace(beforeDataAction)) {
                        int loop = 0;
                        foreach (var item in myBrnchArray) 
                        {                 
                            try
                            {
                                if (item != null && item.Length == 4)
                                {
                                    _logger.LogInformation(MSG.ORCH_BEGIN_WORKER_MSG, item[1], item[0], DateTimeOffset.Now);                             
                                    tasks.Add(new MeasureRedditDataWorker(_logger, clientData, host,authAction, dataAction,nextDataAction,beforeDataAction)
                                                                         .Execute(item[1], item[3], item[0], item[2], loop,null, stoppingToken));

                                }
                                else throw new Exception(ERRMSG.ORCH_WORKER_CONFIG_ERROR);
                            }
                            catch (AggregateException ae) 
                            { 
                                _logger.LogError(ERRMSG.ORCH_CRITICALTASK_ERROR, item[1], item[0], ae.InnerException?.Message ?? ae.Message); 
                            }
                            catch (Exception ex) { _logger.LogError(ERRMSG.ORCH_CRITICALTASK_ERROR, item[1], item[0], ex.Message); }
                          
                        }

                        if (tasks.Count > 0)
                        {
                            _logger.LogInformation(MSG.ORCH_WORKER_RUNNING, DateTimeOffset.Now);
                            await Task.WhenAll(tasks);
                        }
                    }

                    if (tasks.Count == 0) _logger.LogError(ERRMSG.ORCH_CONFIG_ERROR);
                    else tasks?.Clear();                  
                }                
            }
        }
    }
}
