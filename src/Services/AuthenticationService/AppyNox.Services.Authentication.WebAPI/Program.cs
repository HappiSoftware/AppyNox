using AppyNox.Services.Authentication.Infrastructure.Data;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.ControllerDependencies;
using AppyNox.Services.Authentication.WebAPI.Managers.Implementations;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AppyNox.Services.Authentication.WebAPI.Utilities;
using AppyNox.Services.Base.API.Helpers;
using AppyNox.Services.Base.API.Logger;
using AppyNox.Services.Base.Infrastructure.Helpers;
using AppyNox.Services.Base.Infrastructure.HostedServices;
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

builder.Services.AddScoped<INoxApiLogger, NoxApiLogger>();

var loggerFactory = LoggerFactory.Create(loggingBuilder =>
{
    loggingBuilder.AddSerilog();
});
var logger = loggerFactory.CreateLogger<Program>();

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

AppyNox.Services.Authentication.Infrastructure.DependencyInjection.AddAuthenticationInfrastructure(builder.Services, configuration, builder.Environment.GetEnvironment());
AppyNox.Services.Authentication.Application.DependencyInjection.ConfigureServices(builder.Services, configuration);

builder.Services.AddHealthChecks();

builder.Services.AddScoped<PasswordValidator<IdentityUser>>();
builder.Services.AddScoped<PasswordHasher<IdentityUser>>();
builder.Services.AddScoped<UsersControllerBaseDependencies>();

#region [ Jwt Settings ]

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

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { UseApiProblemDetailsException = true });

app.UseHealthChecks("/api/health");

var consulHostedService = app.Services.GetServices<IHostedService>()
    .OfType<ConsulHostedService>()
    .First();

consulHostedService.OnConsulConnectionFailed += () =>
{
    var lifeTime = app.Services.GetService<IHostApplicationLifetime>();
    lifeTime?.StopApplication();
    return Task.CompletedTask;
};

app.Services.ApplyMigrations<IdentityDbContext>();

app.Run();