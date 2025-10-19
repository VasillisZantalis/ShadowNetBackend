# ShadowNetBackend

ShadowNetBackend is a modern, modular backend service using the **Vertical Slice Architecture** built with ASP.NET Core 9.0. It is designed for scalability, maintainability, and security, leveraging popular open-source libraries and best practices.

## Features

- **JWT Authentication**: Secure endpoints using JSON Web Tokens.
- **Role-based Identity**: User and role management with ASP.NET Core Identity.
- **RESTful Endpoints**: Modular API endpoints using Carter.
- **Validation**: Request validation powered by FluentValidation.
- **CQRS with MediatR**: Clean separation of commands, queries, and handlers.
- **Database Access**: Entity Framework Core with PostgreSQL.
- **Caching**: Distributed caching with Redis.
- **Real-time Communication**: SignalR integration for real-time features.
- **Logging**: Structured logging with Serilog and Seq.
- **Scalar**: API documentation and exploration.
- **Global Exception Handling**: Centralized error handling middleware.

## Architecture

- **Vertical Slice Architecture**: Feature-oriented slices that encapsulate all layers for a single use case.
- **CQRS Pattern**: Commands and queries are handled separately using MediatR.
- **Dependency Injection**: All services are registered and resolved via DI.
- **Modular Endpoints**: Carter is used for organizing endpoints by feature.
- **Validation Pipeline**: FluentValidation integrated as a MediatR pipeline behavior.
- **Exception Handling**: Custom middleware for consistent error responses.

## Main Technologies & Packages

- [.NET 9.0](https://dotnet.microsoft.com/)
- [Carter](https://github.com/CarterCommunity/Carter)
- [FluentValidation](https://fluentvalidation.net/)
- [MediatR](https://github.com/jbogard/MediatR)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [Npgsql](https://www.npgsql.org/)
- [StackExchange.Redis](https://stackexchange.github.io/StackExchange.Redis/)
- [Serilog](https://serilog.net/)
- [SignalR](https://docs.microsoft.com/aspnet/core/signalr/)

## Patterns Used

- **CQRS (Command Query Responsibility Segregation)**
- **Mediator Pattern** (via MediatR)
- **Repository Pattern** (via EF Core DbContext)
- **Validation Pipeline** (FluentValidation behaviors)
- **Dependency Injection**
- **Modular Routing** (Carter)

## Getting Started

1. Clone the repository.
2. Set up PostgreSQL and Redis instances.
3. Configure secrets and connection strings (see `appsettings.json` and user secrets).
4. Run database migrations.
5. Start the application.

## How to Run

You can run the application in two ways:

### Option 1: Using Visual Studio
1. Open the solution in Visual Studio.
2. Set `docker-compose` as the startup project (via the dropdown next to the Run button).
3. Click **Run** (F5) to start the containers.
4. Set `ShadowNetBackend` as the startup project and run it to start the API.

### Option 2: Using Docker CLI
1. Open a terminal in the project root.
2. Run the command:

```bash
docker-compose up
```
3. Set `ShadowNetBackend` as the startup project and run it to start the API.

## Project Structure

- `ShadowNetBackend/` - Main API project
- `ShadowNetBackend.Tests.Unit/` - Unit tests
- `ShadowNetBackend.Tests.Integration/` - Integration tests
