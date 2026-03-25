# Feature 8: Shortlist / Save Professionals

**Role:** Consumer
**Auth:** JWT required (all endpoints)

## Overview

Consumers can save professionals to a shortlist for later viewing. The shortlist is accessible from the consumer's profile/settings area. Professionals can be saved/unsaved from their detail page or from browse results.

## Screens

1. **ShortlistScreen** -- List of all saved professionals

The save/unsave toggle also appears on:
- ProfessionalDetailScreen (Feature 7) -- heart icon
- BrowseScreen (Feature 6) -- optional heart on each card

## Navigation Flow

```
Settings/Profile -> ShortlistScreen -> ProfessionalDetail (Feature 7)
```

---

## Screen 1: ShortlistScreen

**Route:** `/shortlist`

### UI Elements
- Header: "Saved Professionals"
- List of saved professional cards:
  - Profile photo (or initials)
  - Full name
  - Location
  - Professional roles chips
  - Hourly rate
  - Red filled heart icon (tap to remove)
  - Tap card to view detail
- Empty state: "No saved professionals yet. Browse and save professionals you like!"
- Pull-to-refresh

### Endpoint

```
GET /api/shortlist
Authorization: Bearer {token}
```

**Response:**

```json
[
  {
    "id": "guid",
    "professionalProfileId": "guid",
    "professionalName": "Jane Doe",
    "location": "Mumbai, India",
    "hourlyRate": 75.00,
    "professionalRoles": [0, 1],
    "savedAtUtc": "2026-03-25T10:00:00Z"
  }
]
```

### UX Notes
- Tapping a card navigates to ProfessionalDetailScreen (Feature 7) with `profileId`
- Tapping the heart icon removes from shortlist with confirmation:
  - "Remove Jane Doe from your shortlist?"
  - Buttons: "Cancel" / "Remove"
- Use optimistic removal: remove from list immediately, add back on API error
- Sort by most recently saved (newest first)

---

## Save / Remove Endpoints

**Save a professional:**

```
POST /api/shortlist/{profileId}
Authorization: Bearer {token}
Response: 201 Created
```

**Remove a professional:**

```
DELETE /api/shortlist/{profileId}
Authorization: Bearer {token}
Response: 204 No Content
```

### Integration with Other Screens

On BrowseScreen and ProfessionalDetailScreen:
- Fetch shortlist IDs on screen mount to know which professionals are saved
- Or maintain a local set of saved profileIds in app state
- Toggle heart: call POST (save) or DELETE (remove) accordingly

---

## State Management

- Maintain a `Set<profileId>` of saved professionals in React Context or Redux
- On app launch (after auth), fetch full shortlist to populate the set
- On save/remove, update the set optimistically and sync with API
- This allows the heart icon to show correct state across all screens

## Enum Reference

| Enum | Values |
|------|--------|
| ProfessionalRole | `0`=Photographer, `1`=Videographer, `2`=Editor, `3`=DroneOwner, `4`=DigitalRental |
