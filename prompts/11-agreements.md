# Feature 11: Agreements (Quotation & Negotiation)

**Role:** Both Consumer and Professional
**Auth:** JWT required (all endpoints)

## Overview

After a professional accepts a booking request, they create a formal agreement (quotation) with deliverables, pricing, terms, and conditions. The consumer can request revisions, and the professional re-issues a new version. When the consumer accepts, the project starts immediately (goes Active). Agreements are versioned -- each revision creates a new version, and the full history is viewable.

### Agreement Flow

```
Professional creates agreement (v1) -> Consumer views -> [Accept | Request Revision]
If revision requested -> Professional creates revised agreement (v2) -> Consumer views -> [Accept | Request Revision]
...repeat until accepted
Consumer accepts -> Booking goes Active immediately
```

## Screens

1. **CreateAgreementScreen** -- Professional creates/revises agreement
2. **AgreementDetailScreen** -- View current agreement (both parties)
3. **AgreementHistoryScreen** -- View all versions
4. **RequestRevisionModal** -- Consumer enters revision notes

## Navigation Flow

```
BookingDetail (Feature 10) -> CreateAgreement (Professional, status=PendingQuotation)
BookingDetail (Feature 10) -> AgreementDetail (Both, status=QuotationSent+)
AgreementDetail -> RequestRevision (Consumer)
AgreementDetail -> AgreementHistory (Both)
BookingDetail -> CreateAgreement (Professional, status=RevisionRequested, to revise)
```

---

## Screen 1: CreateAgreementScreen (Professional)

**Route:** `/bookings/:bookingId/agreement/create`

### UI Elements
- Header: "Create Agreement" or "Revise Agreement"
- **Package Snapshot** section:
  - JSON input or pre-filled from booking's package (name, type, coverage)
- **Event Details** section:
  - Date picker: Event Date (pre-filled from booking)
  - Text input: Event Location (pre-filled)
  - Text area: Event Description
- **Pricing** section:
  - Number input: Total Price (required)
- **Terms & Conditions** section:
  - Text area: Terms (e.g., "50% advance, 50% on delivery")
  - Text area: Conditions (e.g., "Weather permitting")
  - Text area: Notes
- **Deliverables** section:
  - List of added deliverables, each with:
    - Text input: Deliverable Name
    - Number input: Quantity
    - Dynamic key-value: Configuration
    - Text area: Notes
  - "+ Add Deliverable" button
  - Swipe to remove deliverable
- Button: "Send Agreement"

### Endpoints

**Create (first time, booking in PendingQuotation):**

```
POST /api/bookings/{bookingId}/agreement
Authorization: Bearer {token}
```

**Revise (booking in RevisionRequested):**

```
PUT /api/bookings/{bookingId}/agreement/revise
Authorization: Bearer {token}
```

**Request Body (same for both):**

```json
{
  "bookingRequestId": "{{bookingId}}",
  "packageSnapshot": "{\"name\":\"Premium Wedding\",\"type\":\"Wedding\",\"coverage\":\"BothSides\"}",
  "eventDate": "2026-04-15T10:00:00Z",
  "eventLocation": "Mumbai, India",
  "eventDescription": "Grand wedding ceremony at beachside venue",
  "totalPrice": 75000,
  "terms": "50% advance, 50% on delivery",
  "conditions": "Weather permitting for outdoor shoots",
  "notes": "Includes drone coverage for aerial shots",
  "deliverables": [
    {
      "deliverableName": "Photo Album",
      "quantity": 2,
      "configuration": { "leaf_count": 80, "size": "A4" },
      "notes": "Premium quality hardcover"
    },
    {
      "deliverableName": "Edited Photos",
      "quantity": 500,
      "configuration": null,
      "notes": "Color corrected and retouched"
    },
    {
      "deliverableName": "Highlight Video",
      "quantity": 1,
      "configuration": { "duration_minutes": 5 },
      "notes": "Cinematic wedding highlight"
    }
  ]
}
```

**Response:**

```json
{
  "id": "guid",
  "bookingRequestId": "guid",
  "version": 1,
  "packageSnapshot": "...",
  "eventDate": "2026-04-15T10:00:00Z",
  "eventLocation": "Mumbai, India",
  "eventDescription": "Grand wedding ceremony",
  "totalPrice": 75000,
  "terms": "50% advance, 50% on delivery",
  "conditions": "Weather permitting",
  "notes": "Includes drone coverage",
  "status": "Sent",
  "deliverables": [...],
  "createdAtUtc": "2026-03-25T12:00:00Z"
}
```

