using Zen.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("zen-sql", port: 61514)
    .WithDataVolume("zen-sql-volume");

var couponDb = sql.AddDatabase("coupon-db", "Coupon_Db");
var ssoDb = sql.AddDatabase("sso-db", "Sso_Db");

builder.AddProject<Projects.Zen_Services_Coupon_API>("zen-services-coupon-api")
    .WithReference(couponDb)
    .WaitFor(couponDb)
    .WithEnvironment("OTEL_LOGS_EXPORTER", "console")
    .WithSwaggerUI()
    .WithHangfireDashboard();

builder.AddProject<Projects.Zen_Services_Sso_API>("zen-services-sso-api")
    .WithReference(ssoDb)
    .WaitFor(ssoDb)
    .WithEnvironment("OTEL_LOGS_EXPORTER", "console")
    .WithSwaggerUI()
    .WithHangfireDashboard();

builder.Build().Run();
