using AppyNox.Services.Authentication.Application;
using AppyNox.Services.Authentication.Application.Interfaces.Authentication;
using AppyNox.Services.Authentication.Domain.Entities;
using AppyNox.Services.Authentication.Infrastructure;
using AppyNox.Services.Authentication.Infrastructure.Data;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ControllerDependencies;
using AppyNox.Services.Authentication.WebAPI.Managers;
using AppyNox.Services.Authentication.WebAPI.Middlewares;
using AppyNox.Services.Authentication.WebAPI.Permission;
using AppyNox.Services.Base.API.Extensions;
using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.API.Permissions;
using AppyNox.Services.Base.Application.Interfaces.Loggers;
using AppyNox.Services.Base.Infrastructure.Extensions;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Asp.Versioning;
using AutoWrapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region [ Configuration Service ]

await builder.AddConsulConfiguration("AuthenticationService");
var configuration = builder.Configuration;

#endregion

#region [ Logger Setup ]

builder.Host.UseSerilog((context, services, config) =>
    config.ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext()
);
builder.Services.AddSingleton<INoxApiLogger, NoxApiLogger>();

#region [ Logger for Before DI Initialization ]

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
var logger = loggerFactory.CreateLogger<INoxLogger>();
NoxLogger noxLogger = new(logger, "AuthenticationHost");

#endregion

#endregion

#region [ Identity ]

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddSignInManager()
        .AddEntityFrameworkStores<IdentityDatabaseContext>().AddRoles<ApplicationRole>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Configure password requirements
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});

#endregion

#region [ Configure Services ]

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Web API v1", Version = "version 1" });
});

builder.Services.AddHealthChecks();

#endregion

#region [ Dependency Injection For Layers ]

noxLogger.LogInformation("Registering DI's for layers.");
builder.Services.AddAuthenticationInfrastructure(configuration, builder.Environment.GetEnvironment());
builder.Services.AddAuthenticationApplication(configuration);
noxLogger.LogInformation("Registering DI's for layers completed.");

#endregion

#region [ Dependency Injection Setup ]

builder.Services.AddScoped<PasswordValidator<ApplicationUser>>();
builder.Services.AddScoped<PasswordHasher<ApplicationUser>>();
builder.Services.AddScoped<UsersControllerBaseDependencies>();

#endregion

#region [ Jwt Settings ]

noxLogger.LogInformation("Registering JWT Configuration.");
var jwtConfiguration = new AuthenticationJwtConfiguration();
configuration.GetSection("JwtSettings:AppyNox").Bind(jwtConfiguration);

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "NoxSsoJwtScheme";
    options.DefaultChallengeScheme = "NoxSsoJwtScheme";
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfiguration.Issuer,
        ValidAudience = jwtConfiguration.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.GetSecretKeyBytes())
    };
})
.AddScheme<NoxSsoJwtAuthenticationHandlerOptions, NoxSsoJwtAuthenticationHandler>("NoxSsoJwtScheme", options =>
{
    options.Audience = "AppyNox";
});

builder.Services.AddScoped<ICustomTokenManager, JwtTokenManager>();
builder.Services.AddScoped<ICustomUserManager, CustomUserManager>();

// Add Policy-based Authorization
builder.Services.AddAuthorization(options =>
{
    List<string> _claims = [.. Permissions.Users.Metrics, .. Permissions.Roles.Metrics];

    foreach (var item in _claims)
    {
        options.AddPolicy(item, builder =>
        {
            builder.AddRequirements(new PermissionRequirement(item, "API.Permission"));
        });
    }
});

builder.Services.AddScoped<IAuthorizationHandler, NoxSsoAuthorizationHandler>();
noxLogger.LogInformation("Registering JWT Configuration completed.");

#endregion

var app = builder.Build();

#region [ Pipeline ]

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = true, ShowApiVersion = true, ApiVersion = "1.0" });
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<AuthenticationContextMiddleware>();

app.MapControllers();

app.UseHealthChecks("/api/health");

#endregion

#region [ Hosted Services ]

var consulHostedService = app.Services.GetServices<IHostedService>()
    .OfType<ConsulHostedService>()
    .First();

consulHostedService.OnConsulConnectionFailed += (Exception ex) =>
{
    noxLogger.LogError(ex, "Consul connection failed. Stopping application.");
    var lifeTime = app.Services.GetService<IHostApplicationLifetime>();
    lifeTime?.StopApplication();
    return Task.CompletedTask;
};

#endregion

#region [ Migrations ]

app.Services.ApplyMigrations<IdentityDatabaseContext>();

app.Services.ApplyMigrations<IdentitySagaDatabaseContext>();

#endregion

app.Run();