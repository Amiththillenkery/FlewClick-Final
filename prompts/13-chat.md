# Feature 13: Real-Time Chat

**Role:** Both Consumer and Professional
**Auth:** JWT required (all endpoints and SignalR hub)

## Overview

Chat enables communication between consumers and professionals during booking negotiation and active project work. Uses REST endpoints for message history and SignalR WebSocket for real-time messaging. A conversation is created automatically when the first agreement is sent (Feature 11). System messages are injected for events like revision requests.

### Chat Lifecycle

- **Created:** When the professional sends the first agreement
- **Active during:** Negotiation (QuotationSent, RevisionRequested) and Active project phases
- **Persists:** Even after project completion for reference

## Screens

1. **ChatScreen** -- Full conversation view with real-time messaging

Accessed from BookingDetailScreen (Feature 10) via a "Chat" button.

## Navigation Flow

```
BookingDetail (Feature 10) -> ChatScreen
```

---

## Screen 1: ChatScreen

**Route:** `/chat/:bookingId`

### UI Elements

**Header:**
- Back button
- Other party's name
- Online/typing indicator
- Booking status badge

**Message List:**
- Scrollable, newest at bottom
- Messages grouped by date ("Today", "Yesterday", "Mar 25, 2026")
- **Consumer messages** (right-aligned, colored bubble):
  - Message text
  - Timestamp
  - Read receipt (double check if read)
- **Professional messages** (left-aligned, light bubble):
  - Message text
  - Timestamp
- **System messages** (centered, grey, no bubble):
  - "[System] Consumer requested revision: ..."
  - "[System] Agreement v2 sent"

**Input Area:**
- Text input with placeholder "Type a message..."
- Send button (disabled when empty)
- Typing indicator above input ("Jane is typing...")

---

### On Screen Mount

1. **Fetch conversation metadata:**

```
GET /api/chat/{bookingId}
Authorization: Bearer {token}
```

**Response:**

```json
{
  "id": "conversation-guid",
  "bookingRequestId": "booking-guid",
  "consumerId": "guid",
  "professionalProfileId": "guid",
  "isActive": true,
  "unreadCount": 3,
  "createdAtUtc": "2026-03-25T12:00:00Z"
}
```

2. **Fetch message history (paginated, newest first):**

```
GET /api/chat/{bookingId}/messages?page=1&pageSize=50
Authorization: Bearer {token}
```

**Response:**

```json
[
  {
    "id": "message-guid",
    "conversationId": "conversation-guid",
    "senderId": "guid",
    "senderType": 0,
    "content": "Hello! I'd like to discuss the wedding photography details.",
    "isRead": true,
    "readAt": "2026-03-25T12:05:00Z",
    "createdAtUtc": "2026-03-25T12:00:00Z"
  },
  {
    "id": "message-guid-2",
    "conversationId": "conversation-guid",
    "senderId": "guid",
    "senderType": 1,
    "content": "Hi! I'd be happy to discuss. What's the venue?",
    "isRead": false,
    "readAt": null,
    "createdAtUtc": "2026-03-25T12:10:00Z"
  }
]
```

3. **Mark messages as read:**

```
PATCH /api/chat/{bookingId}/messages/read
Authorization: Bearer {token}
```

No request body needed (recipientId from JWT).

4. **Connect to SignalR hub:**

```javascript
import { HubConnectionBuilder } from '@microsoft/signalr';

const connection = new HubConnectionBuilder()
  .withUrl('http://localhost:5216/hubs/chat?access_token=' + jwtToken)
  .withAutomaticReconnect()
  .build();

await connection.start();

// Join the conversation room
await connection.invoke('JoinConversation', conversationId);
```

---

### Sending Messages

**Via REST (reliable, persisted):**

```
POST /api/chat/{bookingId}/messages
Authorization: Bearer {token}
```

**Request Body:**

```json
{
  "content": "Hello! I'd like to discuss the wedding photography details.",
  "senderType": 0
}
```

`senderType`: `0`=Consumer, `1`=Professional

**Response:** The created ChatMessageDto

**Via SignalR (real-time broadcast):**

```javascript
await connection.invoke('SendMessage', bookingRequestId, 'Hello!');
```

**Recommended approach:** Send via REST endpoint, and rely on SignalR for real-time delivery to the other party. The REST call ensures persistence; SignalR ensures instant display.

---

### Receiving Messages (SignalR Events)

```javascript
// New message received
connection.on('ReceiveMessage', (message) => {
  // message = ChatMessageDto
  // Append to message list
  // Mark as read if chat screen is open
});

// Typing indicator
connection.on('UserTyping', (userId) => {
  // Show "User is typing..." for 3 seconds
  // Reset timer on each new typing event
});
```

---

### Sending Typing Indicator

```javascript
// Call when user starts typing (debounced, max once per 2 seconds)
await connection.invoke('TypingIndicator', conversationId);
```

---

### Leaving Conversation (Screen Unmount)

```javascript
await connection.invoke('LeaveConversation', conversationId);
await connection.stop();
```

---

### Pagination (Load Older Messages)

When the user scrolls to the top of the message list:

```
GET /api/chat/{bookingId}/messages?page=2&pageSize=50
Authorization: Bearer {token}
```

Prepend older messages to the top of the list. Maintain scroll position.

---

## UX Detailed Notes

### Message Bubbles
- Consumer messages: right-aligned, primary color background, white text
- Professional messages: left-aligned, light grey background, dark text
- System messages: centered, smaller font, grey italic text, no bubble

### Timestamps
- Show time (HH:MM) on each message
- Show date separator between messages on different days

### Read Receipts
- Single check: sent
- Double check: delivered (when other party's device receives via SignalR)
- Blue double check: read (when `isRead: true`)

### Typing Indicator
- Show "Jane is typing..." below the header or above the input
- Auto-hide after 3 seconds of no typing events
- Debounce outgoing typing events to max 1 per 2 seconds

### Empty State
- If conversation exists but no messages: "Start the conversation!"
- If no conversation yet (booking not at agreement stage): "Chat will be available after the agreement is sent."

### Error Handling
- SignalR disconnection: show "Reconnecting..." banner, auto-reconnect
- Send failure: show message with red indicator and "Retry" button
- If JWT expired during chat, redirect to login

---

## State Management

- **Connection:** Maintain SignalR connection as long as ChatScreen is mounted
- **Messages:** Store in local state, append on new messages
- **Unread count:** Update on ReceiveMessage event (for badge on other screens)
- **Typing state:** Ephemeral, not persisted

## Dependencies

- `@microsoft/signalr` -- SignalR client for React Native
- React Native FlatList (inverted) for message list

## Enum Reference

| Enum | Values |
|------|--------|
| MessageSenderType | `0`=Consumer, `1`=Professional, `2`=System |
