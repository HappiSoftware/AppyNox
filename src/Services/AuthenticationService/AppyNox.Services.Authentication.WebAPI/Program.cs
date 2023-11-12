using AppyNox.Services.Authentication.Application.DtoUtilities;
using AppyNox.Services.Authentication.Infrastructure.Data;
using AppyNox.Services.Authentication.WebAPI.Configuration;
using AppyNox.Services.Authentication.WebAPI.Helpers;
using AppyNox.Services.Authentication.WebAPI.Managers.Implementations;
using AppyNox.Services.Authentication.WebAPI.Managers.Interfaces;
using AppyNox.Services.Authentication.WebAPI.Utilities;
using Asp.Versioning;
using AutoWrapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region [ SSL Configuration ]

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    string fileName = string.Empty;

    if (builder.Environment.IsDevelopment())
    {
        fileName = Directory.GetCurrentDirectory() + "/ssl/authentication-service.pfx";
    }
    else if (builder.Environment.IsProduction())
    {
        fileName = "/https/authentication-service.pfx";
    }

    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps(fileName ?? throw new InvalidOperationException("SSL certificate file path could not be determined."), "happi2023");
    });
});

#endregion

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

AppyNox.Services.Authentication.Infrastructure.DependencyInjection.ConfigureServices(builder.Services, configuration);

builder.Services.AddValidatorsFromAssembly(Assembly.Load("AppyNox.Services.Authentication.Application"));
builder.Services.AddAutoMapper(Assembly.Load("AppyNox.Services.Authentication.Application"));

builder.Services.AddSingleton<DtoMappingRegistry>();
builder.Services.AddScoped(typeof(DtoMappingHelper<>));
builder.Services.AddScoped<PasswordValidator<IdentityUser>>();
builder.Services.AddScoped<PasswordHasher<IdentityUser>>();

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

AppyNox.Services.Authentication.Infrastructure.DependencyInjection.ApplyMigrations(app.Services);

app.Run();