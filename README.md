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

### Feature 4: Portfolio Configuration (Photographers, Videographers, Editors, Drone Owners)

Connect Instagram, sync posts as portfolio items, and manage visibility/ordering. Uses **Instagram Graph API** -- no file uploads needed.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `GET` | `/api/portfolio/instagram/auth-url?profileId={id}` | Get Instagram OAuth authorization URL |
| 2 | -- | _User authorizes on Instagram_ | Redirects back to callback with code |
| 3 | `GET` | `/api/portfolio/instagram/callback?code={code}&state={profileId}` | Exchange auth code for tokens, store connection |
| 4 | `GET` | `/api/portfolio/instagram/{profileId}/status` | Check Instagram connection status |
| 5 | `POST` | `/api/portfolio/{profileId}/sync` | Sync posts from Instagram into portfolio |
| 6 | `GET` | `/api/portfolio/{profileId}` | View portfolio items (public) |

> **Note:** Steps 1-3 are the one-time OAuth connection flow. Step 5 (sync) can be called anytime to refresh media URLs and pull new posts. DigitalRental professionals cannot use portfolio -- they use the Rental Store instead.

**Step 5 response:**

```json
{
  "totalItems": 25,
  "newItems": 25,
  "updatedItems": 0
}
```

**Step 6 query parameter:** `?visibleOnly=true` (default) or `?visibleOnly=false` to see all items including hidden ones.

#### Portfolio Management Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `PATCH` | `/api/portfolio/items/{itemId}/visibility` | Toggle item visibility (show/hide) |
| `PUT` | `/api/portfolio/{profileId}/reorder` | Reorder portfolio items |
| `POST` | `/api/portfolio/instagram/{profileId}/refresh` | Refresh Instagram token (before 60-day expiry) |
| `DELETE` | `/api/portfolio/instagram/{profileId}?clearPortfolio=false` | Disconnect Instagram |

**Reorder request body:**

```json
{
  "orderedItemIds": [
    "{{itemId1}}",
    "{{itemId2}}",
    "{{itemId3}}"
  ]
}
```

#### Instagram Meta App Endpoints (required for app setup)

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/portfolio/instagram/webhook` | Meta webhook verification (returns `hub.challenge`) |
| `POST` | `/api/portfolio/instagram/deauthorize` | Deauthorize callback (user removed app) |
| `POST` | `/api/portfolio/instagram/data-deletion` | GDPR data deletion request |

---

### Feature 5: Consumer Authentication (OTP + JWT)

Register and login as a consumer using mobile number OTP verification. JWT tokens are returned after successful verification.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `POST` | `/api/auth/register` | Send registration OTP to phone |
| 2 | `POST` | `/api/auth/verify-registration` | Verify OTP, create consumer, return JWT |
| 3 | `POST` | `/api/auth/login` | Send login OTP to phone |
| 4 | `POST` | `/api/auth/verify-login` | Verify OTP, return JWT |
| 5 | `GET` | `/api/auth/me` | Get logged-in consumer profile (requires JWT) |

> **Note:** SMS is currently mocked -- OTP codes are logged to the console. Check server logs for the OTP.

**Step 1 request body:**

```json
{
  "phone": "+919876543210",
  "fullName": "John Consumer"
}
```

**Step 2 request body:**

```json
{
  "phone": "+919876543210",
  "otp": "123456",
  "fullName": "John Consumer"
}
```

**Step 3 request body:**

```json
{
  "phone": "+919876543210"
}
```

**Step 4 request body:**

```json
{
  "phone": "+919876543210",
  "otp": "654321"
}
```

**Step 2 & 4 response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "consumer": {
    "id": "{{consumerId}}",
    "phone": "+919876543210",
    "fullName": "John Consumer",
    "email": null,
    "isPhoneVerified": true,
    "isActive": true,
    "lastLoginAt": "2026-03-25T10:00:00Z",
    "createdAtUtc": "2026-03-25T09:55:00Z"
  }
}
```

---

### Feature 6: Browse & Search Professionals

Browse categories, filter professionals by role/location, and search by text. All endpoints are public (no auth required).

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `GET` | `/api/browse/categories` | List professional role categories with counts |
| 2 | `GET` | `/api/browse/professionals?role={role}&location={loc}&page=1&pageSize=20` | Browse professionals by category/location |
| 3 | `GET` | `/api/search/professionals?q={query}&role={role}&page=1&pageSize=20` | Search across name, bio, location |

**ProfessionalRole query values:** `0`=Photographer, `1`=Videographer, `2`=Editor, `3`=DroneOwner, `4`=DigitalRental

**Step 1 response example:**

```json
[
  { "role": 0, "name": "Photographer", "professionalCount": 12 },
  { "role": 1, "name": "Videographer", "professionalCount": 8 }
]
```

---

### Feature 7: Professional Detail (Consumer View)

