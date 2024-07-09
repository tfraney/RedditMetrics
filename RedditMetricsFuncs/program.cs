using Confluent.Kafka;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.Business.KafkaWrappers;

var host = new HostBuilder()
     .ConfigureFunctionsWorkerDefaults()
     .ConfigureServices(services =>
    {
        var setting = Environment.GetEnvironmentVariable("PRODUCERSERVER") ?? ConfigurationManager.AppSettings["PRODUCERSERVER"];       
        ProducerConfig config = new() { BootstrapServers = setting  };       
        services.AddSingleton<IProducerWrapper>(x =>
               ActivatorUtilities.CreateInstance<ProducerWrapper>(x, config));
        
    })    
    .Build();

host.Run();