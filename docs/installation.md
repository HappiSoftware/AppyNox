# Installation

<br>

**Prerequisites** <br>
Before you proceed with the installation, make sure you have the following prerequisites installed on your system:

- [Docker Desktop](https://www.docker.com/)
- [PostgreSQL](https://www.postgresql.org/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [.NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.100-windows-x64-installer)


1. **SSL Certificates** <br>
   Please follow to this page and create ssl files in order to run it correctly: [SSL Certificates](certificate.md)
   <br>

<br>

2. **AppSettings/Ocelot Files** <br>
   Please follow to this page and configure the project in order to run it correctly: [Appsettings Configurations](appsettings.md)
   <br>

3. **Redis** <br>
    Please follow to this page and configure the project in order to run it correctly: [Redis](redis.md)
    <br>

4. **Launch AppyNox**<br>
   With everything set up, select Docker as the startup project and run the solution. This will launch the AppyNox services in Docker containers. If you want to use DockerSwarm please navigate to [DockerSwarm](dockerswarm.md).
<br>
<br>

**These steps ensure a smooth setup for AppyNox services. Adjust the configurations based on your specific requirements.**

<br>
<br>

## Additional

If you want to run the project on Visual Studio without docker you can either use Visual Studio's Docker or you can add the below yml file and run this. After that you can just run the selected microservices frm Visual Studio. If the services you will be running has rabbitmq or other necessary containers don't forget to adjust the yml file below.

<details>
   <summary>Click here to expand</summary>

```yml
version: '3.8'

volumes:
  consul_data: 
  redis_data: 
  redisinsight_data: 

networks:
  development_network:
    driver: bridge

services:
  appynox-consul:
    image: "hashicorp/consul:latest"
    networks:
      - development_network
    ports:
      - "8500:8500"
      - "8600:8600/udp"

  appynox-redis:
    image: redis:latest
    volumes:
      - redis_data:/data
    networks:
      - development_network
    ports:
      - "6379:6379"

  appynox-redisinsight:
    image: redislabs/redisinsight
    volumes:
      - redisinsight_data:/db
    networks:
      - development_network
    ports:
      - "8001:8001"
```

</details>