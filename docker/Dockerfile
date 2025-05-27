# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/BookApi.csproj ./src/
COPY tests/BookApi.Tests.Playwright/BookApi.Tests.Playwright.csproj ./tests/BookApi.Tests.Playwright/
RUN dotnet restore src/BookApi.csproj \
 && dotnet restore tests/BookApi.Tests.Playwright/BookApi.Tests.Playwright.csproj

COPY . .
RUN dotnet publish src/BookApi.csproj -c Release -o /out/api

# --- Runtime stage -------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update \
 && apt-get install -y --no-install-recommends curl \
 && rm -rf /var/lib/apt/lists/*
# ---------------------------------------------

COPY --from=build /out/api .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=false \
    ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_HTTPS_PORTS=

ENTRYPOINT ["dotnet","BookApi.dll"]

# ---------- TESTS ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS tests
WORKDIR /app

# ---- system + Playwright deps ----
RUN apt-get update \
 && apt-get install -y --no-install-recommends \
      curl ca-certificates gnupg libgbm1 libnss3 libgtk-3-0 \
 && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
 && apt-get install -y nodejs \
 && npm i -g playwright@1.52.0 \
 && npx playwright install-deps \
 && npx playwright install chromium \
 && rm -rf /var/lib/apt/lists/*

# ---- test project ----
COPY --from=build /src/tests/BookApi.Tests.Playwright /app/tests
WORKDIR /app/tests
RUN dotnet restore && dotnet build -c Release

# ---- runner ----
WORKDIR /app
COPY docker/run-tests.sh .
RUN chmod +x run-tests.sh

VOLUME ["/app/test-results"]
ENTRYPOINT ["./run-tests.sh"]
