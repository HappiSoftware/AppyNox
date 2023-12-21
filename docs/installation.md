# Installation

<br>

**Prerequisites**

Before you proceed with the installation, make sure you have the following prerequisites installed on your system:

- [Docker Desktop](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.100-windows-x64-installer)

1. **Connect Github via SSH** <br>
   Connect your local computer to github via ssh:

```
Check Existing SSH keys (OR) Create a new key

    ->Create a new key
        Open Git Bash
        ssh-keygen -t ed25519 -C "your_email@example.com"
        Enter a file in which to save the key (/c/Users/YOU/.ssh/id_ALGORITHM):[Press enter]
        Enter passphrase (empty for no passphrase): [Type a passphrase]
        Enter same passphrase again: [Type passphrase again]

    ->Check existing keys
        Open Git Bash
        ls -al ~/.ssh
        Check the directory listing to see if you already have a public SSH key. By default, the filenames of supported public keys for GitHub are one of the following.
            id_rsa.pub
            id_ecdsa.pub
            id_ed25519.pub

    ->Adding a new SSH key to your account
        Open Git Bash
        clip < ~/.ssh/id_ed25519.pub (If this not works, use this: cat ~/.ssh/id_ed25519.pub | clip)
        Go to github on your browser
        Click Profil photo > Settings
        In the "Access" section of the sidebar, click  SSH and GPG keys.
        Click New SSH key or Add SSH key.
        In the "Title" field, add a descriptive label for the new key. For example, if you're using a personal laptop, you might call this key "Personal laptop".
        type of key: First, select authentication and then do it for signing again
        In the "Key" field, paste your public key.
        Click Add SSH key.
```

<br>

2. **SSL Certificates** <br>
   Please follow to this page and create ssl files in order to run it correctly: [SSL Certificates](certificate.md)
   <br>

<br>

3. **AppSettings Files** <br>
   Please follow to this page and configure the project in order to run it correctly: [Appsettings Configurations](appsettings.md)
   <br>

4. **Services you need to run on Docker container:**

```
    RabbitMQ
        - docker pull rabbitmq
        - docker run -d --name rabbitmq.service -p 5672:5672 -p 15672:15672 rabbitmq:management
        - Open your browser and go to http://localhost:15672. The username will be guest and the password will be guest (default values).

    Consul
        - docker pull hashicorp/consul:latest
        - docker run -d --name=consul.service -p 8500:8500 hashicorp/consul agent -dev -client=0.0.0.0 -bind=0.0.0.0
        - Go to http://localhost:8500
        - Finally, you need to add the following block to the appsettings file of the microservice that will register to the consul service:
            "ConsulConfig": {
              "Address": "http://localhost:8500"
            },
            "Consul": {
              "ServiceId": "{}",
              "ServiceName": "{}",
              "Scheme": "{http or https}",
              "ServiceHost": "{}",
              "ServicePort": {PORT},
              "Tags": [ "Tag1", "Tag2" ],
              "HealthCheckUrl": "{health-check-url}",
              "HealthCheckIntervalSeconds": 30,
              "HealthCheckTimeoutSeconds": 5
            }
```

<br>

<br>

5. **Prepare docker-compose**

Create `docker-compose.override.yml` and copy the content of `docker-compose-development.yml`. Since `docker-compose.override.yml` is **gitignored** you can also change the configurations gitignored.

<br>

6. **Launch AppyNox**

With everything set up, select Docker as the startup project and run the solution. This will launch the AppyNox services in Docker containers. If you want to use DockerSwarm please navigate to [DockerSwarm](dockerswarm.md).
<br>
<br>

**These steps ensure a smooth setup for AppyNox services. Adjust the configurations based on your specific requirements.**
