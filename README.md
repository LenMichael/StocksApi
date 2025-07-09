# StocksApi

A robust, modular RESTful API for stock management, portfolio tracking, and real-time comments, built with ASP.NET Core (.NET 8). The project features secure authentication, background job scheduling, caching, and advanced validation.

## Features

- **User Authentication & Authorization**: ASP.NET Identity with JWT tokens.
- **Stock Management**: CRUD operations for stocks, including filtering, sorting, and pagination.
- **Portfolio Management**: Track user portfolios and related stocks.
- **Comments**: Add and manage comments on stocks, with user association.
- **Background Jobs**: Scheduled and recurring tasks using Hangfire and Coravel (e.g., daily reports, comment processing).
- **Caching**: High-performance Redis caching for improved response times.
- **Validation**: Strong input validation with FluentValidation and custom validators.
- **API Documentation**: Interactive Swagger/OpenAPI documentation.
- **Unit Testing**: xUnit-based tests for services, repositories, and validators.

## Technologies

- .NET 8 / C# 12
- ASP.NET Core Web API
- Entity Framework Core (SQL Server, InMemory for tests)
- Hangfire & Coravel (background jobs)
- StackExchange.Redis
- FluentValidation
- Swagger/OpenAPI
- xUnit, Moq

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Redis](https://redis.io/download) (optional, for caching)

### Setup

1. **Clone the repository**
  - git clone https://github.com/yourusername/StocksApi.git
  - cd StocksApi

2. **Configure the database and Redis**
  - Set your connection strings in `appsettings.json`:
    "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StocksDb;Trusted_Connection=True;",
    "Redis": "localhost:6379"
  }

3. **Apply migrations**
  - dotnet ef database update  

4. **Run the application**
  - dotnet run --project StocksApi


5. **Access Swagger UI**
  - Navigate to [https://localhost:5001/swagger](https://localhost:5001/swagger) (or the port shown in your console).

### Running Tests
dotnet test

## Project Structure

- `Controllers/` - API endpoints
- `Services/` - Business logic (StockService, RedisCacheService, etc.)
- `Repositories/` - Data access layer
- `Models/` - Entity models
- `Dtos/` - Data transfer objects
- `Validators/` - Input validation logic
- `Workers/` - Background jobs (Coravel, Hangfire)
- `Tests/` - Unit tests (xUnit)

## Example API Endpoints

- `GET /api/stocks` - List stocks with filtering and pagination
- `POST /api/stocks` - Create a new stock (requires authentication)
- `GET /api/portfolios/{userId}` - Get a user's portfolio
- `POST /api/comments` - Add a comment to a stock

---

*This project was developed as a modern, extensible API platform for stock and portfolio management, with a focus on clean architecture, security, and scalability.*

 
