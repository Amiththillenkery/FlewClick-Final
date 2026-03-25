# Feature 10: Booking Requests

**Role:** Both Consumer and Professional
**Auth:** JWT required (all endpoints)

## Overview

The booking system is the core of FlewClick. A consumer selects a professional's package and creates a booking request. The professional can accept (if no outstanding platform fees), decline, or the request can be cancelled. After acceptance, the flow moves to agreement negotiation (Feature 11). After agreement acceptance, the project becomes active, and when completed, a 3% platform fee is created.

### Booking State Machine

```
Requested -> PendingQuotation -> QuotationSent -> Accepted -> Active -> Completed
                                                                    \-> Cancelled
Requested -> Declined
Requested -> Cancelled
```

### Fee Blocking Rules

- Incoming requests are **never** blocked -- professionals can always see them
- The **accept action** is the only thing blocked when unpaid platform fees exist from completed projects
- Already active/in-progress projects are never affected
- Once all fees are paid, the professional can accept new requests again

## Screens

### Consumer Screens
1. **CreateBookingScreen** -- Select event date, location, notes for a package
2. **MyBookingsScreen** -- List of consumer's bookings with status
3. **BookingDetailScreen** -- Full booking detail (shared with professional)

### Professional Screens
4. **IncomingBookingsScreen** -- List of incoming booking requests
5. **BookingDetailScreen** -- Same screen, different actions based on role
6. **DeclineBookingModal** -- Enter decline reason
7. **CancelBookingModal** -- Enter cancel reason

## Navigation Flow

### Consumer Flow
```
ProfessionalDetail (Feature 7) -> CreateBooking -> MyBookings
MyBookings -> BookingDetail -> [Agreement (Feature 11) | Chat (Feature 13) | Cancel]
```

### Professional Flow
```
Dashboard -> IncomingBookings -> BookingDetail -> [Accept | Decline | Agreement (Feature 11) | Chat (Feature 13) | Complete]
```

---

## Screen 1: CreateBookingScreen (Consumer)

**Route:** `/bookings/create`

### Prerequisites
- Consumer must be authenticated
- Navigated from ProfessionalDetailScreen with `professionalProfileId` and `packageId`

### UI Elements
- Header: "Book [Professional Name]"
- Package summary card (read-only): name, price, deliverables summary
- Date picker: Event Date (required, must be future)
- Text input: Event Location (optional)
- Text area: Notes / Special Requests (optional)
- Price display (from package)
- Button: "Send Booking Request"

### Endpoint

```
POST /api/bookings
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "professionalProfileId": "{{profileId}}",
  "packageId": "{{packageId}}",
  "eventDate": "2026-04-15T10:00:00Z",
  "eventLocation": "Mumbai, India",
  "notes": "Outdoor wedding ceremony, need drone shots"
}
```

**Response (201):**

```json
{
  "id": "guid",
  "consumerId": "guid",
  "professionalProfileId": "guid",
  "packageId": "guid",
  "eventDate": "2026-04-15T10:00:00Z",
  "eventLocation": "Mumbai, India",
  "notes": "Outdoor wedding ceremony, need drone shots",
  "status": "Requested",
  "createdAtUtc": "2026-03-25T10:00:00Z"
}
```

### UX Notes
- `consumerId` is extracted from JWT on the server -- not sent in body
- Event date must be in the future -- validate client-side and show error if past
- On success, navigate to MyBookingsScreen with a success toast
- Show package price as informational -- final price is set in the agreement

---

## Screen 2: MyBookingsScreen (Consumer)

**Route:** `/bookings/my`

### UI Elements
- Header: "My Bookings"
- Tab filters: All | Active | Completed | Cancelled
- List of booking cards:
  - Professional name
  - Package name
  - Event date
  - Status badge with color:
    - Requested (blue), PendingQuotation (orange), QuotationSent (purple)
    - Accepted/Active (green), Completed (teal), Declined (red), Cancelled (grey)
  - Tap to view detail
- Empty state: "No bookings yet. Browse professionals to get started!"
- Pull-to-refresh

### Endpoint

```
GET /api/bookings/my
Authorization: Bearer {token}
```

**Response:**

```json
[
  {
    "id": "guid",
    "consumerId": "guid",
    "professionalProfileId": "guid",
    "packageId": "guid",
    "eventDate": "2026-04-15T10:00:00Z",
    "eventLocation": "Mumbai, India",
    "status": "Requested",
    "createdAtUtc": "2026-03-25T10:00:00Z"
  }
]
```

---

## Screen 3: IncomingBookingsScreen (Professional)

**Route:** `/bookings/incoming`

### UI Elements
- Header: "Incoming Requests"
- Outstanding fees banner (if blocked):
  - "You have outstanding platform fees. Pay to accept new bookings."
  - "Pay Now" button -> Platform Fees (Feature 12)
