# my-pantry

A small .NET application for managing pantry products.

## Stack
- ASP.NET Core API
- Entity Framework Core
- SQL Server
- Docker Compose

## Solution structure
- MyPantry.Api: REST API and static web UI
- MyPantry.Core: domain models and contracts
- MyPantry.Infrastructure: EF Core DbContext and repository implementation
- MyPantry.Web: MVC sample project

## Configuration

### Environment Variables

The application requires a `.env` file in the project root to configure the database connection and other settings.

#### `DB_SERVER`
- **Description**: Address and port of the SQL Server instance
- **Default value**: `localhost,1433`
- **Examples**: 
  - Local: `localhost,1433`
  - Remote: `my-server.database.windows.net,1433`
  - Named instance: `SERVER\SQLEXPRESS,1433`
- Docker Compose connects the API to the `sqlserver` service automatically; this setting is used for running the API outside Compose.

#### `DB_NAME`
- **Description**: Name of the SQL Server database
- **Default value**: `MyPantryDb`
- **Examples**: `MyPantryDb`, `MyPantry_Production`

#### `DB_USER`
- **Description**: Username for SQL Server authentication
- **Default value**: `sa` (System Administrator)
- **Examples**: 
  - SQL Server: `sa`, `myuser`
  - Azure SQL: `admin@myserver`
- **Note**: For production environments, use a user with limited privileges

#### `DB_PASSWORD`
- **Description**: Password for the SQL Server user
- **Required**: Provide it through the ignored local `.env` file or your deployment's secret configuration
- **Requirements**:
  - Minimum 8 characters
  - Contains uppercase, lowercase, numbers, and special characters
  - Must not contain the username or database name
- **Note**: Keep this password secure and never commit it to Git

#### `DB_TRUST_SERVER_CERTIFICATE`
- **Description**: Allows connection to SQL Server with a self-signed certificate (local development)
- **Allowed values**: `True`, `False`
- **Default value**: `True`
- **Recommended per environment**:
  - Local development: `True`
  - Production: `False` (requires valid certificate)

#### `ASPNETCORE_ENVIRONMENT`
- **Description**: Application execution environment
- **Allowed values**: `Development`, `Staging`, `Production`
- **Default value**: `Development`
- **Behavior per environment**:
  - `Development`: Enables Swagger UI, detailed logging
  - `Staging`: Intermediate configuration for pre-production
  - `Production`: Disables debug, minimal logging, optimizations

## Run locally
1. Start Docker Desktop.
2. Run `start.bat` from File Explorer or Command Prompt. The script builds and starts SQL Server and the API, waits for the API health check, then opens the app in your browser.

To stop the containers, run `docker compose down` from the project folder.

The app is available at http://localhost:8080.

The API endpoints are available at:
- GET /api/products
- POST /api/products
- GET /api/products/{id}
- PUT /api/products/{id}
- DELETE /api/products/{id}
