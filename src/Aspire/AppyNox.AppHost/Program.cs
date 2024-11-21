using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var db = builder.AddPostgres("appynox-db")
    .WithDataVolume("appynox-db-volume")
    .WithPgAdmin();

var ssoDb = db.AddDatabase("appynox-sso-db", "AppyNox_Sso");
var ssoSagaDb = db.AddDatabase("appynox-sso-saga-db", "AppyNox_Sso_Saga");
var licenseDb = db.AddDatabase("appynox-license-db", "AppyNox_License");
var couponDb = db.AddDatabase("appynox-coupon-db", "AppyNox_Coupon");

var redis = builder.AddRedis("appynox-cache")
    .WithRedisInsight();

var consul = builder.AddContainer("appynox-consul", "")
    .WithImageRegistry("hashicorp")
    .WithImage("consul")
    .WithHttpEndpoint(8500, 8500);

var seq = builder.AddSeq("seq")
    .ExcludeFromManifest();

// For NoxLogger
var appynoxSeq = builder.AddSeq("appynox-seq")
    .ExcludeFromManifest();

var internalRabbitmqUsername = builder.AddParameter(configuration["MessageBroker:Internal:Username"] ?? "quest");
var internalRabbitmqPassword = builder.AddParameter(configuration["MessageBroker:Internal:Password"] ?? "quest");
var externalRabbitmqUsername = builder.AddParameter(configuration["MessageBroker:External:Username"] ?? "quest");
var externalRabbitmqPassword = builder.AddParameter(configuration["MessageBroker:External:Password"] ?? "quest");

var internalRabbitmq = builder.AddRabbitMQ("appynox-rabbitmq", internalRabbitmqUsername, internalRabbitmqPassword, 5672)
    .WithManagementPlugin();

var externalRabbitmq = builder.AddRabbitMQ("external-rabbitmq", externalRabbitmqUsername, externalRabbitmqPassword, port: 5673)
    .WithManagementPlugin();

var ssoApi = builder.AddProject<AppyNox_Services_Sso_WebAPI>("appynox-sso-api")
    .WithReference(redis)
    .WithReference(internalRabbitmq)
    .WithReference(externalRabbitmq)
    .WithReference(ssoDb)
    .WithReference(ssoSagaDb)
    .WithReference(seq)
    .WithReference(appynoxSeq)
    .WaitFor(consul)
    .WaitFor(internalRabbitmq)
    .WaitFor(externalRabbitmq);

var couponApi = builder.AddProject<AppyNox_Services_Coupon_WebAPI>("appynox-coupon-api")
    .WithReference(redis)
    .WithReference(internalRabbitmq)
    .WithReference(couponDb)
    .WithReference(seq)
    .WithReference(appynoxSeq)
    .WaitFor(consul)
    .WaitFor(internalRabbitmq);

var licenseApi = builder.AddProject<AppyNox_Services_License_WebAPI>("appynox-license-api")
    .WithReference(redis)
    .WithReference(internalRabbitmq)
    .WithReference(licenseDb)
    .WithReference(seq)
    .WithReference(appynoxSeq)
    .WaitFor(consul)
    .WaitFor(internalRabbitmq);

var gateway = builder.AddProject<AppyNox_Gateway_OcelotGateway>("appynox-gateway")
    .WaitFor(consul);

builder.Build().Run();
