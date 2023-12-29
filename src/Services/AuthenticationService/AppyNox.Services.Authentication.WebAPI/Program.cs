using AppyNox.Services.Authentication.Application;
using AppyNox.Services.Authentication.Infrastructure;
using AppyNox.Services.Authentication.Infrastructure.Data;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ControllerDependencies;
using AppyNox.Services.Authentication.WebAPI.Managers.Implementations;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AppyNox.Services.Authentication.WebAPI.Utilities;
using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.Logger;
using AppyNox.Services.Base.API.Middleware;
using AppyNox.Services.Base.Infrastructure.Helpers;
using AppyNox.Services.Base.Infrastructure.HostedServices;
using AppyNox.Services.Base.Infrastructure.Services.LoggerService;
using Asp.Versioning;
using AutoWrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

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

// Add Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddSignInManager()
        .AddEntityFrameworkStores<IdentityDbContext>().AddRoles<IdentityRole>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Configure password requirements
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
});

#region [ DI For Layers ]

noxLogger.LogInformation("Registering DI's for layers.");
builder.Services.AddAuthenticationInfrastructure(configuration, builder.Environment.GetEnvironment());
builder.Services.AddAuthenticationApplication(configuration);
noxLogger.LogInformation("Registering DI's for layers completed.");

#endregion

builder.Services.AddHealthChecks();

builder.Services.AddScoped<PasswordValidator<IdentityUser>>();
builder.Services.AddScoped<PasswordHasher<IdentityUser>>();
builder.Services.AddScoped<UsersControllerBaseDependencies>();

#region [ Jwt Settings ]

noxLogger.LogInformation("Registering JWT Configuration.");
var jwtConfiguration = new JwtConfiguration();
configuration.GetSection("JwtSettings").Bind(jwtConfiguration);
builder.Services.AddSingleton(jwtConfiguration);

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
noxLogger.LogInformation("Registering JWT Configuration completed.");

#endregion

var app = builder.Build();

#region [ Pipeline ]

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { UseApiProblemDetailsException = true });

app.UseHealthChecks("/api/health");

#endregion

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

app.Services.ApplyMigrations<IdentityDbContext>();

app.Run();