View full professional profile details and their packages. Portfolio is already available via Feature 4. All endpoints are public.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `GET` | `/api/professionals/{profileId}/detail` | Full profile: bio, location, experience, rates, roles, package/portfolio counts |
| 2 | `GET` | `/api/professionals/{profileId}/packages` | Active packages with deliverables and pricing |
| 3 | `GET` | `/api/portfolio/{profileId}` | Portfolio items (reuses existing endpoint from Feature 4) |

---

### Feature 8: Shortlist / Save Professionals

Save professionals to a shortlist for later. **Requires JWT authentication** (consumer must be logged in).

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `POST` | `/api/shortlist/{profileId}` | Save a professional to shortlist |
| 2 | `GET` | `/api/shortlist` | List all saved professionals |
| 3 | `DELETE` | `/api/shortlist/{profileId}` | Remove from shortlist |

> Pass the JWT token in the `Authorization: Bearer {token}` header.

---

### Feature 9: User Management

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

### Feature 10: Booking Requests

Create and manage booking requests between consumers and professionals. Uses a **state machine** pattern with explicit transition guards.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `POST` | `/api/bookings` | Consumer creates a booking request for a professional's package |
| 2 | `GET` | `/api/bookings/my` | Consumer lists their bookings |
| 3 | `GET` | `/api/bookings/incoming?profileId={id}` | Professional lists incoming requests |
| 4 | `GET` | `/api/bookings/{id}` | Get full booking detail (both parties) |
| 5 | `POST` | `/api/bookings/{id}/accept` | Professional accepts request (**blocked if unpaid platform fees exist**) |
| 6 | `POST` | `/api/bookings/{id}/decline` | Professional declines with reason |
| 7 | `POST` | `/api/bookings/{id}/cancel` | Either party cancels with reason |
| 8 | `POST` | `/api/bookings/{id}/complete` | Professional marks project completed (creates 3% platform fee) |

> All endpoints require JWT authentication.

**Step 1 request body:**

```json
{
  "professionalProfileId": "{{profileId}}",
  "packageId": "{{packageId}}",
  "eventDate": "2026-04-15T10:00:00Z",
  "eventLocation": "Mumbai, India",
  "notes": "Outdoor wedding ceremony"
}
```

**Step 5 request body:**

```json
{
  "professionalProfileId": "{{profileId}}"
}
```

**Step 6 request body:**

```json
{
  "professionalProfileId": "{{profileId}}",
  "reason": "Schedule conflict on that date"
}
```

**Booking State Flow:** `Requested` -> `PendingQuotation` -> `QuotationSent` -> `Accepted` -> `Active` -> `Completed`

---

### Feature 11: Agreements (Quotation & Negotiation)

Versioned agreement system for professionals to send quotations with deliverables. Consumers can request revisions before accepting.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `POST` | `/api/bookings/{bookingId}/agreement` | Professional creates agreement with deliverables & pricing |
| 2 | `GET` | `/api/bookings/{bookingId}/agreement` | View current (latest) agreement |
| 3 | `GET` | `/api/bookings/{bookingId}/agreement/history` | View all agreement versions |
| 4 | `POST` | `/api/bookings/{bookingId}/agreement/request-revision` | Consumer requests changes |
| 5 | `PUT` | `/api/bookings/{bookingId}/agreement/revise` | Professional sends revised agreement (new version) |
| 6 | `POST` | `/api/bookings/{bookingId}/agreement/accept` | Consumer accepts -> project goes Active immediately |

> All endpoints require JWT authentication.

**Step 1 request body:**

```json
{
  "bookingRequestId": "{{bookingId}}",
  "packageSnapshot": "{\"name\":\"Premium Wedding\",\"type\":\"Wedding\",\"coverage\":\"BothSides\"}",
  "eventDate": "2026-04-15T10:00:00Z",
  "eventLocation": "Mumbai, India",
  "eventDescription": "Grand wedding ceremony",
  "totalPrice": 75000,
  "terms": "50% advance, 50% on delivery",
  "conditions": "Weather permitting for outdoor shoots",
  "notes": "Includes drone coverage",
  "deliverables": [
    {
      "deliverableName": "Photo Album",
      "quantity": 2,
      "configuration": { "leaf_count": 80, "size": "A4" },
      "notes": "Premium quality"
    },
    {
      "deliverableName": "Edited Photos",
      "quantity": 500,
      "configuration": null,
      "notes": "Color corrected"
    }
  ]
}
```

**Step 4 request body:**

```json
{
  "revisionNotes": "Can we add drone coverage and increase photo count to 600?"
}
```

---

### Feature 12: Platform Fees (Post-Completion Billing)

3% platform fee collected **after** project completion. Professionals are blocked from accepting new bookings until outstanding fees are paid.

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `GET` | `/api/platform-fees/outstanding?profileId={id}` | Check outstanding fees & blocked status |
| 2 | `POST` | `/api/platform-fees/{feeId}/initiate` | Create Razorpay order to pay a fee |
| 3 | `POST` | `/api/platform-fees/verify` | Verify Razorpay payment (webhook, no auth) |
| 4 | `GET` | `/api/platform-fees/history?profileId={id}` | View all fee payments |

