# Installation

<br>

**Prerequisites** <br>
Before you proceed with the installation, make sure you have the following prerequisites installed on your system:

- [Docker Desktop](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.100-windows-x64-installer)

### 1. Installation

**Clone the repository:**
`https://github.com/HappiSoftware/AppyNox.git`

### 2. SSL Certificates

**Create and Update SSL Certificates:**
Please follow to this page and create ssl files in order to run it correctly: [SSL Certificates](certificate.md)


### 3. AppSettings/Ocelot Files

**Create and Update Setting Files:** 
Please follow to this page and configure the project in order to run it correctly: [Appsettings Configurations](appsettings.md)


### 4. Redis

**Create Redis Credentials:**
Please follow to this page and configure the project in order to run it correctly: [Redis](redis.md)


### 5. Launch AppyNox

**Run the Project:** 
With everything set up, select Docker as the startup project and run the solution. This will launch the AppyNox services in Docker containers. If you want to use DockerSwarm please navigate to [DockerSwarm](dockerswarm.md).


**These steps ensure a smooth setup for AppyNox services. Adjust the configurations based on your specific requirements.**