# Appsettings Configurations

First of all it's good the understand the project structure. AppyNox uses all the **functions of stages of development** `(Development, Staging, Production)`.

Since appsettings files are gitignored, you must create the `appsettings.{Environment}.json` files manually. You can either copy the content of `appsettings.json` to the created json files and fill the content or copy the contents from below. Take note that examples are below is configurations of a developer from **Neon Ninjas** so it might not be **compatible** with you.

<details>
    <summary>Coupon Service</summary>

<details>
    <summary>appsettings.Development.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Debug"],
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "System": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "Debug",
        "Args": { "restrictedToMinimumLevel": "Debug" }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true"
  },
  "JwtSettings": {
    "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
    "Issuer": "NoxAuthServer",
    "Audience": "AppyNox"
  },
  "ConsulConfiguration": {
    "Address": "http://localhost:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "http",
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
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/staging-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "StagingConnection": "User ID=postgres;Password=coupon_password;Server=appynox-coupon-db;Port=5432;Database=AppyNox_Coupon_Test",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon",
    "TestConnection": "User ID=postgres;Password=coupon_password;Server=localhost;Port=5434;Database=AppyNox_Coupon_Test"
  },
  "JwtSettings": {
    "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
    "Issuer": "NoxAuthServer",
    "Audience": "AppyNox"
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "http",
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
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Microsoft": "Error",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/production-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "ProductionConnection": "User ID=postgres;Password=coupon_password;Server=appynox-coupon-db;Port=5432;Database=AppyNox_Coupon",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true"
  },
  "JwtSettings": {
    "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
    "Issuer": "NoxAuthServer",
    "Audience": "AppyNox"
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "http",
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

<!-- Below is Sso Service -->
<br>

<details>
    <summary>Sso Service</summary>

<details>
    <summary>appsettings.Development.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Debug"],
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "System": "Information"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "Debug",
        "Args": { "restrictedToMinimumLevel": "Debug" }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Sso",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Sso",
    "TestConnection": "",
    "SagaConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Sso_Saga"
  },
  "JwtSettings": {
    "AppyNox": {
      "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
      "Issuer": "NoxAuthServer",
      "Audience": "AppyNox",
      "TokenLifetimeMinutes": "5"
    },
    "AppyFleet": {
      "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
      "Issuer": "NoxAuthServer",
      "Audience": "AppyFleet",
      "TokenLifetimeMinutes": "5"
    }
  },
  "ConsulConfiguration": {
    "Address": "http://localhost:8500"
  },
  "Consul": {
    "ServiceId": "SsoService",
    "ServiceName": "SsoService",
    "Scheme": "http",
    "ServiceHost": "localhost",
    "ServicePort": "7001",
    "Tags": ["Sso", "SSO"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  },
  "MessageBroker": {
    "Host": "rabbitmq://localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

</details>

<details>
    <summary>appsettings.Staging.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/staging-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "StagingConnection": "User ID=postgres;Password=auth_password;Server=appynox-sso-db;Port=5432;Database=AppyNox_Sso_Test",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Sso",
    "TestConnection": "",
    "SagaConnection": "User ID=postgres;Password=auth_saga_password;Server=appynox-sso-saga-db;Port=5432;Database=AppyNox_Sso_Saga_Test"
  },
  "JwtSettings": {
    "AppyNox": {
      "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
      "Issuer": "NoxAuthServer",
      "Audience": "AppyNox",
      "TokenLifetimeMinutes": "5"
    },
    "AppyFleet": {
      "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
      "Issuer": "NoxAuthServer",
      "Audience": "AppyFleet",
      "TokenLifetimeMinutes": "5"
    }
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "SsoService",
    "ServiceName": "SsoService",
    "Scheme": "http",
    "ServiceHost": "appynox-services-sso-webapi",
    "ServicePort": "7001",
    "Tags": ["Sso", "SSO"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  },
  "MessageBroker": {
    "Host": "rabbitmq://appynox-rabbitmq-service",
    "Username": "HappiCorp",
    "Password": "HappiCorp"
  }
}
```

</details>

<details>
    <summary>appsettings.Production.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Microsoft": "Error",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/production-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "ProductionConnection": "User ID=postgres;Password=auth_password;Server=appynox-sso-db;Port=5432;Database=AppyNox_Sso",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Sso;Pooling=true",
    "SagaConnection": "User ID=postgres;Password=auth_saga_password;Server=appynox-sso-saga-db;Port=5432;Database=AppyNox_Sso_Saga"
  },
  "JwtSettings": {
    "AppyNox": {
      "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
      "Issuer": "NoxAuthServer",
      "Audience": "AppyNox",
      "TokenLifetimeMinutes": "5"
    },
    "AppyFleet": {
      "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
      "Issuer": "NoxAuthServer",
      "Audience": "AppyFleet",
      "TokenLifetimeMinutes": "5"
    }
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "SsoService",
    "ServiceName": "SsoService",
    "Scheme": "http",
    "ServiceHost": "appynox-services-sso-webapi",
    "ServicePort": "7001",
    "Tags": ["Sso", "SSO"],
    "HealthCheckUrl": "health-check",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  },
  "MessageBroker": {
    "Host": "rabbitmq://appynox-rabbitmq-service",
    "Username": "HappiCorp",
    "Password": "HappiCorp"
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
  "Gateway": {
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
      },
      "RequestIdKey": "X-Correlation-ID"
    },
    "Routes": [
      {
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
        "Priority": 1
      },
      {
        "UseServiceDiscovery": true,
        "ServiceName": "SsoService",

        "DownstreamPathTemplate": "/api/{everything}",
        "DownstreamScheme": "http",

        "UpstreamPathTemplate": "/sso-service/{everything}",
        "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
        "UpstreamScheme": "https",

        "RateLimitOptions": {
          "ClientWhitelist": [],
          "EnableRateLimiting": true,
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 3
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
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 5
        }
      },
      {
        "UseServiceDiscovery": true,
        "ServiceName": "LicenseService",

        "DownstreamPathTemplate": "/api/{everything}",
        "DownstreamScheme": "http",

        "UpstreamPathTemplate": "/license-service/{everything}",
        "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
        "UpstreamScheme": "https",

        "RateLimitOptions": {
          "ClientWhitelist": [],
          "EnableRateLimiting": true,
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 5
        }
      }
    ]
  }
}
```

</details>

<details>
    <summary>ocelot.Staging.json Example</summary>

```json
{
  "Gateway": {
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
      },
      "RequestIdKey": "X-Correlation-ID"
    },
    "Routes": [
      {
        "DownstreamPathTemplate": "/health",
        "UpstreamPathTemplate": "/health",
        "UpstreamHttpMethod": ["Get"],
        "DownstreamScheme": "https",
        "DownstreamHostAndPorts": [
          {
            "Host": "localhost",
            "Port": 443
          }
        ],
        "Priority": 1
      },
      {
        "UseServiceDiscovery": true,
        "ServiceName": "SsoService",

        "DownstreamPathTemplate": "/api/{everything}",
        "DownstreamScheme": "http",

        "UpstreamPathTemplate": "/sso-service/{everything}",
        "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
        "UpstreamScheme": "https",

        "RateLimitOptions": {
          "ClientWhitelist": [],
          "EnableRateLimiting": true,
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 3
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
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 5
        }
      },
      {
        "UseServiceDiscovery": true,
        "ServiceName": "LicenseService",

        "DownstreamPathTemplate": "/api/{everything}",
        "DownstreamScheme": "http",

        "UpstreamPathTemplate": "/license-service/{everything}",
        "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
        "UpstreamScheme": "https",

        "RateLimitOptions": {
          "ClientWhitelist": [],
          "EnableRateLimiting": true,
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 5
        }
      }
    ]
  }
}
```

</details>

<details>
    <summary>ocelot.Production.json Example</summary>

```json
{
  "Gateway": {
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
      },
      "RequestIdKey": "X-Correlation-ID"
    },
    "Routes": [
      {
        "DownstreamPathTemplate": "/health",
        "UpstreamPathTemplate": "/health",
        "UpstreamHttpMethod": ["Get"],
        "DownstreamScheme": "https",
        "DownstreamHostAndPorts": [
          {
            "Host": "localhost",
            "Port": 443
          }
        ],
        "Priority": 1
      },
      {
        "UseServiceDiscovery": true,
        "ServiceName": "SsoService",

        "DownstreamPathTemplate": "/api/{everything}",
        "DownstreamScheme": "http",

        "UpstreamPathTemplate": "/sso-service/{everything}",
        "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
        "UpstreamScheme": "https",

        "RateLimitOptions": {
          "ClientWhitelist": [],
          "EnableRateLimiting": true,
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 3
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
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 5
        }
      },
      {
        "UseServiceDiscovery": true,
        "ServiceName": "LicenseService",

        "DownstreamPathTemplate": "/api/{everything}",
        "DownstreamScheme": "http",

        "UpstreamPathTemplate": "/license-service/{everything}",
        "UpstreamHttpMethod": ["Get", "Post", "Delete", "Put"],
        "UpstreamScheme": "https",

        "RateLimitOptions": {
          "ClientWhitelist": [],
          "EnableRateLimiting": true,
          "Period": "3s",
          "PeriodTimespan": 3,
          "Limit": 5
        }
      }
    ]
  }
}
```

</details>

<br>

<details>
    <summary>appsettings.Development.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Debug"],
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "System": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "Debug",
        "Args": { "restrictedToMinimumLevel": "Debug" }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConsulConfiguration": {
    "Address": "http://localhost:8500"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000"]
  }
}
```

</details>

<details>
    <summary>appsettings.Staging.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/staging-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Cors": {
    "AllowedOrigins": []
  }
}
```

