# User Secrets

## What are User Secrets?
User Secrets is a secure way to store sensitive data such as API keys, connection strings, and other configuration values during development. It allows developers to keep sensitive information out of source control and provides a mechanism for storing such data locally on a developer's machine.

## Setting Up User Secrets
1. Install the `Microsoft.Extensions.Configuration.UserSecrets` package in your project:

   ```bash
   dotnet add package Microsoft.Extensions.Configuration.UserSecrets
   ```

2. Right-click on your project in Visual Studio or use the following command in the terminal to initialize user secrets for your project:

   ```bash
   dotnet user-secrets init
   ```

3. Add your sensitive data to the user secrets store using the `dotnet user-secrets set` command:

   ```bash
   dotnet user-secrets set "MySecretKey" "MySecretValue"
   ```

## Accessing User Secrets in Code
1. Inject `IConfiguration` into your class:

   ```csharp
   using Microsoft.Extensions.Configuration;
   
   public class MyClass
   {
       private readonly IConfiguration _configuration;

       public MyClass(IConfiguration configuration)
       {
           _configuration = configuration;
       }

       public void MyMethod()
       {
           string secretValue = _configuration["MySecretKey"];
           // Use the secret value...
       }
   }
   ```

2. Retrieve the secret value using the configuration key.

## Updating, Listing, and Deleting User Secrets
- **Update**: To update an existing secret, use the `dotnet user-secrets set` command with the same key but a new value.
  
  ```bash
  dotnet user-secrets set "MySecretKey" "NewSecretValue"
  ```

- **List**: To list all the secrets stored in the user secrets store, use the `dotnet user-secrets list` command.

  ```bash
  dotnet user-secrets list
  ```

- **Delete**: To delete a secret from the user secrets store, use the `dotnet user-secrets remove` command with the key of the secret you want to delete.

  ```bash
  dotnet user-secrets remove "MySecretKey"
  ```
