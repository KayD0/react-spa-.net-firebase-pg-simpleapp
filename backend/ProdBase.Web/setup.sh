#!/bin/bash

echo "Setting up .NET backend with Clean Architecture..."

echo "Copying environment file..."
cp .env.example .env

echo "Done!"
echo
echo "To run the application:"
echo "1. Make sure .NET 8.0 SDK is installed"
echo "2. Run: dotnet restore"
echo "3. Run: dotnet build"
echo "4. Run: cd ProdBase.Web"
echo "5. Run: dotnet run"
echo
echo "The API will be available at http://localhost:5000"
