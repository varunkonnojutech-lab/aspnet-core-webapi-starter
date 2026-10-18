# ASP.NET Core Web API Starter

[![Build](https://github.com/varunkonnojutech-lab/aspnet-core-webapi-starter/actions/workflows/ci.yml/badge.svg)](https://github.com/varunkonnojutech-lab/aspnet-core-webapi-starter/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://codespaces.new/varunkonnojutech-lab/aspnet-core-webapi-starter)

A production-style starter template for building REST APIs with ASP.NET Core 8. It wires up everything a real service needs out of the box: Entity Framework Core, JWT authentication, request validation, structured logging, global error handling, Swagger, integration tests, Docker support, and CI.

## Who is this for

Backend developers who want a solid, opinionated starting point for a .NET API — or a reference for how the pieces (auth, validation, error handling, testing) fit together.

## What you'll learn

- Structuring a Web API: controllers, DTOs, validators, middleware
- JWT bearer authentication with login and protected endpoints
- FluentValidation request validation with automatic 400 responses
- Global exception middleware returning RFC 7807-style problem JSON
- Serilog structured logging
- Entity Framework Core with SQLite and seed data
- Integration testing with xUnit + `WebApplicationFactory`
- Containerizing with a multi-stage Dockerfile

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) — or click **Open in GitHub Codespaces** above for a ready-made environment in your browser, no install needed.
- [Docker](https://www.docker.com/) (optional, for the container run)

## Quick start

```bash
git clone https://github.com/varunkonnojutech-lab/aspnet-core-webapi-starter.git
cd aspnet-core-webapi-starter/src/WebApi
dotnet run
```

The API listens on `http://localhost:5000`. Open Swagger at `http://localhost:5000/swagger`.

### Run the tests

```bash
cd tests/WebApi.Tests
dotnet test
```

### Run with Docker

```bash
docker build -t webapi-starter .
docker run -p 8080:8080 webapi-starter
```

Swagger is then available at `http://localhost:8080/swagger`.

## How it works

A request flows through: **routing → JWT authentication → FluentValidation → controller → EF Core (SQLite)**. Anything unexpected is caught by the global exception middleware, logged with Serilog, and returned as problem-details JSON. `GET /api/products` is public; write operations require a JWT from the demo login endpoint.

## Authentication

`POST /api/auth/login` accepts the demo credentials and returns a JWT:

- Username: `demo`
- Password: `demo123`

> Demo only — replace the hardcoded user with a real user store before shipping.

## Endpoints

| Method | Route                 | Auth     | Description          |
|--------|-----------------------|----------|----------------------|
| GET    | /api/products         | —        | List all products    |
| GET    | /api/products/{id}    | —        | Get one product      |
| POST   | /api/products         | JWT      | Create a product     |
| PUT    | /api/products/{id}    | JWT      | Update a product     |
| DELETE | /api/products/{id}    | JWT      | Delete a product     |
| POST   | /api/auth/login       | —        | Get a JWT token      |

### Example calls

```bash
# List products (no auth needed)
curl http://localhost:5000/api/products

# Log in and capture the token
TOKEN=$(curl -s -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"demo","password":"demo123"}' \
  | python3 -c "import sys, json; print(json.load(sys.stdin)['token'])")

# Create a product (auth required)
curl -X POST http://localhost:5000/api/products \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"name":"USB-C Hub","description":"7-in-1 hub","price":59.99,"stock":120}'

# Validation example (400): empty name + negative price
curl -X POST http://localhost:5000/api/products \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"name":"","price":-5,"stock":1}'
```

## Project structure

```
aspnet-core-webapi-starter/
├── src/WebApi/
│   ├── Controllers/      # ProductsController, AuthController
│   ├── Data/             # AppDbContext (SQLite + seed data)
│   ├── Dtos/             # ProductDto, CreateProductDto, UpdateProductDto
│   ├── Middleware/       # Global exception handling
│   ├── Models/           # Product entity
│   ├── Validators/       # FluentValidation rules
│   ├── Program.cs        # DI, JWT, Swagger, Serilog, middleware wiring
│   └── appsettings.json  # Connection string + JWT settings
├── tests/WebApi.Tests/   # Integration tests (WebApplicationFactory)
├── .devcontainer/        # One-click GitHub Codespaces setup
├── .github/workflows/    # CI: build + test on push
├── Dockerfile            # Multi-stage build
└── README.md
```

## Configuration

JWT settings live in `appsettings.json` under the `Jwt` section (`Key`, `Issuer`, `Audience`). Override the key in production via environment variables, e.g. `Jwt__Key`.

## Next steps

Want the enterprise-grade version of this architecture? Continue with [clean-architecture-cqrs-template](https://github.com/varunkonnojutech-lab/clean-architecture-cqrs-template) — Clean Architecture + CQRS with MediatR.

## License

MIT — see [LICENSE](LICENSE).
