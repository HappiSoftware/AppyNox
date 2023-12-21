#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7002

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Production
WORKDIR /src
COPY ["src/Services/CouponService/AppyNox.Services.Coupon.WebAPI/AppyNox.Services.Coupon.WebAPI.csproj", "src/Services/CouponService/AppyNox.Services.Coupon.WebAPI/"]
COPY ["src/Services/CouponService/AppyNox.Services.Coupon.Application/AppyNox.Services.Coupon.Application.csproj", "src/Services/CouponService/AppyNox.Services.Coupon.Application/"]
COPY ["src/Services/CouponService/AppyNox.Services.Coupon.Domain/AppyNox.Services.Coupon.Domain.csproj", "src/Services/CouponService/AppyNox.Services.Coupon.Domain/"]
COPY ["src/Services/CouponService/AppyNox.Services.Coupon.Infrastructure/AppyNox.Services.Coupon.Infrastructure.csproj", "src/Services/CouponService/AppyNox.Services.Coupon.Infrastructure/"]
RUN dotnet restore "src/Services/CouponService/AppyNox.Services.Coupon.WebAPI/AppyNox.Services.Coupon.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/CouponService/AppyNox.Services.Coupon.WebAPI"
RUN dotnet build "AppyNox.Services.Coupon.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "AppyNox.Services.Coupon.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppyNox.Services.Coupon.WebAPI.dll"]