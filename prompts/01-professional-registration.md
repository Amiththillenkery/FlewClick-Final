# Feature 1: Professional Registration

**Role:** Professional (new user onboarding)
**Auth:** None (public registration flow)

## Overview

Multi-step onboarding wizard that registers a professional user, creates their profile, and walks them through role-specific configuration screens. The professional selects one or more roles (Photographer, Videographer, Editor, Drone Owner, Digital Rental) and must complete configuration for each selected role before finalizing registration.

## Screens

1. **ProfileCreationScreen** -- Collect personal info and select professional roles
2. **RegistrationStatusScreen** -- Dashboard showing which role configs are pending/complete
3. **PhotographyConfigScreen** -- Configure photography/videography settings
4. **EditingConfigScreen** -- Configure editing settings
5. **DroneConfigScreen** -- Configure drone settings
6. **RentalEquipmentConfigScreen** -- Add rental equipment info
7. **RegistrationCompleteScreen** -- Success confirmation

## Navigation Flow

```
ProfileCreation -> RegistrationStatus -> [ConfigScreens based on roles] -> RegistrationStatus -> Complete
```

The user bounces between RegistrationStatus and individual config screens until all roles are configured.

---

## Screen 1: ProfileCreationScreen

**Route:** `/registration/profile`

### UI Elements
- Text input: Full Name (required)
- Text input: Email (required, email validation)
- Text input: Phone (required, format +91XXXXXXXXXX)
- Multi-select chips: Professional Roles (at least one required)
  - Photographer (0), Videographer (1), Editor (2), Drone Owner (3), Digital Rental (4)
- Text area: Bio (optional)
- Text input: Location (required)
- Number input: Years of Experience (required, min 0)
- Number input: Hourly Rate (required, min 0)
- Text input: Portfolio URL (optional)
- Button: "Create Profile"

### Endpoint

```
POST /api/registration/professional/profile
Authorization: None
```

**Request Body:**

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

**Response (201 Created):**

```json
{
  "userId": "guid",
  "profileId": "guid",
  "fullName": "Jane Doe",
  "professionalRoles": [0, 1],
  "registrationStatus": "Pending"
}
```

### UX Notes
- Store the returned `profileId` -- it is used in every subsequent step
- Validate phone format client-side before submission
- Role chips should visually indicate selected state
- On success, navigate to RegistrationStatusScreen

### States
- **Loading:** Spinner overlay on button press
- **Error:** Show validation errors inline next to fields. Show API error as toast/banner
- **Success:** Navigate to RegistrationStatusScreen with profileId

---

## Screen 2: RegistrationStatusScreen

**Route:** `/registration/status/:profileId`

### UI Elements
- Header: "Complete Your Registration"
- List of role configuration cards, each showing:
  - Role name (e.g., "Photography Configuration")
  - Status badge: "Pending" (orange) or "Completed" (green)
  - Tap to navigate to config screen
- Button: "Complete Registration" (enabled only when ALL configs show "Completed")

### Endpoint

```
GET /api/registration/professional/{profileId}/status
Authorization: None
```

**Response:**

```json
{
  "profileId": "guid",
  "isComplete": false,
  "pendingConfigurations": ["Photography", "Videography"],
  "completedConfigurations": []
}
```

### UX Notes
- Poll or refetch status each time this screen gains focus (e.g., returning from a config screen)
- "Complete Registration" button is disabled/greyed out until `isComplete` is true
- Each card navigates to the appropriate config screen

### Complete Registration Endpoint

```
POST /api/registration/professional/{profileId}/complete
Authorization: None
```

**Response (200):**

```json
{
  "profileId": "guid",
  "registrationStatus": "Completed"
}
```

### States
- **Loading:** Skeleton cards while fetching status
- **All Pending:** Show all cards with orange badges, button disabled
- **All Complete:** Show all cards green, button enabled and prominent
- **Error on complete:** Show error toast (e.g., "Not all configurations are complete")

---

## Screen 3: PhotographyConfigScreen

**Route:** `/registration/config/photography/:profileId`

### UI Elements
- Form fields for photography/videography settings (camera types, equipment, specializations)
- Button: "Save Configuration"

### Endpoint

```
POST /api/registration/professional/{profileId}/photography
Authorization: None
```

**Request Body:** Role-specific configuration fields (camera type, lenses, specializations, etc.)

### UX Notes
- On success, navigate back to RegistrationStatusScreen
- This screen is shared for both Photographer and Videographer roles

---

## Screen 4: EditingConfigScreen

**Route:** `/registration/config/editing/:profileId`

### Endpoint

```
POST /api/registration/professional/{profileId}/editing
Authorization: None
```

---

## Screen 5: DroneConfigScreen

**Route:** `/registration/config/drone/:profileId`

### Endpoint

```
POST /api/registration/professional/{profileId}/drone
Authorization: None
```

---

## Screen 6: RentalEquipmentConfigScreen

**Route:** `/registration/config/rental/:profileId`

### Endpoint

```
POST /api/registration/professional/{profileId}/rental-equipment
Authorization: None
```

---

## Screen 7: RegistrationCompleteScreen

**Route:** `/registration/complete`

### UI Elements
- Success illustration/animation
- "Your registration is complete!" message
- "Go to Dashboard" button

---

## State Management

- Store `profileId` in navigation params or async storage after creation
- No JWT is involved -- professional auth is not yet implemented
- Refetch registration status on screen focus

## Enum Reference

| Enum | Values |
|------|--------|
| ProfessionalRole | `0`=Photographer, `1`=Videographer, `2`=Editor, `3`=DroneOwner, `4`=DigitalRental |
| RegistrationStatus | `Pending`, `Completed` |