### UX Notes
- When revising, pre-fill all fields from the previous version
- Highlight changes from the previous version (if possible)
- Validate at least one deliverable is added
- Total price must be > 0
- On success, booking transitions to `QuotationSent`, navigate back to BookingDetail
- A conversation is automatically created (server-side) for chat

---

## Screen 2: AgreementDetailScreen (Both)

**Route:** `/bookings/:bookingId/agreement`

### UI Elements
- **Version badge:** "Version 2" (if revised)
- **Status badge:** Draft / Sent / Accepted / Rejected / Superseded
- **Event Details card:**
  - Date, location, description
- **Pricing card:**
  - Total price (large, prominent)
- **Deliverables list:**
  - Each deliverable: name, quantity, config details, notes
- **Terms & Conditions:**
  - Collapsible sections for terms, conditions, notes
- **Action buttons** (conditional):

| Viewer | Agreement Status | Actions |
|--------|-----------------|---------|
| Consumer | Sent | "Accept Agreement", "Request Revision" |
| Professional | Sent | View only (waiting for consumer) |
| Both | Accepted | View only (project is active) |

- "View History" link at bottom

### Endpoint

```
GET /api/bookings/{bookingId}/agreement
Authorization: Bearer {token}
```

---

## Consumer: Accept Agreement

```
POST /api/bookings/{bookingId}/agreement/accept
Authorization: Bearer {token}
```

No request body needed (consumerId from JWT).

**Response:** Agreement with status `Accepted`

### UX Notes
- Confirmation modal: "Accept this agreement? The project will start immediately."
- Show the total price prominently in the confirmation
- On success: booking goes `Accepted` then immediately `Active`
- Navigate to BookingDetail which now shows Active status
- A success toast: "Agreement accepted! Project is now active."

---

## Consumer: Request Revision

### UI: RequestRevisionModal

- Text area: Revision Notes (required, min 10 characters)
  - Placeholder: "Describe what changes you'd like..."
- Buttons: "Cancel" / "Request Revision"

### Endpoint

```
POST /api/bookings/{bookingId}/agreement/request-revision
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "revisionNotes": "Can we add drone coverage and increase photo count to 600?"
}
```

### UX Notes
- On success: booking goes to `RevisionRequested`, agreement marked `Rejected`
- A system message is sent in the chat with the revision notes
- Navigate back to BookingDetail, which now shows `RevisionRequested` status
- Professional will see the revision notes and can create a new version

---

## Screen 3: AgreementHistoryScreen (Both)

**Route:** `/bookings/:bookingId/agreement/history`

### UI Elements
- Header: "Agreement History"
- List of all agreement versions (newest first):
  - Version number
  - Status badge
  - Total price
  - Created date
  - Tap to view full details
- Each version card is expandable to show deliverables

### Endpoint

```
GET /api/bookings/{bookingId}/agreement/history
Authorization: Bearer {token}
```

**Response:**

```json
[
  {
    "id": "guid",
    "version": 2,
    "status": "Sent",
    "totalPrice": 80000,
    "deliverables": [...],
    "createdAtUtc": "2026-03-26T10:00:00Z"
  },
  {
    "id": "guid",
    "version": 1,
    "status": "Superseded",
    "totalPrice": 75000,
    "deliverables": [...],
    "createdAtUtc": "2026-03-25T12:00:00Z"
  }
]
```

### UX Notes
- Show a diff indicator if price changed between versions
- Superseded versions should appear slightly dimmed
- Current (latest) version should be highlighted

---

## State Management

- Fetch agreement on BookingDetail mount (if status >= QuotationSent)
- Refetch after accept/revision actions
- Agreement history is loaded on demand

## Enum Reference

| Enum | Values |
|------|--------|
| AgreementStatus | `0`=Draft, `1`=Sent, `2`=Accepted, `3`=Rejected, `4`=Superseded |
| BookingStatus | `0`=Requested, `1`=PendingQuotation, `2`=QuotationSent, `3`=RevisionRequested, `4`=Accepted, `5`=Active, `6`=Completed, `7`=Declined, `8`=Cancelled |
