using AutoWrapper;
using CouponService.Application.DTOUtilities;
using CouponService.Application.Services.Implementations;
using CouponService.Application.Services.Interfaces;
using CouponService.Domain.Interfaces;
using CouponService.Infrastructure.Data;
using CouponService.Infrastructure.Repositories;
using CouponService.WebAPI.Filters;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(QueryParametersActionFilter));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CouponDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(Assembly.Load("CouponService.Application"));
builder.Services.AddValidatorsFromAssembly(Assembly.Load("CouponService.Application"));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<DTOMappingRegistry>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseApiResponseAndExceptionWrapper(new AutoWrapperOptions { UseApiProblemDetailsException = true });

app.Run();
