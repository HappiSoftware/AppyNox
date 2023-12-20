#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7001

# Copy and update SSL certificates
USER root
COPY src/Services/AuthenticationService/AppyNox.Services.Authentication.WebAPI/ssl/*.pfx /https/
COPY src/Services/AuthenticationService/AppyNox.Services.Authentication.WebAPI/ssl/crt/*.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Production
WORKDIR /src
COPY ["src/Services/AuthenticationService/AppyNox.Services.Authentication.WebAPI/AppyNox.Services.Authentication.WebAPI.csproj", "src/Services/AuthenticationService/AppyNox.Services.Authentication.WebAPI/"]
COPY ["src/Services/AuthenticationService/AppyNox.Services.Authentication.Application/AppyNox.Services.Authentication.Application.csproj", "src/Services/AuthenticationService/AppyNox.Services.Authentication.Application/"]
COPY ["src/Services/AuthenticationService/AppyNox.Services.Authentication.Infrastructure/AppyNox.Services.Authentication.Infrastructure.csproj", "src/Services/AuthenticationService/AppyNox.Services.Authentication.Infrastructure/"]
RUN dotnet restore "src/Services/AuthenticationService/AppyNox.Services.Authentication.WebAPI/AppyNox.Services.Authentication.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/AuthenticationService/AppyNox.Services.Authentication.WebAPI"
RUN dotnet build "AppyNox.Services.Authentication.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "AppyNox.Services.Authentication.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppyNox.Services.Authentication.WebAPI.dll"]