#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7001

USER root
# Add the wait script to the image
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.12.1/wait /app/wait
RUN chmod +x /app/wait

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Production
WORKDIR /src
COPY ["src/Services/SsoService/AppyNox.Services.Sso.WebAPI/AppyNox.Services.Sso.WebAPI.csproj", "src/Services/SsoService/AppyNox.Services.Sso.WebAPI/"]
COPY ["src/Services/SsoService/AppyNox.Services.Sso.Application/AppyNox.Services.Sso.Application.csproj", "src/Services/SsoService/AppyNox.Services.Sso.Application/"]
COPY ["src/Services/SsoService/AppyNox.Services.Sso.Infrastructure/AppyNox.Services.Sso.Infrastructure.csproj", "src/Services/SsoService/AppyNox.Services.Sso.Infrastructure/"]
RUN dotnet restore "src/Services/SsoService/AppyNox.Services.Sso.WebAPI/AppyNox.Services.Sso.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/Services/SsoService/AppyNox.Services.Sso.WebAPI"
RUN dotnet build "AppyNox.Services.Sso.WebAPI.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
RUN dotnet publish "AppyNox.Services.Sso.WebAPI.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT /app/wait && dotnet AppyNox.Services.Sso.WebAPI.dll
