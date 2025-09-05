# Hypesoft Products Manager

Hypesoft Products Manager, a web API for managing products and categories, built with ASP.NET Core, MongoDB, and Keycloak for authentication. It supports CRUD operations for products and categories, JWT authentication, and is designed for easy integration with front-end applications.

## System Requirements

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Docker](https://www.docker.com/)
- [Node.js](https://nodejs.org/) (if you plan to run a front-end)
- MongoDB (via Docker Compose)
- Keycloak (via Docker Compose)

## Install Dependencies

```sh
dotnet restore Hypesoft.sln
```

## How to Build

```sh
dotnet build Hypesoft.sln
```

## How to Run

```sh
dotnet run --project src/Hypesoft.API/Hypesoft.API.csproj

```

## How to Run Docker Compose (MongoDB)

```sh
docker compose -f docker-compose.mongo.yml up -d
```

## How to Run Docker Compose (Keycloak)

```sh
docker compose -f docker-compose.keycloak.yml up -d
```

## API URL
- - (http://localhost:5198)

## Project Architecture

```
src/
  Hypesoft.API/           # ASP.NET Core Web API (controllers, startup)
  Hypesoft.Application/   # Application logic (DTOs, commands, queries, mapping)
  Hypesoft.Domain/        # Domain entities and interfaces
  Hypesoft.Infrastructure/# Infrastructure (MongoDB repositories, data context)
tests/                    # Unit and integration tests
docker/                   # Docker-related files
```

- **Controllers**: API endpoints ([src/Hypesoft.API/Controllers](src/Hypesoft.API/Controllers))
- **DTOs**: Data transfer objects ([src/Hypesoft.Application/DTOs](src/Hypesoft.Application/DTOs))
- **Repositories**: MongoDB data access ([src/Hypesoft.Infrastructure/Repositories](src/Hypesoft.Infrastructure/Repositories))
- **Entities**: Domain models ([src/Hypesoft.Domain/Entities](src/Hypesoft.Domain/Entities))

## API Endpoints

### Products

- `GET /api/product/all`  
  Search products (supports filtering, pagination)
- `GET /api/product/{id}`  
  Get product by ID
- `POST /api/product`  
  Create a new product
- `PUT /api/product/{id}`  
  Update product
- `DELETE /api/product/{id}`  
  Delete product

### Categories

- `GET /api/category`  
  List all categories
- `GET /api/category/{id}`  
  Get category by ID
- `POST /api/category`  
  Create a new category
- `PUT /api/category/{id}`  
  Update category
- `DELETE /api/category/{id}`  
  Delete category

# Swagger Documentation

- (http://localhost:5198/swagger/index.html)