using RedditMetricsSmartReporter.Managers;
using RedditMetrics.DataLayer.Interfaces;
using RedditMetrics.DataLayer.Models;
using Confluent.Kafka;
using RedditMetricsSmartReporter.Service;
using RedditMetricsSmartReporter.Service.Wrappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
  .AddJwtBearer()
  .AddJwtBearer(@"LocalAuthIssuer");


builder.Services.AddSingleton<IMetricData, MetricData>();

builder.Services.AddSingleton<HotBucketManager>();
builder.Services.AddSingleton<TopBucketManager>();
builder.Services.AddSingleton<NewBucketManager>();

builder.Services.AddSingleton<NewConsumerWrapper>();
builder.Services.AddSingleton<TopConsumerWrapper>();
builder.Services.AddSingleton<HotConsumerWrapper>();

ConsumerConfig consumerConfig = builder.Configuration.GetSection(@"ConsumerConfig").Get<ConsumerConfig>() ?? new ConsumerConfig();
builder.Services.AddSingleton(consumerConfig);
builder.Services.AddHostedService<NewRedditMessageConsumer>();
builder.Services.AddHostedService<HotRedditMessageConsumer>();
builder.Services.AddHostedService<TopRedditMessageConsumer>();

var app = builder.Build();
app.UseHttpsRedirection();

ApiSimponentManager.MapAll(app);


app.Run();
