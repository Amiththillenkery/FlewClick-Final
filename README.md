# FlewClick Backend

A robust, scalable backend for the FlewClick digital hub built with .NET 10, following **Clean Architecture** and **Domain-Driven Design (DDD)** with **CQRS**.

## Solution Structure

```
backend/
├── FlewClick.sln
├── docker-compose.yml
├── src/
│   ├── FlewClick.Domain/          # Entities, enums, value objects, exceptions
│   ├── FlewClick.Application/     # CQRS (MediatR), validators, repository interfaces
│   ├── FlewClick.Infrastructure/  # EF Core + PostgreSQL, repository implementations
│   └── FlewClick.Api/             # Minimal API endpoints, health check, Swagger
```

## User Roles

| Role | Type | Description |
|------|------|-------------|
| User | `UserType.User` | General consumer who accesses services |
| Professional User | `UserType.ProfessionalUser` | Service provider (must have a ProfessionalRole) |

### Professional Roles (subtypes of Professional User)

| Role | Enum Value |
|------|------------|
| Photographer / Videographer | `ProfessionalRole.PhotographerVideographer` |
| Editor | `ProfessionalRole.Editor` |
| Drone Owner | `ProfessionalRole.DroneOwner` |
| Digital Rental | `ProfessionalRole.DigitalRental` |

## Prerequisites

- .NET 10 SDK
- Docker (for PostgreSQL)
- EF Core tools: `dotnet tool install -g dotnet-ef`

## Quick Start

1. **Start PostgreSQL**

   ```bash
   docker-compose up -d
   ```

2. **Apply migrations**

   ```bash
   dotnet ef database update --project src/FlewClick.Infrastructure --startup-project src/FlewClick.Api
   ```

3. **Run the API**

   ```bash
   dotnet run --project src/FlewClick.Api
   ```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/healthz` | Health check |
| GET | `/api/users` | List all users |
| GET | `/api/users/{id}` | Get user by ID |
| GET | `/api/users/type/{userType}` | Filter by user type (User / ProfessionalUser) |
| GET | `/api/users/professionals/{role}` | Filter professionals by role |
| POST | `/api/users/register` | Register a general user |
| POST | `/api/users/register-professional` | Register a professional user |
| PUT | `/api/users/{id}/profile` | Update user profile |
| POST | `/api/users/{id}/deactivate` | Deactivate a user |
