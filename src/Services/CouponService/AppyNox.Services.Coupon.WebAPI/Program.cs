using AppyNox.Services.Coupon.WebAPI.Filters;
using AutoWrapper;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

#region [ SSL Configuration ]

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    string fileName = string.Empty;

    if (builder.Environment.IsDevelopment())
    {
        fileName = Directory.GetCurrentDirectory() + "/ssl/coupon-service.pfx";
    }
    else if (builder.Environment.IsProduction())
    {
        fileName = "/https/coupon-service.pfx";
    }

    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps(fileName ?? throw new InvalidOperationException("SSL certificate file path could not be determined."), "happi2023");
    });
});

#endregion

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(QueryParametersActionFilter));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppyNox.Services.Coupon.Infrastructure.DependencyInjection.ConfigureServices(builder.Services, configuration);
AppyNox.Services.Coupon.Application.DependencyInjection.ConfigureServices(builder.Services, configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { IsApiOnly = true, ShowApiVersion = true, ApiVersion = "1.0", UseApiProblemDetailsException = true });

AppyNox.Services.Coupon.Infrastructure.DependencyInjection.ApplyMigrations(app.Services);

app.Run();