> Steps 1, 2, 4 require JWT. Step 3 is a Razorpay webhook (no auth).

**Step 1 response:**

```json
{
  "isBlocked": true,
  "totalOutstanding": 2250.00,
  "fees": [
    {
      "id": "{{feeId}}",
      "bookingRequestId": "{{bookingId}}",
      "agreementAmount": 75000.00,
      "feePercentage": 3.0,
      "feeAmount": 2250.00,
      "status": "Pending",
      "dueDate": "2026-04-14T00:00:00Z"
    }
  ]
}
```

**Step 2 request body:**

```json
{
  "professionalProfileId": "{{profileId}}"
}
```

**Step 2 response:**

```json
{
  "orderId": "order_mock_abc123",
  "amount": 2250.00,
  "currency": "INR"
}
```

**Fee blocking rules:**
- Requests can arrive at any time -- never blocked
- The professional can see all incoming requests
- Only the **accept action** is blocked when unpaid fees exist
- Active/in-progress projects are never affected
- Once all fees are paid, new acceptances are allowed

---

### Feature 13: Real-Time Chat

Chat between consumer and professional for booking negotiation and project communication. Uses **SignalR** for real-time messaging.

**REST Endpoints:**

| Step | Method | Endpoint | Description |
|------|--------|----------|-------------|
| 1 | `GET` | `/api/chat/{bookingId}` | Get conversation for a booking |
| 2 | `GET` | `/api/chat/{bookingId}/messages?page=1&pageSize=50` | Get paginated messages |
| 3 | `POST` | `/api/chat/{bookingId}/messages` | Send a message |
| 4 | `PATCH` | `/api/chat/{bookingId}/messages/read` | Mark messages as read |

> All endpoints require JWT authentication.

**Step 3 request body:**

```json
{
  "content": "Hello! I'd like to discuss the wedding photography details.",
  "senderType": 0
}
```

**SenderType values:** `0`=Consumer, `1`=Professional, `2`=System

**SignalR Hub:** `/hubs/chat`

Connect with JWT token as query parameter: `/hubs/chat?access_token={jwt}`

| Hub Method | Direction | Description |
|------------|-----------|-------------|
| `JoinConversation(conversationId)` | Client -> Server | Join a conversation room |
| `LeaveConversation(conversationId)` | Client -> Server | Leave a conversation room |
| `SendMessage(bookingRequestId, content)` | Client -> Server | Send a message via SignalR |
| `TypingIndicator(conversationId)` | Client -> Server | Notify typing |
| `ReceiveMessage(message)` | Server -> Client | Receive new message |
| `UserTyping(userId)` | Server -> Client | Receive typing indicator |

> A conversation is created automatically when the first agreement is sent.

---

### Feature 14: Professional Authentication (Password + JWT + Refresh Token)

Password-based login for professionals with short-lived JWT access tokens (15 min) and long-lived refresh tokens (30 days). Supports token refresh rotation for seamless session persistence.

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `/api/professional/auth/login` | None | Login with email + password |
| `POST` | `/api/professional/auth/refresh` | None | Rotate refresh token, get new access token |
| `POST` | `/api/professional/auth/revoke` | JWT | Revoke refresh token (logout) |
| `GET` | `/api/professional/auth/me` | JWT | Get current professional profile |

**Login request:**
```json
{
  "email": "jane@example.com",
  "password": "SecureP@ss123"
}
```

**Login response:**
```json
{
  "accessToken": "eyJhbG...",
  "refreshToken": "base64-random-token...",
  "expiresInSeconds": 900,
  "profile": {
    "userId": "guid",
    "profileId": "guid",
    "fullName": "Jane Doe",
    "email": "jane@example.com",
    "phone": "+919876543210",
    "roles": ["Photographer", "Videographer"],
    "bio": "...",
    "location": "Mumbai",
    "yearsOfExperience": 5,
    "isRegistrationComplete": true
  }
}
```

**Refresh token request:**
```json
{
  "refreshToken": "base64-random-token..."
}
```

**Refresh token response:**
```json
{
  "accessToken": "eyJhbG...(new)",
  "refreshToken": "base64-random-token...(new)",
  "expiresInSeconds": 900
}
```

**Revoke token request (logout):**
```json
{
  "refreshToken": "base64-random-token..."
}
```

**Registration update:** `POST /api/registration/professional/profile` now requires a `password` field (min 8 chars, must include uppercase, lowercase, digit, and special character).

**Token flow:**
1. Register with password -> Login with email + password -> Receive `accessToken` + `refreshToken`
2. Use `accessToken` in `Authorization: Bearer` header for all authenticated requests
3. When `accessToken` expires (15 min), call `/refresh` with the `refreshToken` to get a new pair
4. On logout, call `/revoke` to invalidate the refresh token
5. Store `accessToken` in memory, `refreshToken` in secure storage (e.g. React Native SecureStore)

**JWT claims for professional tokens:** `sub` (userId), `profileId`, `email`, `name`, `user_type` ("Professional"), `roles` (JSON array)

---

### Health Check

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/healthz` | Application + database health status |
