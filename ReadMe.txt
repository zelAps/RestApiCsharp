# Book API

A minimal **.NET 8** REST service with Playwright‑powered black‑box tests and a ready‑to‑use Docker/GitLab CI pipeline.

---

## Requirements

| Tool                                                | Version  |
| --------------------------------------------------- | -------- |
| .NET SDK                                            | 8.0.x    |
| Docker Engine & Compose v2                          | ⩾ 24     |
| Node .js (optional, only for local Playwright runs) | 18 or 20 |

---

## 1. Quick start (everything inside Docker)

```bash
# builds API + test images, runs tests, prints results, then stops
docker compose --profile ci up --build --abort-on-container-exit
```

* 🗎 JUnit/Trx files – `./test-results/`

---

## 2. Local development workflow

```bash
# start the API on localhost:8080
dotnet run --project src/BookApi.csproj --urls "http://localhost:8080"

# run Playwright tests against that API
export API_URL=http://localhost:8080   # PowerShell:  $Env:API_URL="http://localhost:8080"
dotnet test tests/BookApi.Tests.Playwright
```

---

## 3. Project layout

```
📁 docker/              – multi‑stage Dockerfile + helper scripts
📁 src/                 – ASP.NET Core Web API
📁 tests/               – NUnit + Microsoft.Playwright tests
.gitlab-ci.yml         – one‑job pipeline (build → test → publish artefacts)
docker-compose.yml     – local/CI orchestration
```

---

## 4. Understanding *docker‑compose.yml*

| Service   | Image                 | Purpose                             | Exposed port | Health check      |
| --------- | --------------------- | ----------------------------------- | ------------ | ----------------- |
| **api**   | `restapicsharp-api`   | Publishes `BookApi.dll` (+ Swagger) | 8080         | `GET /health`     |
| **tests** | `restapicsharp-tests` | Launches Playwright + NUnit suite   | —            | waits for **api** |

Common commands:

```bash
# rebuild from scratch (ignore cache)
docker compose --profile ci build --no-cache

# stop & clean containers, images, volumes
docker compose down -v --remove-orphans
```

---

## 5. CI pipeline (GitLab)

1. Enable BuildKit → faster layer caching.
2. Build `api` + `tests` images.
3. Start compose with `ci` profile; suite must finish with exit‑code 0.
4. Archive `test-results/*.trx` (JUnit) and `playwright-report/` for review.

> Tip: add a shared **Docker cache** volume to speed up NuGet/NPM layers between jobs.

---

## 6. Troubleshooting

| Symptom                            |  Fix                                                                 |
| ---------------------------------- | -------------------------------------------------------------------- |
| `ECONNREFUSED :8080` in Playwright | API container not ready → check `docker compose ps` and health‑logs. |
| Port already in use                | `lsof -i :8080` → `kill -9 <PID>`                                    |
| Need to reset everything           | `docker system prune -af --volumes`                                  |
