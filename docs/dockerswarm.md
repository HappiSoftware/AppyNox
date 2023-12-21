# Docker Swarm

1. **Build The Images**

```bash
docker-compose -f docker-compose.yml -f docker-compose.development.yml build
```

2. **Initialize Docker Swarm**

```bash
docker swarm init
```

3. **Deploy Services**

```bash
docker stack deploy -c docker-compose.yml -c docker-compose.production.yml AppyNox
```

<br>

**Extras**

`Stop Docker Swarm`

```bash
docker stack rm AppyNox
```

`Scaling and Load Balancing`

```bash
docker service scale <service_name>=<replica_count>
```