- List of booking request cards:
  - Consumer name
  - Package name
  - Event date
  - Status badge
  - "Accept" and "Decline" buttons on Requested items
  - Tap to view detail
- Empty state: "No incoming requests"

### Endpoint

```
GET /api/bookings/incoming?profileId={profileId}
Authorization: Bearer {token}
```

### UX Notes
- If `CheckOutstandingFees` returns `isBlocked: true`, show the warning banner prominently
- Accept/Decline buttons only show for bookings in `Requested` status
- Show a date proximity indicator (e.g., "Event in 5 days")

---

## Screen 4: BookingDetailScreen (Both)

**Route:** `/bookings/:bookingId`

### UI Elements
- **Status bar** at top with current status and colored indicator
- **Booking info:**
  - Professional name (consumer view) / Consumer name (professional view)
  - Package name
  - Event date
  - Event location
  - Notes
  - Created date
- **Timeline** showing status history (from BookingStatusHistory)
- **Action buttons** (conditional on status and role):

| Status | Consumer Actions | Professional Actions |
|--------|-----------------|---------------------|
| Requested | Cancel | Accept, Decline |
| PendingQuotation | Cancel | Create Agreement (Feature 11) |
| QuotationSent | View Agreement, Request Revision, Accept Agreement, Cancel | View Agreement |
| Active | Chat, Cancel | Chat, Complete, Cancel |
| Completed | -- | Pay Fee (if pending) |
| Declined/Cancelled | -- | -- |

- **Chat button** (available from PendingQuotation onwards) -> Chat (Feature 13)
- **Agreement section** (visible from QuotationSent onwards) -> Agreement (Feature 11)

### Endpoint

```
GET /api/bookings/{id}
Authorization: Bearer {token}
```

**Response:**

```json
{
  "booking": {
    "id": "guid",
    "consumerId": "guid",
    "professionalProfileId": "guid",
    "packageId": "guid",
    "eventDate": "2026-04-15T10:00:00Z",
    "eventLocation": "Mumbai, India",
    "notes": "...",
    "status": "Active",
    "declineReason": null,
    "cancellationReason": null,
    "cancelledBy": null,
    "createdAtUtc": "2026-03-25T10:00:00Z"
  },
  "consumerName": "John Consumer",
  "professionalName": "Jane Doe",
  "packageName": "Premium Wedding Package"
}
```

---

## Accept Booking (Professional)

```
POST /api/bookings/{id}/accept
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "professionalProfileId": "{{profileId}}"
}
```

**Success (200):** Updated booking with status `PendingQuotation`

**Error (400) -- Outstanding fees:**

```json
{
  "error": "You have outstanding platform fees from completed projects. Please pay before accepting new bookings."
}
```

### UX Notes
- On fee-blocked error, show a modal/dialog:
  - "Outstanding Fees"
  - "You have unpaid platform fees from completed projects. Pay now to accept new bookings."
  - "Pay Now" button -> navigates to Platform Fees (Feature 12)
  - "Later" button -> dismiss

---

## Decline Booking (Professional)

```
POST /api/bookings/{id}/decline
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "professionalProfileId": "{{profileId}}",
  "reason": "Schedule conflict on that date"
}
```

### UI: DeclineBookingModal
- Text area: Reason (required, min 10 characters)
- Buttons: "Cancel" / "Decline Booking"

---

## Cancel Booking (Both)

```
POST /api/bookings/{id}/cancel
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "cancelledById": "{{userId or profileId}}",
  "cancelledByType": 0,
  "reason": "Plans changed, no longer need the service"
}
```

`cancelledByType`: `0`=Consumer, `1`=Professional

### UI: CancelBookingModal
- Warning text: "Are you sure you want to cancel this booking?"
- Text area: Reason (required)
- Buttons: "Go Back" / "Cancel Booking" (destructive red)

---

## Complete Booking (Professional)

```
POST /api/bookings/{id}/complete
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "professionalProfileId": "{{profileId}}"
}
```

**Response:** Updated booking with status `Completed`

### UX Notes
- Confirm: "Mark this project as completed? A 3% platform fee will be created."
- On success, show the fee amount and link to pay
- This creates a `PlatformFeePayment` (3% of agreement amount, due in 15 days)

---

## State Management

- Consumer bookings: fetch on MyBookingsScreen focus, cache for back-nav
- Professional incoming: fetch on IncomingBookingsScreen focus
- Booking detail: fetch fresh each time (status may have changed)
- Outstanding fees check: fetch on IncomingBookingsScreen mount

## Enum Reference

| Enum | Values |
|------|--------|
| BookingStatus | `0`=Requested, `1`=PendingQuotation, `2`=QuotationSent, `3`=RevisionRequested, `4`=Accepted, `5`=Active, `6`=Completed, `7`=Declined, `8`=Cancelled |
| MessageSenderType | `0`=Consumer, `1`=Professional, `2`=System |
