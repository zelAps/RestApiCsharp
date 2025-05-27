set -e

until curl -sf http://api:8080/health > /dev/null; do
  sleep 1
done

cd /app/tests
dotnet test \
  --logger "trx;LogFileName=testresults.trx" \
  --results-directory /app/test-results

[ -d TestResults/playwright-report ] && \
  cp -r TestResults/playwright-report /app/test-results
