# Feature 7: Professional Detail (Consumer View)

**Role:** Consumer (viewing professional profiles -- public endpoints)
**Auth:** None for viewing; JWT required for shortlisting or booking

## Overview

When a consumer taps a professional from browse/search results, they see a full profile page with bio, experience, rates, active packages with deliverables/pricing, and an Instagram-synced portfolio gallery. From here, consumers can shortlist the professional or initiate a booking.

## Screens

1. **ProfessionalDetailScreen** -- Full profile view with tabs

This is a single screen with multiple sections or tabs.

## Navigation Flow

```
Browse/Search -> ProfessionalDetail -> [ShortlistToggle | CreateBooking (Feature 10)]
ProfessionalDetail -> PackageDetail (expanded view)
ProfessionalDetail -> PortfolioItem (media viewer)
```

---

## Screen 1: ProfessionalDetailScreen

**Route:** `/professionals/:profileId`

### UI Layout

**Header Section:**
- Profile photo (or initials avatar)
- Full name
- Location with map pin icon
- Years of experience badge
- Hourly rate display
- Shortlist heart icon (top-right, toggles save/unsave -- requires auth)

**Bio Section:**
- Full bio text
- Professional roles chips (Photographer, Videographer, etc.)

**Tabs or Sections:**
1. **Packages Tab** -- Active service packages
2. **Portfolio Tab** -- Instagram media gallery

---

### Packages Tab

Shows all active packages with deliverables and pricing.

#### Endpoint

```
GET /api/professionals/{profileId}/packages
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
    "deliverables": [
      {
        "id": "guid",
        "deliverableName": "Photo Album",
        "quantity": 2,
        "configuration": { "leaf_count": 80, "size": "A4" },
        "notes": "Premium quality"
      }
    ],
    "pricing": {
      "pricingType": 0,
      "basePrice": 50000,
      "discountPercentage": 10,
      "finalPrice": 45000,
      "isNegotiable": true,
      "notes": "Full day coverage"
    }
  }
]
```

#### UI Elements per Package Card
- Package name and type badge
- Coverage type
- Description
- Expandable deliverables list:
  - Deliverable name, quantity, config details
- Pricing:
  - Base price (strikethrough if discount)
  - Final price (after discount)
  - "Negotiable" badge if applicable
  - Pricing type (Fixed, PerHour, etc.)
- **"Book This Package" button** (requires auth -- prompt login if not authenticated)

---

### Portfolio Tab

Shows Instagram-synced portfolio items in a grid.

#### Endpoint

```
GET /api/portfolio/{profileId}?visibleOnly=true
Authorization: None
```

**Response:**

```json
[
  {
    "id": "guid",
    "mediaType": "Image",
    "mediaUrl": "https://...",
    "thumbnailUrl": "https://...",
    "caption": "Beautiful sunset shoot",
    "permalink": "https://instagram.com/p/...",
    "postedAt": "2026-03-20T15:00:00Z",
    "displayOrder": 0,
    "isVisible": true
  }
]
```

#### UI Elements
- 3-column image grid (Instagram-style)
- Video items show play icon overlay
- Carousel items show multi-image icon
- Tapping opens full-screen media viewer:
  - Swipe between items
  - Caption text below
  - "View on Instagram" link (opens permalink)

---

### Profile Detail Endpoint

```
GET /api/professionals/{profileId}/detail
Authorization: None
```

**Response:**

```json
{
  "profileId": "guid",
  "fullName": "Jane Doe",
  "bio": "Professional photographer with 5 years of experience",
  "location": "Mumbai, India",
  "yearsOfExperience": 5,
  "hourlyRate": 75.00,
  "professionalRoles": [0, 1],
  "portfolioUrl": "https://janedoe.com",
  "packageCount": 3,
  "portfolioCount": 25
}
```

---

### Shortlist Toggle

**Save:**

```
POST /api/shortlist/{profileId}
Authorization: Bearer {token}
```

**Remove:**

```
DELETE /api/shortlist/{profileId}
Authorization: Bearer {token}
```

#### UX Notes
- Heart icon: outline = not saved, filled red = saved
- If user is not authenticated, tapping the heart opens a login prompt
- Optimistic UI: toggle heart immediately, revert on API error

---

### "Book This Package" Action

Tapping "Book This Package" navigates to the Create Booking flow (Feature 10) with:
- `professionalProfileId`
- `packageId`
- Package name and price (for display)

If user is not authenticated, redirect to login first, then return to this screen.

---

## State Management

- Fetch profile detail, packages, and portfolio in parallel on screen mount
- Cache profile data for back-navigation
- Track shortlist state locally (sync with API)

## Enum Reference

| Enum | Values |
|------|--------|
| ProfessionalRole | `0`=Photographer, `1`=Videographer, `2`=Editor, `3`=DroneOwner, `4`=DigitalRental |
| PackageType | `0`=Wedding, `1`=PreWedding, `2`=Birthday, `3`=Corporate, `4`=Event, `5`=QuickShoot, `6`=FullDay, `7`=HalfDay, `8`=Custom |
| CoverageType | `0`=OneSide, `1`=BothSides, `2`=Full, `3`=Custom |
| PricingType | `0`=Fixed, `1`=PerHour, `2`=PerDay, `3`=PerEvent, `4`=Custom |
| MediaType | `Image`, `Video`, `CarouselAlbum` |
