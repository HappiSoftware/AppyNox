# Appsettings Configurations

First of all it's good the understand the project structure. AppyNox uses all the **functions of stages of development** `(Development, Staging, Production)`.

Since appsetting files are gitignored, you must create the `appsettings.{Environment}.json` files manually. You can either copy the content of `appsettings.json` to the created json files and fill the content or copy the contents from below. Take note that examples are below is configurations of a developer from **Neon Ninjas** so it might not be **compatible** with you.

<details>
    <summary>Coupon Service</summary>

<details>
    <summary>appsettings.Development.json Example</summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true"
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://localhost:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "https",
    "ServiceHost": "localhost",
    "ServicePort": "7002",
    "Tags": ["Coupon", "Coupons"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  }
}
```

</details>

<details>
    <summary>appsettings.Staging.json Example</summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "StagingConnection": "User ID=postgres;Password=coupon_password;Server=appynox-coupon-db-test;Port=5432;Database=AppyNox_Coupon_Test",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "TestConnection": "User ID=postgres;Password=coupon_password;Server=localhost;Port=5434;Database=AppyNox_Coupon_Test"
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "https",
    "ServiceHost": "appynox-services-coupon-webapi",
    "ServicePort": "7002",
    "Tags": ["Coupon", "Coupons"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  }
}
```

</details>

<details>
    <summary>appsettings.Production.json Example</summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "ProductionConnection": "User ID=postgres;Password=coupon_password;Server=appynox-coupon-db;Port=5432;Database=AppyNox_Coupon",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true"
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "https",
    "ServiceHost": "appynox-services-coupon-webapi",
    "ServicePort": "7002",
    "Tags": ["Coupon", "Coupons"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  }
}
```

</details>

</details>

<!-- Below is Authentication Service -->
<br>

<details>
    <summary>Authentication Service</summary>

<details>
    <summary>appsettings.Development.json Example</summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "TestConnection": "" // for integration tests, use this to connect to dockerized database container from localhost
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://localhost:8500"
  },
  "Consul": {
    "ServiceId": "AuthenticationService",
    "ServiceName": "AuthenticationService",
    "Scheme": "https",
    "ServiceHost": "localhost",
    "ServicePort": "7001",
    "Tags": ["Authentication", "SSO"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  }
}
```

</details>

<details>
    <summary>appsettings.Staging.json Example</summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "StagingConnection": "User ID=postgres;Password=auth_password;Server=appynox-authentication-db-test;Port=5432;Database=AppyNox_Authentication_Test",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "TestConnection": "" // for integration tests, use this to connect to dockerized database container from localhost
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "AuthenticationService",
    "ServiceName": "AuthenticationService",
    "Scheme": "https",
    "ServiceHost": "appynox-services-authentication-webapi",
    "ServicePort": "7001",
    "Tags": ["Authentication", "SSO"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  }
}
```

</details>

<details>
    <summary>appsettings.Production.json Example</summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "ProductionConnection": "User ID=postgres;Password=auth_password;Server=appynox-authentication-db;Port=5432;Database=AppyNox_Authentication",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "TestConnection": "" // for integration tests, use this to connect to dockerized database container from localhost
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "AuthenticationService",
    "ServiceName": "AuthenticationService",
    "Scheme": "https",
    "ServiceHost": "appynox-services-authentication-webapi",
    "ServicePort": "7001",
    "Tags": ["Authentication", "SSO"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  }
}
```

</details>

</details>

<!-- Below is Gateway Service -->
<br>

<details>
    <summary>Gateway Service</summary>

<details>
    <summary>ocelot.Development.json Example</summary>

```json
{
  "GlobalConfiguration": {
    "BaseUrl": "https://localhost:7000",
    "ServiceDiscoveryProvider": {
      "Host": "localhost",
      "Port": 8500,
      "Type": "Consul"
    },
    "RateLimitOptions": {
      "QuotaExceededMessage": "You have sent too many requests in a row.",
      "ClientIdHeader": "ClientId"
    }
  },
  "Routes": [
    {
      // Special route for /health to bypass Ocelot
      "DownstreamPathTemplate": "/health",
      "UpstreamPathTemplate": "/health",
      "UpstreamHttpMethod": ["Get"],
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 7000
        }
      ],
      "Priority": 1 // High priority to ensure it takes precedence
    },
    {
      "UseServiceDiscovery": true,
      "ServiceName": "IdentityService",

      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "http",

      "UpstreamPathTemplate": "/auth/{everything}",
      "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
      "UpstreamScheme": "https",

      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "3s", // Depends on service needs
        "PeriodTimespan": 3, // Depends on service needs
        "Limit": 3 // Depends on service needs
      }
    },
    {
      "UseServiceDiscovery": true,
      "ServiceName": "CouponService",

      "DownstreamPathTemplate": "/api/{everything}",
      "DownstreamScheme": "http",

      "UpstreamPathTemplate": "/coupon-service/{everything}",
      "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
      "UpstreamScheme": "https",

      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "3s", // Depends on service needs
        "PeriodTimespan": 3, // Depends on service needs
        "Limit": 5 // Depends on service needs
      }
    }
  ]
}
```

