#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 443

# Install necessary tools
USER root
RUN apt-get update && apt-get install -y \
    ca-certificates \
    openssl

# Copy the pfx file into the container and trust it
COPY ["ssl/appynox.pfx", "/usr/local/share/ca-certificates/appynox.pfx"]
RUN openssl pkcs12 -in /usr/local/share/ca-certificates/appynox.pfx -clcerts -nokeys -out /usr/local/share/ca-certificates/appynox.crt -passin pass:happi2023 \
    && cp /usr/local/share/ca-certificates/appynox.crt /usr/local/share/ca-certificates/ \
    && update-ca-certificates

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Production
WORKDIR /src
COPY ["src/Gateways/AppyNox.Gateway.OcelotGateway/AppyNox.Gateway.OcelotGateway.csproj", "src/Gateways/AppyNox.Gateway.OcelotGateway/"]
RUN dotnet restore "./src/Gateways/AppyNox.Gateway.OcelotGateway/./AppyNox.Gateway.OcelotGateway.csproj"
COPY . .
WORKDIR "/src/src/Gateways/AppyNox.Gateway.OcelotGateway"
RUN dotnet build "./AppyNox.Gateway.OcelotGateway.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
RUN dotnet publish "./AppyNox.Gateway.OcelotGateway.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppyNox.Gateway.OcelotGateway.dll"]