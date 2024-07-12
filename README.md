### RedditMetrics
## RedditMetrics by Thomas Franey


#Project hours: Reasearching and fighting Reddit API, and .net open source changes on azure functions for most part.
               Architecting bewteen othe revents within last few weeks.

#UPDATE: Fixed a way to Authenticate by user id and password. it searches all new posts for users now (reddit paging itself is buggy)
 


#Project: 
          Multi-Factpr microservice architecture with Kafka and Azure influence. N-tier libraries
         .Net core 8.0 style dependency injection (domain approach)
          comminication by API and Kafka message queues.
          use of minamum API structure for .net 8.0



### --wish list 

##Wishlist : FIXED: Reddit Authentication does not wrk or there is no proper instructions (so for now used the config file to store token) 
##          FIXED: Token may not work after authenticating in postman. See new instructions
##          FIXED SOME: need more time to clean up constant strings.
##          wanted to add interrupts for all api actions. (domain develp0ment approach)
##          wanted a client side api that talks to the reporter, but now just use reporter to grab the data (wanted signalR as well)

           


##download postman for API tests


##setting up kafka: use c:\KafKa , install Java
for your instructions (use c:\kafka): https://dzone.com/articles/running-apache-kafka-on-windows-os

##starts Kafka:
# .\bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties

# .\bin\windows\kafka-server-start.bat .\config\server.properties

##add topics if you have not yet:
# .\bin\windows\kafka-console-consumer.bat --bootstrap-server localhost:9092 --topic posts-new --from-beginning  
# .\bin\windows\kafka-console-consumer.bat --bootstrap-server localhost:9092 --topic posts-hot --from-beginning 
# .\bin\windows\kafka-console-consumer.bat --bootstrap-server localhost:9092 --topic posts-top --from-beginning 

##shows what topics are registered. (it will stay on your system)
# .\bin\windows\kafka-topics.bat --list --bootstrap-server localhost:9092


##Requirements.
#Visual studios 2022 - c# 12
#Install Azure functions in VS installer to acquire azure ability and Azurite
#Azurite should be part of VS 2022.

#Project was meant to be split into multiple solutions, but decide to keep nder one for easy git cloning.
#under solutions property, set multiple project to start on RedditMetricsFuncs (azure functions),
#RedditMetricsOrchestrator (timing chain to call multiple requests to functions to throttle),
#RedditMetricsSmartREporter (buckes the Kafka messages into metrics to diplay through API)

##in appsettings.Json for Orchestartor, is where to set your nodes (or branches of API calls to the http triggers)
##ex:

#apiHost points the the http functions (RedditMetricsFunc) to access the backend reddit data
#apiHAuth setting format path to fill in a script to call reddit api for auth,

#apiAction setting format path to fill in a script to call reddit api for reads (hot, new , top, etc),

 "apiHost": "http://localhost:7125",
 "apiAuth": "api/auth?name={0}&authdata={1}&login={2}",
 "apiAction": "api/{3}?name={0}&authdata={1}&count={2}",

#subreddit name, the action (Newpost,TopPost,HotPost), and ID of client (name after youa app name), limit of records to pull per hit.
 "myRedditBranches": [
   [ "funny", "NewPost", "metrics", "100" ],
   [ "science", "NewPost", "metrics", "100" ],
   [ "funny", "HotPost", "metrics", "25" ],
   [ "funny", "TopPost", "metrics", "25" ],
   [ "science", "HotPost", "metrics", "25" ],
   [ "science", "TopPost", "metrics", "25" ]
 ],

#Set the client is, name of app (client), bit 64 version of your (client id: secret), and but version of your user (login id, password) for reddit, other 2 are left blank.
 "myRedditClients": [
   [ "metrics", "X25iempwRWtDdVh4QlE4QjhGS0p2QTp4ZlNVVVRMZWlwQXJDNjlyNklxdEFnQlNEZExOdFE=", "TmVydm91c0FsdGVybmF0aXZlNzA7Q2h1Y2s4NTcxJA==", "", "" ],
   [ "metrics2", "cEVpdEV6bHdvdkIxcjZkaFpxdUcyUTt1QUpiY2dJYVg0WG5yX2ZvOE9hM2FtbUpuYzBLa0E=", "TmVydm91c0FsdGVybmF0aXZlNzA7Q2h1Y2s4NTcxJA==", "", "" ],
   [ "metrics3", "TGJyclpUZHRacHlleUZLWUFLUnVzQTtTbmJhN0lwMkFmZW1hWjJxbTBkVXd3anRBXzkwbnc=", "TmVydm91c0FsdGVybmF0aXZlNzA7Q2h1Y2s4NTcxJA==", "", "" ]
                 
 ]






##after this: run project, should have 3 windows


##testing front end API from reporter:
##determine your pot numer for front end spi (xxxx) - most likely 7020

#to get single metrics like hottest post, top contributer, etc for subreddit r/funny (https://localhost:7020/api/metrics/funny).

##to list tables of top 25
#https://localhost:7020/api/top/funny (top posts in votes)
#https://localhost:7020/api/hot/funny (hottest posts , shoulde for comment count?)
#https://localhost:7020/api/new/funny (top 100 new posts) -- sorry there is no full list to analyze all contributer post count yet.
#https://localhost:7020/api/BestAuthors/funny (contributers with the highest posts - based on current 100 new posts loaded in bucket)

### azuzu77@yahoo.com is ny email.

