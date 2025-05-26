Build Docker Image: docker build -t bookapi .
Run Docker Container: docker run -d -p 5000:80 --name bookapi-container bookapi
Stop and Clean Up:
docker stop bookapi-container
docker rm bookapi-container
docker rmi bookapi
docker ps
docker start bookapi-container
docker logs bookapi-container

With Yml: docker-compose up --build

Rebuild Docker Image (If you changed code):
docker stop bookapi-container
docker rm bookapi-container
docker build -t bookapi .
docker run -d -p 5000:80 --name bookapi-container bookapi

Find and kill port process:
sudo lsof -i tcp:5000
sudo kill -9 PID


## How to run

```bash
# local
dotnet run --project src/BookApi.csproj --urls "http://localhost"
dotnet test tests/BookApi.Tests.Playwright

# docker / CI
docker compose up --build --abort-on-container-exit