</details>

<details>
    <summary>appsettings.Production.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Microsoft": "Error",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/production-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Cors": {
    "AllowedOrigins": []
  }
}
```

</details>

</details>

<br>
<!--Below is License Service-->

<details>
    <summary>License Service</summary>

<details>
    <summary>appsettings.Development.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.Debug"],
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "System": "Information"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:dd-MM-yyyy HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "Debug",
        "Args": { "restrictedToMinimumLevel": "Debug" }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_License;Pooling=true",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_License;Pooling=true"
  },
  "JwtSettings": {
    "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
    "Issuer": "NoxAuthServer",
    "Audience": "AppyNox"
  },
  "ConsulConfiguration": {
    "Address": "http://localhost:8500"
  },
  "Consul": {
    "ServiceId": "LicenseService",
    "ServiceName": "LicenseService",
    "Scheme": "http",
    "ServiceHost": "localhost",
    "ServicePort": "7003",
    "Tags": ["License", "Licensing"],
    "HealthCheckUrl": "health",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  },
  "MessageBroker": {
    "Host": "rabbitmq://localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

</details>

<details>
    <summary>appsettings.Staging.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/staging-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "StagingConnection": "User ID=postgres;Password=license_password;Server=appynox-license-db;Port=5432;Database=AppyNox_License_Test",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_License_Test",
    "TestConnection": "User ID=postgres;Password=license_password;Server=localhost;Port=5436;Database=AppyNox_License_Test"
  },
  "JwtSettings": {
    "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
    "Issuer": "NoxAuthServer",
    "Audience": "AppyNox"
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "LicenseService",
    "ServiceName": "LicenseService",
    "Scheme": "http",
    "ServiceHost": "appynox-services-license-webapi",
    "ServicePort": "7003",
    "Tags": ["License", "Licensing"],
    "HealthCheckUrl": "health",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  },
  "MessageBroker": {
    "Host": "rabbitmq://appynox-rabbitmq-service",
    "Username": "HappiCorp",
    "Password": "HappiCorp"
  }
}
```

