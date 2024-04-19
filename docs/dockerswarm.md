# Docker Swarm Management for AppyNox

## Building Docker Images

To build all necessary Docker images with Docker Compose, use the following command:

```bash
docker-compose -f docker-compose.yml -f docker-compose.Development.yml build
```

**Troubleshooting:** 
If you encounter errors such as `failed to receive status: rpc error: code = Unavailable desc = error reading from server: EOF`, try building the images individually. For example:

```bash
docker-compose -f docker-compose.yml -f docker-compose.Development.yml build appynox-services-sso-webapi
```

## Initializing Docker Swarm

To initialize Docker Swarm mode on your machine, run:

```bash
docker swarm init
```

This will turn your Docker Engine into a Swarm manager node.

## Deploying Services

Deploy your services using the following command:

```bash
docker stack deploy -c docker-compose.yml -c docker-compose.Production.yml AppyNox
```

**Note:** 
If API services do not initialize correctly in the swarm, you may need to troubleshoot by first running the containers with Docker Compose:

```bash
docker-compose -f docker-compose.yml -f docker-compose.Development.yml up
docker-compose -f docker-compose.yml -f docker-compose.Development.yml down
```

Afterward, try redeploying the stack again.

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

### Sso Only Startup

If the project you are working on is using Nox Sso, you can simply run the necessary services via:

```bash
docker stack deploy -c docker-compose.Sso.yml AppyNox_Sso
```

### Inspecting Swarm Nodes

List services, check service status, view logs, and inspect nodes:

```bash
docker service ls
docker service ps [NodeId]
docker service logs [ContainerId]
docker service inspect --pretty [NodeId]
```


This format should ensure all text is contained within the code block for easy copying.