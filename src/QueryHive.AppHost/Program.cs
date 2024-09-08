var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("hive-redis");

var orleans = builder.AddOrleans("hive-cluster")
    .WithClustering(redis)
    .WithGrainStorage("queries", redis);

builder.Build().Run();
