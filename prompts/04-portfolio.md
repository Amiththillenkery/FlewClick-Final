# Feature 4: Portfolio Configuration

**Role:** Professional (Photographers, Videographers, Editors, Drone Owners -- NOT Digital Rental)
**Auth:** None (professional auth not yet implemented)

## Overview

Professionals connect their Instagram business account to automatically sync posts (photos, reels, carousels) as portfolio items. No file uploads needed -- everything comes from Instagram via OAuth. Professionals can reorder items, toggle visibility, and refresh their feed.

## Screens

1. **PortfolioOverviewScreen** -- View portfolio items with management controls
2. **InstagramConnectScreen** -- OAuth flow to connect Instagram
3. **InstagramStatusScreen** -- Connection status and token management
4. **ReorderScreen** -- Drag-and-drop reorder of portfolio items

## Navigation Flow

```
PortfolioOverview -> [InstagramConnect (if not connected)] -> InstagramStatus -> PortfolioOverview
PortfolioOverview -> ReorderScreen -> PortfolioOverview
```

---

## Screen 1: PortfolioOverviewScreen

**Route:** `/portfolio/:profileId`

### UI Elements
- Header: "My Portfolio"
- Instagram connection status bar at top:
  - Connected: green badge with username, "Sync Now" button
  - Not connected: "Connect Instagram" button
- Grid of portfolio items (2 or 3 columns):
  - Image/video thumbnail
  - Caption preview (truncated)
  - Visibility toggle (eye icon)
  - Media type badge (Image, Video, Carousel)
- "Reorder" button
- Pull-to-refresh triggers sync

### Endpoints

```
GET /api/portfolio/{profileId}?visibleOnly=false     -- All items (owner view)
GET /api/portfolio/instagram/{profileId}/status       -- Connection status
POST /api/portfolio/{profileId}/sync                  -- Sync from Instagram
PATCH /api/portfolio/items/{itemId}/visibility        -- Toggle visibility
```

**Portfolio Response:**

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

**Sync Response:**

```json
{
  "totalItems": 25,
  "newItems": 5,
  "updatedItems": 3
}
```

### UX Notes
- Show sync result as a brief toast: "Synced 5 new items, updated 3"
- Tapping an item could open it in a modal with full caption and permalink
- Items with `isVisible: false` should appear dimmed in the grid

---

## Screen 2: InstagramConnectScreen

**Route:** `/portfolio/instagram/connect`

### Flow
1. Call the auth URL endpoint to get the Instagram OAuth URL
2. Open a WebView or in-app browser with that URL
3. User authorizes on Instagram
4. Instagram redirects to the callback URL with an auth code
5. The backend handles the callback and exchanges the code for tokens
6. Navigate back to PortfolioOverviewScreen

### Endpoint

```
GET /api/portfolio/instagram/auth-url?profileId={profileId}
Authorization: None
```

**Response:**

```json
{
  "authUrl": "https://api.instagram.com/oauth/authorize?client_id=...&redirect_uri=...&scope=...&state={profileId}"
}
```

### Callback (handled by backend)

```
GET /api/portfolio/instagram/callback?code={code}&state={profileId}
```

### UX Notes
- Use `react-native-webview` or `expo-web-browser`
- Listen for the redirect URL in the WebView to know when auth is complete
- Show loading spinner while waiting for callback completion
- On success, show "Instagram connected!" and return to portfolio

---

## Screen 3: InstagramStatusScreen

**Route:** `/portfolio/instagram/:profileId/status`

### UI Elements
- Connection status: Connected / Not Connected
- Instagram username (if connected)
- Token expiry info
- "Refresh Token" button (tokens expire every 60 days)
- "Disconnect Instagram" button (with confirmation dialog)

### Endpoints

```
GET /api/portfolio/instagram/{profileId}/status           -- Check status
POST /api/portfolio/instagram/{profileId}/refresh         -- Refresh token
DELETE /api/portfolio/instagram/{profileId}?clearPortfolio=false  -- Disconnect
```

### UX Notes
- Warn before disconnecting: "This will remove Instagram connection. Portfolio items will remain unless you choose to clear them."
- `clearPortfolio` query param: if true, also removes all portfolio items

---

## Screen 4: ReorderScreen

**Route:** `/portfolio/:profileId/reorder`

### UI Elements
- Draggable grid or list of portfolio items
- Each item shows thumbnail and current order number
- "Save Order" button
- "Cancel" button

### Endpoint

```
PUT /api/portfolio/{profileId}/reorder
Authorization: None
```

**Request Body:**

```json
{
  "orderedItemIds": ["guid1", "guid2", "guid3"]
}
```

### UX Notes
- Use a drag-and-drop library like `react-native-draggable-flatlist`
- Show current position numbers that update as items are dragged
- Disable save button until order actually changes

---

## State Management

- Store Instagram connection status locally
- Refetch portfolio items after sync
- Cache portfolio grid for offline viewing

## Enum Reference

| Enum | Values |
|------|--------|
| MediaType | `Image`, `Video`, `CarouselAlbum` |
