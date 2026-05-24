# Productivity Optimizer API (.NET)

A creative, weather-powered REST API built with ASP.NET Core 10, optimized for Azure App Services deployment.

## Features

### 🎯 Endpoints

1. **Productivity Recommendation** (`GET /api/productivity/recommend`)
   - Returns personalized productivity tips based on energy level and task type
   - Query params: `energyLevel` (low|medium|high), `taskType` (focus|creativity|energy), `limit` (number)

2. **Focus Score Calculator** (`POST /api/focus-score/calculate`)
   - Analyzes weather conditions to calculate optimal focus conditions
   - Request body: `temperature` (°C), `humidity` (0-100), `cloudCover` (0-100), `isRaining` (bool), `dayOfWeek` (string)
   - Returns focus score (0-100) with recommendations

3. **Health Check** (`GET /health`)
   - Azure App Services health monitoring endpoint

4. **Swagger/OpenAPI** (`GET /swagger`)
   - Interactive API documentation

## Prerequisites

- .NET 10 SDK or later
- Visual Studio 2022, VS Code, or any .NET-compatible IDE

## Quick Start

### Local Development

```bash
dotnet restore
dotnet build
dotnet run
```

API will be available at `http://localhost:8080`
Swagger UI at `http://localhost:8080/swagger`

## Azure App Services Deployment

### Prerequisites
- Azure CLI installed
- Azure subscription

### Deploy from Visual Studio

1. Right-click project → Publish
2. Select "Azure" → "Azure App Service"
3. Create new App Service or select existing
4. Target framework: .NET 8
5. Click Publish

### Deploy from Azure CLI

```bash
# Create resource group
az group create --name MyResourceGroup --location eastus

# Create App Service plan
az appservice plan create \
  --name MyAppServicePlan \
  --resource-group MyResourceGroup \
  --sku B1 \
  --is-linux

# Create web app
az webapp create \
  --resource-group MyResourceGroup \
  --plan MyAppServicePlan \
  --name productivity-optimizer-api \
  --runtime "DOTNETCORE|8.0"

# Publish from local
dotnet publish -c Release -o ./publish
az webapp deployment source config-zip \
  --resource-group MyResourceGroup \
  --name productivity-optimizer-api \
  --src-path ./publish.zip
```

### Deploy from GitHub (CI/CD)

```bash
# Create GitHub action workflow
# The publish directory can be zipped and deployed to Azure
```

## Project Structure

```
.
├── ProductivityOptimizerApi.csproj  # Project file
├── Program.cs                        # Application startup
├── Controllers.cs                    # API endpoints
├── Models.cs                         # Request/Response models
├── Services.cs                       # Business logic
├── appsettings.json                  # Configuration
├── web.config                        # IIS configuration for Azure
├── .gitignore                        # Git ignore rules
└── README.md                         # This file
```

## API Examples

### Get Productivity Tips
```bash
curl "http://localhost:8080/api/productivity/recommend?energyLevel=high&taskType=focus&limit=2"
```

### Calculate Focus Score
```bash
curl -X POST http://localhost:8080/api/focus-score/calculate \
  -H "Content-Type: application/json" \
  -d '{
    "temperature": 21,
    "humidity": 50,
    "cloudCover": 40,
    "isRaining": false,
    "dayOfWeek": "Wednesday"
  }'
```

### Health Check
```bash
curl http://localhost:8080/health
```

## Environment Variables

- `ASPNETCORE_ENVIRONMENT` - Environment (Development/Production)
- `PORT` - Server port (default: 8080)

## Monitoring in Azure

- Application Insights for detailed monitoring
- Health endpoint automatically monitored by App Service
- Check logs in Azure Portal → App Service → Log stream

## License

MIT
