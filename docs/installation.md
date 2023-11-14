# Installation

1) **SSL Certificates** <br> 
After cloning the repository, add development SSL certificates to the services for running them in a Docker container. Run the following commands to generate and trust the SSL certificates:
```
dotnet dev-certs https -ep .\src\Services\CouponService\AppyNox.Services.Coupon.WebAPI\ssl\coupon-service.pfx -p happi2023
dotnet dev-certs https -ep .\src\Services\AuthenticationService\AppyNox.Services.Authentication.WebAPI\ssl\authentication-service.pfx -p happi2023
dotnet dev-certs https --trust
```
<br>

2) **docker-compose.override.yml** <br>
Create a docker-compose.override.yml file (gitignored) to contain connection strings. Fill it with the appropriate content for each service.
**Example Content:**
```
version: '3.4'
services:
  appynox.services.authentication.webapi:
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_HTTPS_PORTS=7001
      - ConnectionStrings__DefaultConnection=User ID={your db user};Password={your db password};Server=authentication.db;Port=5432;Database=AppyNox_Authentication;IntegratedSecurity=true;Pooling=true
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/authentication-service.pfx 
      - ASPNETCORE_Kestrel__Certificates__Default__Password={your certificate password}
    ports:
      - "7001:7001"
    volumes:
      - ${APPDATA}/Microsoft/UserSecrets:/home/app/.microsoft/usersecrets:ro
      - ${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro

  appynox.services.coupon.webapi:
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_HTTPS_PORTS=7002
      - ConnectionStrings__DefaultConnection=User ID={your db user};Password={your db password};Server=coupon.db;Port=5432;Database=AppyNox_Coupon;IntegratedSecurity=true;Pooling=true
      - ASPNETCORE_Kestrel__Certificates__Default__Path=/https/coupon-service.pfx 
      - ASPNETCORE_Kestrel__Certificates__Default__Password={your certificate password}
    ports:
      - "7002:7002"
    volumes:
      - ${APPDATA}/Microsoft/UserSecrets:/home/app/.microsoft/usersecrets:ro
      - ${APPDATA}/ASP.NET/Https:/home/app/.aspnet/https:ro
```
<br>
<br>

If your docker-compose.override.yml didn't go under docker-compose.yml and located in the same level you can check the following in docker-compose settings:
```
<ItemGroup>
  <None Include="docker-compose.override.yml">
    <DependentUpon>docker-compose.yml</DependentUpon>
  </None>
  <None Include="docker-compose.yml" />
  <None Include=".dockerignore" />
</ItemGroup>
```
<br>

![docker-compose-override](_media/docker-compose.override.png)

<br>

3) **AppSettings Files** <br>
Create appsettings.Production.json files for both services. Customize the content with your database and JWT settings.

CouponService appsettings.Production.json :
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "User ID={your db user};Password={your db password};Server=coupon.db;Port=5432;Database=AppyNox_Coupon;IntegratedSecurity=true;Pooling=true"
  }
}
```
<br>

AuthenticationService appsettings.Production.json:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "User ID=postgres;Password=auth_password;Server=authentication.db;Port=5432;Database=AppyNox_Authentication;IntegratedSecurity=true;Pooling=true"
  },
  "JwtSettings": {
    "SecretKey": "{your secret key}",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  }
}
```
<br>

4) With everything set up, select Docker as the startup project and run the solution. This will launch the AppyNox services in Docker containers.
<br>
<br>

**These steps ensure a smooth setup for AppyNox services. Adjust the configurations based on your specific requirements.**