var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("hive-redis");

var orleans = builder.AddOrleans("hive-cluster")
    .WithClustering(redis)
    .WithGrainStorage("queries", redis);

var app = builder.AddProject<Projects.QueryHive_Web>("hive-web")
    .WithExternalHttpEndpoints()
    .WithReference(orleans);


builder.Build().Run();
