# Feature 9: User Management

**Role:** Admin / Internal (not consumer-facing)
**Auth:** None (no admin auth implemented yet)

## Overview

Administrative interface to list, filter, view, update, and deactivate users in the system. This is an internal management feature, not exposed to regular consumers or professionals. In an MVP, this could be a simple admin panel or a separate admin section within the app.

## Screens

1. **UserListScreen** -- Paginated list of all users with filters
2. **UserDetailScreen** -- View and edit a single user
3. **ProfessionalsByRoleScreen** -- Filter professionals by role

## Navigation Flow

```
UserList -> UserDetail -> [Edit | Deactivate]
UserList -> ProfessionalsByRole
```

---

## Screen 1: UserListScreen

**Route:** `/admin/users`

### UI Elements
- Header: "User Management"
- Filter tabs: "All" | "Users" | "Professionals"
- Search bar (future -- filter by name/phone locally)
- List of user cards:
  - Full name
  - User type badge (User / ProfessionalUser)
  - Phone number
  - Status badge (Active / Inactive)
  - Created date
  - Tap to view detail
- Pull-to-refresh

### Endpoints

```
GET /api/users                            -- All users
GET /api/users/type/{userType}            -- Filter by type (User / ProfessionalUser)
GET /api/users/professionals/{role}       -- Filter by role
```

**All Users Response:**

```json
[
  {
    "id": "guid",
    "fullName": "Jane Doe",
    "email": "jane@example.com",
    "phone": "+919876543210",
    "userType": "ProfessionalUser",
    "isActive": true,
    "createdAtUtc": "2026-03-25T10:00:00Z"
  }
]
```

### UX Notes
- Tab switching changes the API call (all / by type)
- Professional role filter could be a dropdown when "Professionals" tab is active

---

## Screen 2: UserDetailScreen

**Route:** `/admin/users/:userId`

### UI Elements
- User info card: name, email, phone, type, status, created date
- If professional: list of roles, profile link
- "Edit Profile" button -> inline edit form or modal
- "Deactivate" button (with confirmation dialog)

### Endpoints

```
GET /api/users/{id}                       -- Get user detail
PUT /api/users/{id}/profile               -- Update profile
POST /api/users/{id}/deactivate           -- Deactivate user
```

**Update Profile Request:**

```json
{
  "fullName": "Jane Doe Updated",
  "phone": "+919876543211"
}
```

**Deactivate:** No body needed. Returns 200.

### UX Notes
- Confirm before deactivation: "Are you sure you want to deactivate this user? They will not be able to log in."
- Show a "Deactivated" banner if user is already inactive
- Deactivated users should not appear in browse/search results

---

## State Management

- User list is ephemeral -- refetch on focus
- No local caching needed for admin views

## Enum Reference

| Enum | Values |
|------|--------|
| UserType | `User`, `ProfessionalUser` |
| ProfessionalRole | `Photographer`, `Videographer`, `Editor`, `DroneOwner`, `DigitalRental` |
