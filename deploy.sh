#!/usr/bin/env bash
set -euo pipefail

APP_NAME="objectionary-app"
RESOURCE_GROUP="rg-objectionary-prod"

# Pre-flight checks
command -v az >/dev/null || { echo "Azure CLI not found"; exit 1; }
command -v dotnet >/dev/null || { echo ".NET SDK not found"; exit 1; }
command -v node >/dev/null || { echo "Node.js not found"; exit 1; }
command -v npm >/dev/null || { echo "npm not found"; exit 1; }
command -v python >/dev/null || { echo "python not found (needed for zip)"; exit 1; }
az account show >/dev/null 2>&1 || { echo "Not logged in to Azure CLI"; exit 1; }

# 1. Build frontend
echo "Building frontend..."
(cd client && npm install && npm run build)

# Verify frontend build produced output
[ -f src/Objectionary/wwwroot/index.html ] || { echo "Frontend build failed -- wwwroot/index.html not found"; exit 1; }

# 2. Publish .NET app
echo "Publishing .NET app..."
dotnet publish src/Objectionary/Objectionary.csproj -c Release -o ./publish

# 3. Zip the output
echo "Creating deployment package..."
python -c "
import zipfile, os
with zipfile.ZipFile('deploy.zip', 'w', zipfile.ZIP_DEFLATED) as zf:
    for root, dirs, files in os.walk('publish'):
        for f in files:
            full = os.path.join(root, f)
            arc = os.path.relpath(full, 'publish').replace(os.sep, '/')
            zf.write(full, arc)
"

# 4. Deploy to Azure
echo "Deploying to Azure..."
az webapp deploy --resource-group "$RESOURCE_GROUP" --name "$APP_NAME" --src-path deploy.zip --type zip

# 5. Verify deployment
echo "Verifying deployment..."
for i in 1 2 3; do
  sleep 10
  STATUS=$(curl -s -o /dev/null -w "%{http_code}" "https://$APP_NAME.azurewebsites.net/api/health")
  if [ "$STATUS" = "200" ]; then
    echo "Deployment successful! Site live at https://$APP_NAME.azurewebsites.net"
    break
  fi
  if [ "$i" = "3" ]; then
    echo "WARNING: Health check returned HTTP $STATUS after 3 attempts"
    echo "Check logs: az webapp log tail --resource-group $RESOURCE_GROUP --name $APP_NAME"
  else
    echo "Health check returned $STATUS, retrying in 10s..."
  fi
done

# 6. Cleanup
rm -rf publish deploy.zip
