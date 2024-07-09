using Confluent.Kafka;
using RedditMetricsOrchestrator.Workers;
using CONST = RedditMetrics.DataLayer.FunctionConstants;

namespace RedditMetricsOrchestrator.Services
{
    public class InitializeRedditCallService(ILogger<InitializeRedditCallService> logger, IConfigurationManager config) : BackgroundService
    {
        private readonly ILogger<InitializeRedditCallService> _logger = logger;
        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!stoppingToken.IsCancellationRequested)
            {
                //wait for other projects to start
                Task.Delay(3500, stoppingToken).Wait(stoppingToken);

                List<Task> tasks = [];
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(@"Begin Orchestration Mechanism.");

                    var myArray = config.GetSection(@"MyRedditBranches").Get<string[][]>();                 
                    var host = config.GetValue<string>(@"apiHost");

                    if (myArray != null &&  myArray.Length > 0 && !string.IsNullOrWhiteSpace(host)) { 
                        
                        foreach (var item in myArray)
                        {
                            try
                            {
                                if (item != null && item.Length == 4)
                                {
                                    _logger.LogInformation(@"Worker {topic}{sub} to start at: {time}", item[1], item[0], DateTimeOffset.Now);
                                    var worker = new MeasureRedditDataWorker(_logger);
                                    tasks.Add(worker.Execute(host, item[1], item[3], item[0], item[2], stoppingToken));
                                }
                                else throw new Exception(@"Configuration not properly set for worker branch");
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(@"Worker {topic}{sub} errored out. {ex}", item[1], item[0], ex.Message);
                            }
                        }
                        if (tasks.Count > 0)
                        {
                            _logger.LogInformation(@"Workers running at: {time}", DateTimeOffset.Now);
                            await Task.WhenAll(tasks);
                        }

                    }
                    if (tasks.Count == 0) _logger.LogWarning(@"Configuration not set for workers.");
                    else tasks?.Clear();                  
                }                
            }           

        }
    }
}