</details>

<details>
    <summary>ocelot.Staging.json Example</summary>

```json
{
  "GlobalConfiguration": {
    "BaseUrl": "https://appynox-gateway-ocelotgateway:7000",
    "ServiceDiscoveryProvider": {
      "Host": "appynox-consul",
      "Port": 8500,
      "Type": "Consul"
    },
    "RateLimitOptions": {
      "QuotaExceededMessage": "You have sent too many requests in a row.",
      "ClientIdHeader": "ClientId"
    }
  },
  "Routes": [
    {
      // Special route for /health to bypass Ocelot
      "DownstreamPathTemplate": "/health",
      "UpstreamPathTemplate": "/health",
      "UpstreamHttpMethod": ["Get"],
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 7000
        }
      ],
      "Priority": 1 // High priority to ensure it takes precedence
    },
    {
      "UseServiceDiscovery": true,
      "ServiceName": "IdentityService",

      "DownstreamPathTemplate": "/{everything}",
      "DownstreamScheme": "http",

      "UpstreamPathTemplate": "/auth/{everything}",
      "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
      "UpstreamScheme": "https",

      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "3s", // Depends on service needs
        "PeriodTimespan": 3, // Depends on service needs
        "Limit": 3 // Depends on service needs
      }
    },
    {
      "UseServiceDiscovery": true,
      "ServiceName": "CouponService",

      "DownstreamPathTemplate": "/api/{everything}",
      "DownstreamScheme": "http",

      "UpstreamPathTemplate": "/coupon-service/{everything}",
      "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
      "UpstreamScheme": "https",

      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "3s", // Depends on service needs
        "PeriodTimespan": 3, // Depends on service needs
        "Limit": 5 // Depends on service needs
      }
    }
  ]
}
```

</details>

<details>
    <summary>ocelot.Production.json Example</summary>

```json
{
  "GlobalConfiguration": {
    "BaseUrl": "https://appynox-gateway-ocelotgateway:7000",
    "ServiceDiscoveryProvider": {
      "Host": "appynox-consul",
      "Port": 8500,
      "Type": "Consul"
    },
    "RateLimitOptions": {
      "QuotaExceededMessage": "You have sent too many requests in a row.",
      "ClientIdHeader": "ClientId"
    }
  },
  "Routes": [
    {
      // Special route for /health to bypass Ocelot
      "DownstreamPathTemplate": "/health",
      "UpstreamPathTemplate": "/health",
      "UpstreamHttpMethod": ["Get"],
      "DownstreamScheme": "https",
      "DownstreamHostAndPorts": [
        {
          "Host": "localhost",
          "Port": 7000
        }
      ],
      "Priority": 1 // High priority to ensure it takes precedence
    },
    {
      "UseServiceDiscovery": true,
      "ServiceName": "IdentityService",

      "DownstreamPathTemplate": "/api/{everything}",
      "DownstreamScheme": "http",

      "UpstreamPathTemplate": "/auth/{everything}",
      "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
      "UpstreamScheme": "https",

      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "3s", // Depends on service needs
        "PeriodTimespan": 3, // Depends on service needs
        "Limit": 3 // Depends on service needs
      }
    },
    {
      "UseServiceDiscovery": true,
      "ServiceName": "CouponService",

      "DownstreamPathTemplate": "/api/{everything}",
      "DownstreamScheme": "http",

      "UpstreamPathTemplate": "/coupon-service/{everything}",
      "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
      "UpstreamScheme": "https",

      "RateLimitOptions": {
        "ClientWhitelist": [],
        "EnableRateLimiting": true,
        "Period": "3s", // Depends on service needs
        "PeriodTimespan": 3, // Depends on service needs
        "Limit": 5 // Depends on service needs
      }
    }
  ]
}
```

</details>

</details>

# Read Also

- [GitHub appsetting Secrets](github.md#add-appsetting-secrets-to-github-for-workflow)
