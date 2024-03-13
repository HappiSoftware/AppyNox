# Docker Swarm

1. **Build The Images**

```bash
docker-compose -f docker-compose.yml -f docker-compose.Development.yml build
```

2. **Initialize Docker Swarm**

```bash
docker swarm init
```

3. **Deploy Services**

```bash
docker stack deploy -c docker-compose.yml -c docker-compose.Production.yml AppyNox
```

<br>

**Extras**

- `Stop Docker Swarm`

```bash
docker stack rm AppyNox
```

<br>

- `Scaling and Load Balancing`

```bash
docker service scale <service_name>=<replica_count>
```

<br>

- `Sso Only Startup`

If the project you are working on is using Nox Sso, you can simply just run the necessary services via:
```bash
docker stack deploy -c docker-compose.Sso.yml AppyNox_Sso
```
