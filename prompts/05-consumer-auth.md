# Feature 5: Consumer Authentication (Password + JWT + Refresh Token)

**Role:** Consumer (new user registration and login)
**Auth:** None for registration/login/refresh, JWT for profile access and logout

## Overview

Consumers register and log in using their phone number + password. The flow mirrors the professional auth system: on successful registration or login, the backend returns a short-lived JWT access token (15 min) and a long-lived refresh token (30 days). The client uses the access token for authenticated requests and silently refreshes it before expiry using the refresh token.

## Screens

1. **WelcomeScreen** -- Entry point with Register / Login options
2. **RegisterScreen** -- Phone + name + password + optional email for new users
3. **LoginScreen** -- Phone + password for returning users
4. **ProfileScreen** -- View logged-in consumer profile
5. (No OTP screen needed -- password-based auth)

## Navigation Flow

```
Welcome -> [Register | Login] -> Home (authenticated)
Settings -> Profile
Token expired -> silent refresh via /api/consumer/auth/refresh
Logout -> revoke refresh token -> Welcome
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
- Text input: Email (optional)
- Text input: Password (required, secure entry)
- Text input: Confirm Password (client-side validation only)
- Button: "Register"
- "Already have an account? Login" link

### Password Requirements
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one digit
- At least one special character

### Endpoint

```
POST /api/consumer/auth/register
Authorization: None
```

**Request Body:**

```json
{
  "phone": "+919876543210",
  "fullName": "John Consumer",
  "password": "SecureP@ss123",
  "email": "john@example.com"
}
```

**Response (201):**

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "base64-random-token...",
  "expiresInSeconds": 900,
  "consumer": {
    "id": "guid",
    "phone": "+919876543210",
    "fullName": "John Consumer",
    "email": "john@example.com",
    "isPhoneVerified": true,
    "isActive": true,
    "lastLoginAt": "2026-03-26T10:00:00Z",
    "createdAtUtc": "2026-03-26T10:00:00Z"
  }
}
```

### UX Notes
- Validate phone format: must start with +91, 10 digits after
- Validate password requirements client-side before submitting
- Confirm password must match password (client-side only)
- On success:
  1. Store `accessToken` in memory (React state/context)
  2. Store `refreshToken` securely using `expo-secure-store` or `react-native-keychain`
  3. Store consumer profile in app state
  4. Navigate to Home screen (reset navigation stack)
- If phone already registered, API returns error -- show "This number is already registered. Please login instead."

---

## Screen 3: LoginScreen

**Route:** `/auth/login`

### UI Elements
- Text input: Phone Number (required)
- Text input: Password (required, secure entry)
- Button: "Login"
- "New user? Register" link

### Endpoint

```
POST /api/consumer/auth/login
Authorization: None
```

**Request Body:**

```json
{
  "phone": "+919876543210",
  "password": "SecureP@ss123"
}
```

**Response (200):**

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "base64-random-token...",
  "expiresInSeconds": 900,
  "consumer": {
    "id": "guid",
    "phone": "+919876543210",
    "fullName": "John Consumer",
    "email": "john@example.com",
    "isPhoneVerified": true,
    "isActive": true,
    "lastLoginAt": "2026-03-26T10:00:00Z",
    "createdAtUtc": "2026-03-25T09:55:00Z"
  }
}
```

### UX Notes
- On success: store tokens and profile, navigate to Home
- On error "Invalid phone number or password." -- show error, do not reveal which field is wrong
- On error "Your account has been deactivated." -- show message with support contact

---

## Token Refresh (Silent -- No Screen)

When the access token is about to expire or a 401 is received, call the refresh endpoint.

### Endpoint

```
POST /api/consumer/auth/refresh
Authorization: None
```

**Request Body:**

```json
{
  "refreshToken": "base64-random-token..."
}
```

**Response (200):**

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...(new)",
  "refreshToken": "base64-random-token...(new)",
  "expiresInSeconds": 900
}
```

### UX Notes
- Implement an Axios/fetch interceptor that:
  1. Detects 401 responses
  2. Calls `/api/consumer/auth/refresh` with the stored refresh token
  3. Updates stored tokens with the new pair
  4. Retries the original failed request with the new access token
- If refresh also fails (expired/revoked), redirect to LoginScreen
- The old refresh token is automatically revoked on the server -- always store the new one
- Consider proactively refreshing ~1 minute before expiry using a timer

---

## Logout (Revoke Token)

### Endpoint

```
POST /api/consumer/auth/revoke
Authorization: Bearer {accessToken}
```

**Request Body:**

```json
{
  "refreshToken": "base64-random-token..."
}
```

**Response:** `204 No Content`

### UX Notes
- Call this endpoint when the user taps "Logout"
- After calling, clear both tokens and profile from storage
- Navigate to WelcomeScreen (reset navigation stack)
- Even if the revoke call fails (network error), still clear local state

---

## Screen 4: ProfileScreen

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
GET /api/consumer/auth/me
Authorization: Bearer {accessToken}
```

**Response (200):**

```json
{
  "id": "guid",
  "phone": "+919876543210",
  "fullName": "John Consumer",
  "email": "john@example.com",
  "isPhoneVerified": true,
  "isActive": true,
  "lastLoginAt": "2026-03-26T10:00:00Z",
  "createdAtUtc": "2026-03-25T09:55:00Z"
}
```

### UX Notes
- On app launch, call `/me` with stored access token to restore session
- If 401, attempt token refresh; if refresh fails, redirect to login

---

## State Management

- **Access Token:** Store in memory (React Context / Redux). Never persist to disk.
- **Refresh Token:** Store securely using `expo-secure-store` or `react-native-keychain`. Never store in AsyncStorage.
- **Consumer Profile:** Store in React Context or Redux after login. Refetch on app launch via `/api/consumer/auth/me`.
- **Auth State:** Maintain an `isAuthenticated` flag. Check on app start by attempting `/api/consumer/auth/me`.
- **Token Expiry:** Access token expires after 15 minutes. Set up automatic refresh.

## Token Flow

```
Register/Login
  └─> Receive accessToken (15 min) + refreshToken (30 days)
  └─> Store accessToken in memory, refreshToken in SecureStore
  └─> Use accessToken in Authorization: Bearer header

Access token about to expire
  └─> POST /api/consumer/auth/refresh { refreshToken }
  └─> Receive new accessToken + new refreshToken
  └─> Update stored tokens

Logout
  └─> POST /api/consumer/auth/revoke { refreshToken }
  └─> Clear all stored tokens and profile
  └─> Navigate to Welcome screen

App restart
  └─> Read refreshToken from SecureStore
  └─> POST /api/consumer/auth/refresh { refreshToken }
  └─> If success: restore session
  └─> If fail: redirect to Login
```

## API Endpoint Summary

| Method | Endpoint | Auth | Description |
|--------|----------|------|-------------|
| `POST` | `/api/consumer/auth/register` | None | Register with phone + password |
| `POST` | `/api/consumer/auth/login` | None | Login with phone + password |
| `POST` | `/api/consumer/auth/refresh` | None | Rotate refresh token, get new access token |
| `POST` | `/api/consumer/auth/revoke` | JWT | Revoke refresh token (logout) |
| `GET` | `/api/consumer/auth/me` | JWT | Get current consumer profile |

## JWT Claims

| Claim | Value |
|-------|-------|
| `sub` | Consumer ID (GUID) |
| `phone` | Phone number |
| `name` | Full name |
| `email` | Email (if set) |
| `user_type` | `"Consumer"` |
