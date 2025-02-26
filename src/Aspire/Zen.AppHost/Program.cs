var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("zen-sql")
    .WithDataVolume("zen-sql-volume");

var couponDb = sql.AddDatabase("coupon-db", "Coupon_Db");

builder.AddProject<Projects.Zen_Services_Coupon_API>("zen-services-coupon-api")
    .WithReference(couponDb)
    .WaitFor(couponDb)
    .WithEnvironment("OTEL_LOGS_EXPORTER", "console"); ;

builder.Build().Run();
