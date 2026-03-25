# Feature 6: Browse & Search Professionals

**Role:** Consumer (or any user -- public endpoints)
**Auth:** None (all endpoints are public)

## Overview

The consumer home screen shows professional categories (Photographer, Videographer, etc.) with counts of available professionals. Consumers can browse professionals filtered by role and location, or search by text across names, bios, and locations. This is the primary discovery mechanism in the app.

## Screens

1. **HomeScreen** -- Category cards with counts, search bar
2. **BrowseScreen** -- Filtered list of professionals by category/location
3. **SearchResultsScreen** -- Text search results

## Navigation Flow

```
Home -> [Browse (tap category) | SearchResults (tap search)]
Browse/SearchResults -> ProfessionalDetail (Feature 7)
```

---

## Screen 1: HomeScreen

**Route:** `/home`

### UI Elements
- Search bar at top (tapping opens SearchResultsScreen)
- "Browse by Category" section header
- Grid of category cards (2 columns), each showing:
  - Category icon (camera for Photographer, video for Videographer, etc.)
  - Category name
  - Professional count badge (e.g., "12 professionals")
  - Tap navigates to BrowseScreen with role filter
- Optional: "Featured Professionals" horizontal scroll (future enhancement)

### Endpoint

```
GET /api/browse/categories
Authorization: None
```

**Response:**

```json
[
  { "role": 0, "name": "Photographer", "professionalCount": 12 },
  { "role": 1, "name": "Videographer", "professionalCount": 8 },
  { "role": 2, "name": "Editor", "professionalCount": 5 },
  { "role": 3, "name": "DroneOwner", "professionalCount": 3 },
  { "role": 4, "name": "DigitalRental", "professionalCount": 2 }
]
```

### UX Notes
- Use distinct icons for each category
- Hide categories with 0 professionals or show them greyed out
- Pull-to-refresh to reload category counts
- Search bar should be prominent -- this is the primary action

---

## Screen 2: BrowseScreen

**Route:** `/browse?role={role}`

### UI Elements
- Header: Category name (e.g., "Photographers")
- Location filter (text input or picker at top)
- Paginated list of professional cards:
  - Profile photo placeholder (initials)
  - Full name
  - Location
  - Years of experience
  - Hourly rate
  - Star rating (future)
  - Brief bio snippet (truncated)
- Infinite scroll pagination
- Empty state: "No professionals found in this category"

### Endpoint

```
GET /api/browse/professionals?role={role}&location={location}&page=1&pageSize=20
Authorization: None
```

**Response:**

```json
{
  "items": [
    {
      "profileId": "guid",
      "fullName": "Jane Doe",
      "location": "Mumbai, India",
      "bio": "Professional photographer with 5 years...",
      "yearsOfExperience": 5,
      "hourlyRate": 75.00,
      "profilePhotoUrl": null,
      "packageCount": 3,
      "portfolioCount": 25
    }
  ],
  "totalCount": 12,
  "page": 1,
  "pageSize": 20,
  "totalPages": 1
}
```

### UX Notes
- Tapping a professional card navigates to ProfessionalDetailScreen (Feature 7)
- Location filter: debounce input, refetch on change
- Implement infinite scroll: load next page when user nears bottom
- Show skeleton cards while loading
- Show package count and portfolio count as subtle badges

---

## Screen 3: SearchResultsScreen

**Route:** `/search`

### UI Elements
- Auto-focused search input at top
- Real-time search results as user types (debounced 300ms)
- Optional: role filter dropdown
- Same professional card layout as BrowseScreen
- "Recent searches" when input is empty (stored locally)
- Empty state: "No results for '{query}'"

### Endpoint

```
GET /api/search/professionals?q={query}&role={role}&page=1&pageSize=20
Authorization: None
```

**Response:** Same paginated format as browse endpoint

### UX Notes
- Debounce search input by 300ms to avoid excessive API calls
- Minimum 2 characters before triggering search
- Highlight matching text in results (client-side)
- Store last 5 searches in AsyncStorage for "Recent searches"
- Tapping a result navigates to ProfessionalDetailScreen (Feature 7)

---

## State Management

- Category counts: fetch on app launch, cache, refetch on pull-to-refresh
- Search results: ephemeral, not cached
- Browse results: cache per role+location for back-navigation
- Recent searches: store in AsyncStorage (array of strings, max 5)

## Enum Reference

| Enum | Values |
|------|--------|
| ProfessionalRole | `0`=Photographer, `1`=Videographer, `2`=Editor, `3`=DroneOwner, `4`=DigitalRental |
