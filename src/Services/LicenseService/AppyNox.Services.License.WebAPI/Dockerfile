#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7003

USER root
# Add the wait script to the image
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.12.1/wait /app/wait
RUN chmod +x /app/wait

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Production
WORKDIR /src
COPY ["src/Services/LicenseService/AppyNox.Services.License.WebAPI/AppyNox.Services.License.WebAPI.csproj", "src/Services/LicenseService/AppyNox.Services.License.WebAPI/"]
COPY ["src/Services/LicenseService/AppyNox.Services.License.Application/AppyNox.Services.License.Application.csproj", "src/Services/LicenseService/AppyNox.Services.License.Application/"]
COPY ["src/Services/LicenseService/AppyNox.Services.License.Domain/AppyNox.Services.License.Domain.csproj", "src/Services/LicenseService/AppyNox.Services.License.Domain/"]
COPY ["src/Services/LicenseService/AppyNox.Services.License.Infrastructure/AppyNox.Services.License.Infrastructure.csproj", "src/Services/LicenseService/AppyNox.Services.License.Infrastructure/"]
RUN dotnet restore "src/Services/LicenseService/AppyNox.Services.License.WebAPI/./AppyNox.Services.License.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/LicenseService/AppyNox.Services.License.WebAPI"
RUN dotnet build "AppyNox.Services.License.WebAPI.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
RUN dotnet publish "AppyNox.Services.License.WebAPI.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT /app/wait && dotnet AppyNox.Services.License.WebAPI.dll