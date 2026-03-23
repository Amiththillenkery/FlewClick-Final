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
| Professional User | `UserType.ProfessionalUser` | Service provider (must have at least one ProfessionalRole) |

### Professional Roles (multi-select)

A professional can hold **one or more** roles simultaneously (e.g. Photographer + Videographer).

| Role | Enum Value |
|------|------------|
| Photographer | `ProfessionalRole.Photographer` |
| Videographer | `ProfessionalRole.Videographer` |
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

4. **Open Swagger** at `http://localhost:5216/swagger`

---

## Feature Endpoint Calling Order

Each feature below lists endpoints in the **order they should be called** to complete the flow end-to-end.

---

### Feature 1: Professional Registration

Register a new professional user, configure role-specific settings, and complete registration.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `POST` | `/api/registration/professional/profile` | Create professional user + profile |
| 2 | `GET` | `/api/registration/professional/{profileId}/status` | Check which role configs are pending |
| 3a | `POST` | `/api/registration/professional/{profileId}/photography` | Configure photography (Photographer / Videographer) |
| 3b | `POST` | `/api/registration/professional/{profileId}/editing` | Configure editing (Editor) |
| 3c | `POST` | `/api/registration/professional/{profileId}/drone` | Configure drone (Drone Owner) |
| 3d | `POST` | `/api/registration/professional/{profileId}/rental-equipment` | Add rental equipment (Digital Rental) |
| 4 | `GET` | `/api/registration/professional/{profileId}/status` | Verify all role configs are complete |
| 5 | `POST` | `/api/registration/professional/{profileId}/complete` | Finalize registration |

> **Note:** Step 3 depends on the user's roles. A user with multiple roles (e.g. Photographer + Videographer) must complete all applicable configs before step 5.

**Step 1 request body:**

```json
{
  "fullName": "Jane Doe",
  "email": "jane@example.com",
  "phone": "+919876543210",
  "professionalRoles": [0, 1],
  "bio": "Professional photographer and videographer",
  "location": "Mumbai, India",
  "yearsOfExperience": 5,
  "hourlyRate": 75.00,
  "portfolioUrl": "https://janedoe.com"
}
```

**ProfessionalRoles values:** `0`=Photographer, `1`=Videographer, `2`=Editor, `3`=DroneOwner, `4`=DigitalRental

---

### Feature 2: Package Configuration (Photographers, Videographers, Editors, Drone Owners)

Create service packages, attach deliverables from the master catalog, and configure pricing.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `POST` | `/api/packages` | Create a new package |
| 2 | `GET` | `/api/packages/{id}` | View package details |
| 3 | `GET` | `/api/deliverables/master/{role}` | Browse available deliverables for your role |
| 4 | `POST` | `/api/packages/{packageId}/deliverables` | Add a deliverable to the package |
| 5 | `POST` | `/api/packages/{packageId}/pricing` | Set pricing for the package |
| 6 | `GET` | `/api/packages/profile/{profileId}` | List all packages for a profile |

**Step 1 request body:**

```json
{
  "profileId": "{{profileId}}",
  "name": "Premium Wedding Package",
  "packageType": 0,
  "description": "Complete wedding coverage",
  "coverageType": 1
}
```

**PackageType values:** `0`=Wedding, `1`=PreWedding, `2`=Birthday, `3`=Corporate, `4`=Event, `5`=QuickShoot, `6`=FullDay, `7`=HalfDay, `8`=Custom

**CoverageType values:** `0`=OneSide, `1`=BothSides, `2`=Full, `3`=Custom

**Step 4 request body:**

```json
{
  "deliverableMasterId": "{{masterId}}",
  "quantity": 2,
  "configuration": { "leaf_count": 80, "size": "A4" },
  "notes": "Premium quality album"
}
```

**Step 5 request body:**

```json
{
  "pricingType": 0,
  "basePrice": 50000,
  "discountPercentage": 10,
  "durationHours": null,
  "isNegotiable": true,
  "notes": "Full day coverage"
}
```

**PricingType values:** `0`=Fixed, `1`=PerHour, `2`=PerDay, `3`=PerEvent, `4`=Custom

#### Additional Package Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `PUT` | `/api/packages/{id}` | Update package details |
| `PATCH` | `/api/packages/{id}/status` | Activate / deactivate package |
| `DELETE` | `/api/packages/{id}` | Delete a package |
| `GET` | `/api/packages/{packageId}/deliverables` | List deliverables in a package |
| `PUT` | `/api/packages/{packageId}/deliverables/{id}` | Update a package deliverable |
| `DELETE` | `/api/packages/{packageId}/deliverables/{id}` | Remove a deliverable from package |
| `GET` | `/api/packages/{packageId}/pricing` | Get package pricing |
| `DELETE` | `/api/packages/{packageId}/pricing` | Remove package pricing |

---

### Feature 3: Rental Store (Digital Rental Professionals Only)

Configure a rental store, list products with images, and set rental pricing.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `POST` | `/api/rental/store` | Configure rental store |
| 2 | `POST` | `/api/rental/store/{storeId}/products` | Add a product to the store |
| 3 | `POST` | `/api/rental/products/{productId}/images` | Upload product photos |
| 4 | `POST` | `/api/rental/products/{productId}/pricing` | Set rental rates for the product |
| 5 | `GET` | `/api/rental/products/{id}` | View full product detail (product + images + pricing) |
| 6 | `GET` | `/api/rental/store/{storeId}/products` | List all products in the store |

**Step 1 request body:**

```json
{
  "profileId": "{{profileId}}",
  "storeName": "ProGear Rentals",
  "description": "Premium camera and lens rentals",
  "policies": {
    "min_rental_days": 1,
    "deposit_required": true,
    "cancellation_policy": "24 hours notice required"
  }
}
```

**Step 2 request body:**

```json
{
  "name": "Sony A7IV",
  "condition": 1,
  "description": "Full frame mirrorless camera",
  "category": "Camera",
  "brand": "Sony",
  "model": "A7IV",
  "specifications": { "sensor": "Full Frame", "megapixels": 33 }
}
```

**ProductCondition values:** `0`=New, `1`=LikeNew, `2`=Good, `3`=Fair

**Step 3 request body:**

```json
{
  "imageUrl": "https://cdn.example.com/products/sony-a7iv.jpg",
  "displayOrder": 0,
  "isPrimary": true
}
```

**Step 4 request body:**

```json
{
  "depositAmount": 5000,
  "hourlyRate": 200,
  "dailyRate": 1500,
  "weeklyRate": 8000,
  "monthlyRate": 25000
}
```

#### Additional Rental Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `PUT` | `/api/rental/store/{id}` | Update store configuration |
| `GET` | `/api/rental/store/profile/{profileId}` | Get store by professional profile |
| `PUT` | `/api/rental/products/{id}` | Update product details |
| `DELETE` | `/api/rental/products/{id}` | Delete a product |
| `PATCH` | `/api/rental/products/{id}/availability` | Toggle product availability |
| `DELETE` | `/api/rental/products/images/{imageId}` | Remove a product image |

---

### Feature 4: User Management

Query and manage users in the system.

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/users` | List all users |
| `GET` | `/api/users/{id}` | Get user by ID |
| `GET` | `/api/users/type/{userType}` | Filter by user type (`User` / `ProfessionalUser`) |
| `GET` | `/api/users/professionals/{role}` | Filter professionals by role (`Photographer`, `Videographer`, etc.) |
| `PUT` | `/api/users/{id}/profile` | Update user profile (name, phone) |
| `POST` | `/api/users/{id}/deactivate` | Deactivate a user |

---

### Health Check

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/healthz` | Application + database health status |
