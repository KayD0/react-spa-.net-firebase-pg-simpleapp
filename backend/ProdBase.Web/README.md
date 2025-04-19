# .NET Backend for User Profile API

This is a .NET implementation of the User Profile API backend, equivalent to the Go implementation.

## Features

- User authentication using Firebase
- User profile management
- PostgreSQL database integration
- RESTful API endpoints

## Prerequisites

- .NET 8.0 SDK or higher
- PostgreSQL database
- Firebase project

## Environment Variables

Copy the `.env.example` file to `.env` and fill in the values:

```bash
# Windows
copy .env.example .env

# Unix/Linux/macOS
cp .env.example .env
```

Alternatively, you can set these values in your `appsettings.json` or `appsettings.Development.json` files.

## Installation

1. Restore dependencies:

```bash
dotnet restore
```

2. Build the application:

```bash
dotnet build
```

## Running the Application

```bash
dotnet run
```

The API will be available at https://localhost:44308

## API Endpoints

- `GET /` - API information
- `POST /api/auth/verify` - Verify authentication token
- `GET /api/profile` - Get user profile
- `PUT /api/profile` - Update user profile

## Development

### Project Structure

- `Program.cs` - Application entry point and configuration
- `Controllers/` - API route handlers
- `Middleware/` - Middleware functions
- `Models/` - Database models
- `Services/` - Business logic and external services
- `Data/` - Database context and configuration

### Database Migrations

To create and apply database migrations:

```bash
# Create a migration
dotnet ef migrations add InitialCreate

# Apply migrations to the database
dotnet ef database update
