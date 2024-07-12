using Confluent.Kafka;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.Business.KafkaWrappers;
using CONST = RedditMetrics.DataLayer.FunctionConstants.CommonMessages;

var host = new HostBuilder()
     .ConfigureFunctionsWorkerDefaults()
     .ConfigureServices(services =>
    { 
        var setting = Environment.GetEnvironmentVariable(CONST.PRODUCERSERVER) ?? ConfigurationManager.AppSettings[CONST.PRODUCERSERVER];       
        ProducerConfig config = new() { BootstrapServers = setting  };       
        services.AddSingleton<IProducerWrapper>(x =>
               ActivatorUtilities.CreateInstance<ProducerWrapper>(x, config));
        
    })    
    .Build();

host.Run();