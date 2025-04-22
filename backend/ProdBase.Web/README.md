# ProdBase API - Clean Architecture

This is a .NET Core Web API project structured according to Clean Architecture principles.

## Project Structure

The solution is divided into four projects:

1. **ProdBase.Domain**
   - Contains business entities and interfaces
   - Core business logic
   - No dependencies on other projects or external libraries (except for minimal .NET dependencies)

2. **ProdBase.Application**
   - Contains application use cases and DTOs
   - Depends only on the Domain layer
   - Implements business logic using domain entities and interfaces

3. **ProdBase.Infrastructure**
   - Contains implementations of interfaces defined in the Domain layer
   - Provides concrete implementations for data access, external services, etc.
   - Depends on Domain and Application layers

4. **ProdBase.Web**
   - Contains API controllers, middleware, and configuration
   - Depends on all other layers
   - Responsible for handling HTTP requests and responses

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL database

### Setup

1. Clone the repository
2. Run the setup script:
   - Windows: `setup.bat`
   - Linux/Mac: `./setup.sh`
3. Update the connection string in `appsettings.json` if needed
4. Run the application:
   ```
   dotnet restore
   dotnet build
   cd ProdBase.Web
   dotnet run
   ```

## API Endpoints

- `/` - API information
- `/api/auth/verify` - Verify authentication token (POST with Authorization header)
- `/api/profile` - Get user profile (GET with Authorization header)
- `/api/profile` - Update user profile (PUT with JSON body and Authorization header)

## Authentication

This API uses Firebase Authentication. Include a valid Firebase ID token in the Authorization header for authenticated endpoints.

## Docker Support

A Dockerfile is included for containerization. To build and run the Docker container:

```
docker build -t prodbase-api .
docker run -p 8080:8080 prodbase-api