</details>

<details>
    <summary>appsettings.Production.json Example</summary>

```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Warning",
      "Override": {
        "Microsoft": "Error",
        "System": "Error"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/production-log-.txt",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": ["FromLogContext"]
  },
  "ConnectionStrings": {
    "ProductionConnection": "User ID=postgres;Password=license_password;Server=appynox-license-db;Port=5432;Database=AppyNox_License",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_License;Pooling=true"
  },
  "JwtSettings": {
    "SecretKey": "XPFk7n/yI+Sm9DtWlZ/6TYawZs22meQjENPNMmZ9ONA=",
    "Issuer": "NoxAuthServer",
    "Audience": "AppyNox"
  },
  "ConsulConfiguration": {
    "Address": "http://appynox-consul:8500"
  },
  "Consul": {
    "ServiceId": "LicenseService",
    "ServiceName": "LicenseService",
    "Scheme": "http",
    "ServiceHost": "appynox-services-license-webapi",
    "ServicePort": "7003",
    "Tags": ["License", "Licensing"],
    "HealthCheckUrl": "health",
    "HealthCheckIntervalSeconds": 30,
    "HealthCheckTimeoutSeconds": 5
  },
  "MessageBroker": {
    "Host": "rabbitmq://appynox-rabbitmq-service",
    "Username": "HappiCorp",
    "Password": "HappiCorp"
  }
}
```

</details>

</details>

<br>
<br>

# Read Also

- [GitHub appsetting Secrets](github.md#add-appsetting-secrets-to-github-for-workflow)
