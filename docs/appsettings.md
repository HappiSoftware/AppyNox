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
    "StagingConnection": "User ID=postgres;Password=coupon_password;Server=coupon.db.test;Port=5432;Database=AppyNox_Coupon_Test",
    "ProductionConnection": "User ID=postgres;Password=coupon_password;Server=coupon.db;Port=5432;Database=AppyNox_Coupon",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "TestConnection": "User ID=postgres;Password=coupon_password;Server=localhost;Port=5434;Database=AppyNox_Coupon_Test"
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
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "StagingConnection": "User ID=postgres;Password=coupon_password;Server=coupon.db.test;Port=5432;Database=AppyNox_Coupon_Test",
    "ProductionConnection": "User ID=postgres;Password=coupon_password;Server=coupon.db;Port=5432;Database=AppyNox_Coupon",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "TestConnection": "User ID=postgres;Password=coupon_password;Server=localhost;Port=5434;Database=AppyNox_Coupon_Test"
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://consul.service:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "https",
    "ServiceHost": "coupon.service",
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
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "StagingConnection": "User ID=postgres;Password=coupon_password;Server=coupon.db.test;Port=5432;Database=AppyNox_Coupon_Test",
    "ProductionConnection": "User ID=postgres;Password=coupon_password;Server=coupon.db;Port=5432;Database=AppyNox_Coupon",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Coupon;Pooling=true",
    "TestConnection": "User ID=postgres;Password=coupon_password;Server=localhost;Port=5434;Database=AppyNox_Coupon_Test"
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://consul.service:8500"
  },
  "Consul": {
    "ServiceId": "CouponService",
    "ServiceName": "CouponService",
    "Scheme": "https",
    "ServiceHost": "coupon.service",
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
    "StagingConnection": "User ID=postgres;Password=auth_password;Server=authentication.db.test;Port=5432;Database=AppyNox_Authentication_Test",
    "ProductionConnection": "User ID=postgres;Password=auth_password;Server=authentication.db;Port=5432;Database=AppyNox_Authentication",
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
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "StagingConnection": "User ID=postgres;Password=auth_password;Server=authentication.db.test;Port=5432;Database=AppyNox_Authentication_Test",
    "ProductionConnection": "User ID=postgres;Password=auth_password;Server=authentication.db;Port=5432;Database=AppyNox_Authentication",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "TestConnection": "" // for integration tests, use this to connect to dockerized database container from localhost
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://consul.service:8500"
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
    "DevelopmentConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "StagingConnection": "User ID=postgres;Password=auth_password;Server=authentication.db.test;Port=5432;Database=AppyNox_Authentication_Test",
    "ProductionConnection": "User ID=postgres;Password=auth_password;Server=authentication.db;Port=5432;Database=AppyNox_Authentication",
    "DefaultConnection": "User ID=postgres;Password=sapass;Server=localhost;Port=5432;Database=AppyNox_Authentication",
    "TestConnection": "" // for integration tests, use this to connect to dockerized database container from localhost
  },
  "JwtSettings": {
    "SecretKey": "vA+A/of8yadsbwe/CmS6PD0Kp837BozrQFMDuQ2Kwwg=",
    "Issuer": "AuthServerV1",
    "Audience": "AppyNoxBasic"
  },
  "ConsulConfig": {
    "Address": "http://consul.service:8500"
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

</details>

<!-- Below is Gateway Service -->
<br>

<details>
    <summary>Gateway Service</summary>

<details>
    <summary>ocelot.Development.json Example</summary>

```json
// TODO Yasin
```

</details>

<details>
    <summary>ocelot.Staging.json Example</summary>

```json
// TODO Yasin
```

</details>

<details>
    <summary>ocelot.Production.json Example</summary>

```json
// TODO Yasin
```

</details>

</details>
