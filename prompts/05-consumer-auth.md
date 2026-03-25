# Feature 5: Consumer Authentication (OTP + JWT)

**Role:** Consumer (new user registration and login)
**Auth:** None for registration/login, JWT for profile access

## Overview

Consumers register and log in using their mobile number. The flow is OTP-based: the app sends a phone number, the backend sends an OTP via SMS (currently mocked -- OTP appears in server console logs), the consumer enters the OTP, and a JWT token is returned on success. The JWT is used for all subsequent authenticated consumer requests.

## Screens

1. **WelcomeScreen** -- Entry point with Register / Login options
2. **RegisterScreen** -- Phone + name input for new users
3. **LoginScreen** -- Phone input for returning users
4. **OtpVerificationScreen** -- 6-digit OTP input (shared for both flows)
5. **ProfileScreen** -- View logged-in consumer profile

## Navigation Flow

```
Welcome -> [Register | Login] -> OtpVerification -> Home (authenticated)
Settings -> Profile
```

---

## Screen 1: WelcomeScreen

**Route:** `/welcome`

### UI Elements
- App logo and tagline
- "Register" button (primary)
- "Already have an account? Login" link/button
- No API calls on this screen

---

## Screen 2: RegisterScreen

**Route:** `/auth/register`

### UI Elements
- Text input: Phone Number (required, format +91XXXXXXXXXX)
- Text input: Full Name (required)
- Button: "Send OTP"
- "Already have an account? Login" link

### Endpoint

```
POST /api/auth/register
Authorization: None
```

**Request Body:**

```json
{
  "phone": "+919876543210",
  "fullName": "John Consumer"
}
```

**Response (200):**

```json
{
  "message": "OTP sent successfully",
  "phone": "+919876543210"
}
```

### UX Notes
- Validate phone format: must start with +91, 10 digits after
- On success, navigate to OtpVerificationScreen with `phone`, `fullName`, and `flow: "register"`
- If phone already registered, API returns error -- show "This number is already registered. Please login instead."

---

## Screen 3: LoginScreen

**Route:** `/auth/login`

### UI Elements
- Text input: Phone Number (required)
- Button: "Send OTP"
- "New user? Register" link

### Endpoint

```
POST /api/auth/login
Authorization: None
```

**Request Body:**

```json
{
  "phone": "+919876543210"
}
```

**Response (200):**

```json
{
  "message": "OTP sent successfully",
  "phone": "+919876543210"
}
```

### UX Notes
- On success, navigate to OtpVerificationScreen with `phone` and `flow: "login"`
- If phone not found, API returns error -- show "No account found. Please register first."

---

## Screen 4: OtpVerificationScreen

**Route:** `/auth/verify`

### UI Elements
- Display text: "Enter the OTP sent to +91XXXXXXXX10" (masked phone)
- 6-digit OTP input (individual boxes, auto-focus next)
- Countdown timer: "Resend OTP in 00:30"
- "Resend OTP" button (enabled after timer)
- Auto-submit when all 6 digits entered

### Endpoints

**For Registration:**

```
POST /api/auth/verify-registration
Authorization: None
```

**Request Body:**

```json
{
  "phone": "+919876543210",
  "otp": "123456",
  "fullName": "John Consumer"
}
```

**For Login:**

```
POST /api/auth/verify-login
Authorization: None
```

**Request Body:**

```json
{
  "phone": "+919876543210",
  "otp": "654321"
}
```

**Response (both flows):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "consumer": {
    "id": "guid",
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

### UX Notes
- Determine which endpoint to call based on the `flow` param from the previous screen
- On success:
  1. Store the JWT token securely (use `expo-secure-store` or `react-native-keychain`)
  2. Store consumer profile in app state (context/redux)
  3. Navigate to the Home screen (reset navigation stack)
- On error: Show "Invalid OTP. Please try again." and clear the input
- "Resend OTP" calls the original register/login endpoint again
- SMS is currently mocked -- check server console logs for the OTP code

---

## Screen 5: ProfileScreen

**Route:** `/profile`

### UI Elements
- Consumer avatar (initials-based)
- Full Name
- Phone (verified badge)
- Email (if set)
- Account status
- Member since date
- "Edit Profile" button (future)
- "Logout" button

### Endpoint

```
GET /api/auth/me
Authorization: Bearer {token}
```

**Response:**

```json
{
  "id": "guid",
  "phone": "+919876543210",
  "fullName": "John Consumer",
  "email": null,
  "isPhoneVerified": true,
  "isActive": true,
  "lastLoginAt": "2026-03-25T10:00:00Z",
  "createdAtUtc": "2026-03-25T09:55:00Z"
}
```

### UX Notes
- If token is expired or invalid, redirect to LoginScreen
- Logout: clear stored token + profile, reset to WelcomeScreen

---

## State Management

- **JWT Token:** Store securely using `expo-secure-store` or `react-native-keychain`. Never store in AsyncStorage.
- **Consumer Profile:** Store in React Context or Redux after login. Refetch on app launch via `/api/auth/me`.
- **Auth State:** Maintain an `isAuthenticated` flag. Check on app start by attempting `/api/auth/me`.
- **Token Expiry:** Token expires after 10080 minutes (7 days). Handle 401 responses by redirecting to login.

## API Integration Notes

- All authenticated requests must include: `Authorization: Bearer {token}` header
- If any API returns 401, clear auth state and redirect to login
- The JWT `sub` claim contains the consumer's `id` (GUID)

## Enum Reference

No feature-specific enums. The JWT token is opaque to the client.
