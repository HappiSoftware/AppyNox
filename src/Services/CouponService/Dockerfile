FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root

# Use the official .NET 8 SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/Services/CouponService

# Copy the .csproj files and restore dependencies for Domain
COPY ["AppyNox.Services.Coupon.Domain/AppyNox.Services.Coupon.Domain.csproj", "AppyNox.Services.Coupon.Domain/"]
RUN dotnet restore "AppyNox.Services.Coupon.Domain/AppyNox.Services.Coupon.Domain.csproj"

# Copy the .csproj files and restore dependencies for Infrastructure
COPY ["AppyNox.Services.Coupon.Infrastructure/AppyNox.Services.Coupon.Infrastructure.csproj", "AppyNox.Services.Coupon.Infrastructure/"]
RUN dotnet restore "AppyNox.Services.Coupon.Infrastructure/AppyNox.Services.Coupon.Infrastructure.csproj"

# Copy the .csproj files and restore dependencies for Application
COPY ["AppyNox.Services.Coupon.Application/AppyNox.Services.Coupon.Application.csproj", "AppyNox.Services.Coupon.Application/"]
RUN dotnet restore "AppyNox.Services.Coupon.Application/AppyNox.Services.Coupon.Application.csproj"

# Copy the .csproj file and restore dependencies for WebAPI
COPY ["AppyNox.Services.Coupon.WebAPI/AppyNox.Services.Coupon.WebAPI.csproj", "AppyNox.Services.Coupon.WebAPI/"]
RUN dotnet restore "AppyNox.Services.Coupon.WebAPI/AppyNox.Services.Coupon.WebAPI.csproj"

# Copy the remaining source code
COPY . .

# Publish the Domain layer
WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.Domain"
RUN dotnet build "AppyNox.Services.Coupon.Domain.csproj" -c Production -o /app/build

# Publish the Infrastructure layer
WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.Infrastructure"
RUN dotnet build "AppyNox.Services.Coupon.Infrastructure.csproj" -c Production -o /app/build

# Publish the Application layer
WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.Application"
RUN dotnet build "AppyNox.Services.Coupon.Application.csproj" -c Production -o /app/build

# Publish the WebAPI layer
WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.WebAPI"
RUN dotnet build "AppyNox.Services.Coupon.WebAPI.csproj" -c Production -o /app/build

FROM build AS publish
WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.Domain"
RUN dotnet publish "AppyNox.Services.Coupon.Domain.csproj" -c Production -o /app/publish

WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.Infrastructure"
RUN dotnet publish "AppyNox.Services.Coupon.Infrastructure.csproj" -c Production -o /app/publish

WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.Application"
RUN dotnet publish "AppyNox.Services.Coupon.Application.csproj" -c Production -o /app/publish

WORKDIR "/src/Services/CouponService/AppyNox.Services.Coupon.WebAPI"
RUN dotnet publish "AppyNox.Services.Coupon.WebAPI.csproj" -c Production -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Copy the published layers
COPY --from=publish /app/publish .

EXPOSE 7001

# Specify the entry point
ENTRYPOINT ["dotnet", "AppyNox.Services.Coupon.WebAPI.dll"]
