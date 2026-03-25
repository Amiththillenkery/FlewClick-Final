# Feature 12: Platform Fees (Post-Completion Billing)

**Role:** Professional
**Auth:** JWT required (except Razorpay webhook)

## Overview

When a professional marks a project as completed, a 3% platform fee is automatically created. The professional must pay outstanding fees before accepting new booking requests. Payment is done via Razorpay checkout. This feature provides a dashboard to view outstanding fees, initiate payment, and view payment history.

### Fee Blocking Mechanics

- Fees are created when a booking transitions to `Completed`
- Fee amount = 3% of agreement's total price
- Fee is due within 15 days (grace period)
- The professional can still **see** incoming booking requests
- The professional is **blocked from accepting** new requests while fees are unpaid
- Already active projects are never affected
- Once all fees are paid, the professional can accept new bookings

### Timeline Example

```
Mar 25  Accept Order A                    (no fees, allowed)
Mar 26  Accept Order B                    (Order A still active, allowed)
Mar 29  Complete Order A                  (3% fee created = pending)
Mar 30  Try to accept Order C             (BLOCKED -- Order A fee unpaid)
Mar 30  Pay fee via Razorpay              (fee cleared)
Mar 30  Accept Order C                    (allowed)
```

## Screens

1. **OutstandingFeesScreen** -- Dashboard showing unpaid fees
2. **PaymentScreen** -- Razorpay checkout flow
3. **PaymentHistoryScreen** -- All fee payments (paid + pending)

## Navigation Flow

```
Dashboard/IncomingBookings -> OutstandingFees -> Payment -> OutstandingFees (cleared)
Dashboard -> PaymentHistory
```

Also triggered from:
- IncomingBookingsScreen (Feature 10) when fee banner is shown
- Accept booking error modal "Pay Now" button

---

## Screen 1: OutstandingFeesScreen (Professional)

**Route:** `/platform-fees/outstanding`

### UI Elements
- Header: "Outstanding Fees"
- **Summary card at top:**
  - Blocked status indicator: red "Blocked" or green "Clear"
  - Total outstanding amount (large text)
  - "Pay All" button (if multiple fees)
- **List of unpaid fees:**
  - Each fee card shows:
    - Booking reference (booking ID or event date)
    - Agreement amount
    - Fee percentage (3%)
    - Fee amount
    - Status badge: Pending (orange) / Processing (blue)
    - Due date (with "overdue" red text if past)
    - "Pay" button
- Empty state: "No outstanding fees. You're all clear!"

### Endpoint

```
GET /api/platform-fees/outstanding?profileId={profileId}
Authorization: Bearer {token}
```

**Response:**

```json
{
  "isBlocked": true,
  "totalOutstanding": 2250.00,
  "fees": [
    {
      "id": "fee-guid",
      "bookingRequestId": "booking-guid",
      "professionalProfileId": "profile-guid",
      "agreementAmount": 75000.00,
      "feePercentage": 3.0,
      "feeAmount": 2250.00,
      "status": "Pending",
      "razorpayOrderId": null,
      "paidAt": null,
      "dueDate": "2026-04-14T00:00:00Z",
      "createdAtUtc": "2026-03-30T10:00:00Z"
    }
  ]
}
```

### UX Notes
- If `isBlocked` is true, show a prominent red banner
- Due date: show as "Due in X days" or "Overdue by X days" with color coding
- "Pay" button on each fee initiates payment for that specific fee

---

## Screen 2: PaymentScreen (Razorpay Checkout)

**Route:** `/platform-fees/pay/:feeId`

### Flow

1. Call initiate endpoint to create a Razorpay order
2. Open Razorpay checkout (React Native SDK) with the order details
3. On payment success, call verify endpoint
4. On payment failure, show error and allow retry

### Step 1: Initiate Payment

```
POST /api/platform-fees/{feeId}/initiate
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "professionalProfileId": "{{profileId}}"
}
```

**Response:**

```json
{
  "orderId": "order_mock_abc123",
  "amount": 2250.00,
  "currency": "INR"
}
```

### Step 2: Open Razorpay Checkout

Use `react-native-razorpay` package:

```javascript
import RazorpayCheckout from 'react-native-razorpay';

const options = {
  description: 'FlewClick Platform Fee',
  image: 'https://flewclick.com/logo.png',
  currency: response.currency,
  key: 'rzp_test_...', // Razorpay key from config
  amount: response.amount * 100, // Amount in paise
  order_id: response.orderId,
  name: 'FlewClick',
  prefill: {
    email: 'professional@email.com',
    contact: '+919876543210',
    name: 'Jane Doe'
  },
  theme: { color: '#F37254' }
};

RazorpayCheckout.open(options)
  .then((data) => {
    // Payment success -- verify
    verifyPayment(data.razorpay_order_id, data.razorpay_payment_id, data.razorpay_signature);
  })
  .catch((error) => {
    // Payment failed
    showError('Payment failed. Please try again.');
  });
```

### Step 3: Verify Payment

```
POST /api/platform-fees/verify
Authorization: None (webhook-style, can also be called from client)
```

**Request Body:**

```json
{
  "razorpayOrderId": "order_mock_abc123",
  "razorpayPaymentId": "pay_abc123",
  "razorpaySignature": "signature_hash"
}
```

**Response:**

```json
{
  "id": "fee-guid",
  "status": "Completed",
  "paidAt": "2026-03-30T11:00:00Z",
  "feeAmount": 2250.00
}
```

### UX Notes
- Show a loading overlay while Razorpay opens
- On success: show success animation, navigate back to OutstandingFeesScreen
- On failure: show error with "Retry" button
- Currently using mock Razorpay -- payment always succeeds in dev

---

## Screen 3: PaymentHistoryScreen (Professional)

**Route:** `/platform-fees/history`

### UI Elements
- Header: "Payment History"
- Filter tabs: All | Pending | Completed
- List of all fee records:
  - Booking reference
  - Agreement amount
  - Fee amount
  - Status badge: Pending (orange) / Completed (green) / Failed (red)
  - Payment date (if paid)
  - Due date
- Summary at top: total fees paid, total pending

### Endpoint

```
GET /api/platform-fees/history?profileId={profileId}
Authorization: Bearer {token}
```

**Response:**

```json
[
  {
    "id": "guid",
    "bookingRequestId": "guid",
    "agreementAmount": 75000.00,
    "feePercentage": 3.0,
    "feeAmount": 2250.00,
    "status": "Completed",
    "paidAt": "2026-03-30T11:00:00Z",
    "dueDate": "2026-04-14T00:00:00Z",
    "createdAtUtc": "2026-03-30T10:00:00Z"
  }
]
```

---

## State Management

- Outstanding fees: fetch on IncomingBookingsScreen and OutstandingFeesScreen mount
- Refetch after successful payment
- Store `isBlocked` state globally to show fee banner on relevant screens
- Payment history: fetch on demand, not cached

## Integration with Other Features

- **Feature 10 (Bookings):** IncomingBookingsScreen shows fee warning banner. Accept action checks fees.
- **Feature 10 (Bookings):** CompleteBooking creates the fee record automatically.
- Accept booking error response includes a link to pay fees.

## Enum Reference

| Enum | Values |
|------|--------|
| PaymentStatus | `0`=Pending, `1`=Processing, `2`=Completed, `3`=Failed, `4`=Refunded |
