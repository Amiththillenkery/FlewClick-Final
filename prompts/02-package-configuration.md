# Feature 2: Package Configuration

**Role:** Professional (Photographers, Videographers, Editors, Drone Owners)
**Auth:** None (professional auth not yet implemented)

## Overview

Professionals create service packages for their offerings. Each package has a type (Wedding, PreWedding, etc.), deliverables selected from a master catalog, and pricing. Packages can be activated/deactivated and managed over time.

## Screens

1. **PackageListScreen** -- List all packages for the professional
2. **CreatePackageScreen** -- Form to create a new package
3. **PackageDetailScreen** -- View package with deliverables and pricing
4. **DeliverableCatalogScreen** -- Browse master deliverables for a role
5. **AddDeliverableScreen** -- Add a deliverable to a package with quantity and config
6. **SetPricingScreen** -- Configure pricing for a package

## Navigation Flow

```
PackageList -> CreatePackage -> PackageDetail -> [AddDeliverable | SetPricing] -> PackageDetail
```

---

## Screen 1: PackageListScreen

**Route:** `/packages/:profileId`

### UI Elements
- Header: "My Packages"
- FAB or header button: "+ New Package"
- List of package cards showing:
  - Package name
  - Package type badge (Wedding, PreWedding, etc.)
  - Status: Active (green) / Inactive (grey)
  - Price (if set)
  - Tap to view details
- Empty state: "No packages yet. Create your first package!"

### Endpoint

```
GET /api/packages/profile/{profileId}
Authorization: None
```

**Response:**

```json
[
  {
    "id": "guid",
    "name": "Premium Wedding Package",
    "packageType": 0,
    "description": "Complete wedding coverage",
    "coverageType": 1,
    "isActive": true,
    "createdAtUtc": "2026-03-25T10:00:00Z"
  }
]
```

---

## Screen 2: CreatePackageScreen

**Route:** `/packages/create`

### UI Elements
- Text input: Package Name (required)
- Dropdown/picker: Package Type (required)
- Text area: Description (optional)
- Dropdown/picker: Coverage Type (required)
- Hidden field: profileId (from navigation params)
- Button: "Create Package"

### Endpoint

```
POST /api/packages
Authorization: None
```

**Request Body:**

```json
{
  "profileId": "{{profileId}}",
  "name": "Premium Wedding Package",
  "packageType": 0,
  "description": "Complete wedding coverage",
  "coverageType": 1
}
```

**Response (201):** Created package object with `id`

### UX Notes
- On success, navigate to PackageDetailScreen with the new package ID
- Validate package name is not empty

---

## Screen 3: PackageDetailScreen

**Route:** `/packages/:packageId`

### UI Elements
- Package name and type header
- Status toggle (Active/Inactive)
- **Deliverables section:**
  - List of added deliverables with name, quantity, config summary
  - "+ Add Deliverable" button
  - Swipe to delete deliverable
- **Pricing section:**
  - Shows pricing if set (base price, discount, type)
  - "Set Pricing" or "Update Pricing" button
- Edit and Delete buttons in header

### Endpoints Used

```
GET /api/packages/{id}                          -- Package details
GET /api/packages/{packageId}/deliverables      -- List deliverables
GET /api/packages/{packageId}/pricing           -- Get pricing
PATCH /api/packages/{id}/status                 -- Toggle active/inactive
DELETE /api/packages/{id}                       -- Delete package
PUT /api/packages/{id}                          -- Update package
DELETE /api/packages/{packageId}/deliverables/{id}  -- Remove deliverable
DELETE /api/packages/{packageId}/pricing        -- Remove pricing
```

---

## Screen 4: DeliverableCatalogScreen

**Route:** `/deliverables/catalog/:role`

### UI Elements
- List of master deliverables for the professional's role
- Each item shows: name, description, default configuration options
- Tap to select and navigate to AddDeliverableScreen

### Endpoint

```
GET /api/deliverables/master/{role}
Authorization: None
```

Role values: `Photographer`, `Videographer`, `Editor`, `DroneOwner`

---

## Screen 5: AddDeliverableScreen

**Route:** `/packages/:packageId/deliverables/add`

### UI Elements
- Selected deliverable name (read-only)
- Number input: Quantity (required, min 1)
- Dynamic config fields based on deliverable type (e.g., leaf_count, size for albums)
- Text area: Notes (optional)
- Button: "Add to Package"

### Endpoint

```
POST /api/packages/{packageId}/deliverables
Authorization: None
```

**Request Body:**

```json
{
  "deliverableMasterId": "{{masterId}}",
  "quantity": 2,
  "configuration": { "leaf_count": 80, "size": "A4" },
  "notes": "Premium quality album"
}
```

---

## Screen 6: SetPricingScreen

**Route:** `/packages/:packageId/pricing`

### UI Elements
- Dropdown/picker: Pricing Type (Fixed, PerHour, PerDay, PerEvent, Custom)
- Number input: Base Price (required)
- Number input: Discount Percentage (optional, 0-100)
- Number input: Duration Hours (optional, for PerHour type)
- Switch: Is Negotiable
- Text area: Notes (optional)
- Button: "Save Pricing"

### Endpoint

```
POST /api/packages/{packageId}/pricing
Authorization: None
```

**Request Body:**

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

---

## State Management

- Store `profileId` to fetch packages
- Refetch package list on screen focus (after create/edit)
- Cache deliverable catalog locally (it rarely changes)

## Enum Reference

| Enum | Values |
|------|--------|
| PackageType | `0`=Wedding, `1`=PreWedding, `2`=Birthday, `3`=Corporate, `4`=Event, `5`=QuickShoot, `6`=FullDay, `7`=HalfDay, `8`=Custom |
| CoverageType | `0`=OneSide, `1`=BothSides, `2`=Full, `3`=Custom |
| PricingType | `0`=Fixed, `1`=PerHour, `2`=PerDay, `3`=PerEvent, `4`=Custom |
