# Docker Management for AppyNox

## Building Docker Images

To build all necessary Docker images with Docker Compose, use the following command:

```bash
docker compose build
```

**Troubleshooting:** 
If you encounter errors such as `failed to receive status: rpc error: code = Unavailable desc = error reading from server: EOF`, try building the images individually. For example:

```bash
docker compose build appynox-services-sso-webapi
```

To run the project locally you can use docker compose.

```bash
docker compose up
```

<br>
<br>

**Running specified Services Only** <br>
To run the specific services only such as Sso or Coupon you can use the `docker-compose.Development.yml`. This yml will only launch the essantial containers such as rabbitmq and consul etc.
After running these containers you can run the desired services on Visual Studio.

```bash
docker compose -f docker-compose.Development.yml up
```

**Important** <br>
When running services in Visual Studio, they operate on your local machine, while Consul is hosted within a Docker network. To enable Consul to effectively communicate with 
these services for health monitoring, a simple `localhost` reference is insufficient. Therefore, the service URL should be modified to utilize `host.docker.internal`, 
which allows seamless connectivity between the Docker network and the locally running services. You can find the mentioned reference in `appsettings.Development.json:Consul:HealthCheckServiceHost`.

<br>
<br>

## Initializing Docker Swarm

To initialize Docker Swarm mode on your machine, run:

```bash
docker swarm init
```

This will turn your Docker Engine into a Swarm manager node.

<br>
<br>

## Deploying Services

Deploy your services using the following command:

```bash
docker stack deploy AppyNox
```

<br>
<br>

## Additional Commands and Tips

### Stopping Docker Swarm

To remove a deployed stack from Docker Swarm:

```bash
docker stack rm AppyNox
```

### Scaling and Load Balancing

To scale a service to a specific number of replicas:

```bash
docker service scale <service_name>=<replica_count>
```

### Inspecting Swarm Nodes

List services, check service status, view logs, and inspect nodes:

```bash
docker service ls
docker service ps [NodeId]
docker service logs [ContainerId]
docker service inspect --pretty [NodeId]
```