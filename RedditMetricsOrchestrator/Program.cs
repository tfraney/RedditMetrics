using RedditMetricsOrchestrator.Services;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;
using Microsoft.Extensions.Configuration;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IHeaderData, HeaderData>();
builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);
builder.Services.AddHostedService<InitializeRedditCallService>();
var host = builder.Build();
host.Run();
