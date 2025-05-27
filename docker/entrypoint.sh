set -e

dotnet ./BookApi.dll &
API_PID=$!

echo "Waiting for API to start..."
until curl -sf http://localhost:8080/health >/dev/null; do
  sleep 1
done
echo "API is healthy!"

wait $API_PID
