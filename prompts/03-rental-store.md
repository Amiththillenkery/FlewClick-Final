# Feature 3: Rental Store

**Role:** Professional (Digital Rental only)
**Auth:** None (professional auth not yet implemented)

## Overview

Digital Rental professionals configure a rental store, list products (cameras, lenses, lighting, etc.) with images and specifications, and set rental pricing (hourly, daily, weekly, monthly rates). Consumers browse these products when viewing a rental professional's profile.

## Screens

1. **StoreSetupScreen** -- Create or edit the rental store
2. **ProductListScreen** -- List all products in the store
3. **AddProductScreen** -- Add a new product
4. **ProductDetailScreen** -- View product with images and pricing
5. **ProductImagesScreen** -- Manage product images
6. **ProductPricingScreen** -- Set rental rates for a product

## Navigation Flow

```
StoreSetup -> ProductList -> AddProduct -> ProductDetail -> [ProductImages | ProductPricing]
```

---

## Screen 1: StoreSetupScreen

**Route:** `/rental/store/setup`

### UI Elements
- Text input: Store Name (required)
- Text area: Description (optional)
- Dynamic key-value inputs: Policies (min rental days, deposit required, cancellation policy)
- Hidden field: profileId
- Button: "Create Store" or "Update Store"

### Endpoints

**Create:**

```
POST /api/rental/store
Authorization: None
```

**Request Body:**

```json
{
  "profileId": "{{profileId}}",
  "storeName": "ProGear Rentals",
  "description": "Premium camera and lens rentals",
  "policies": {
    "min_rental_days": 1,
    "deposit_required": true,
    "cancellation_policy": "24 hours notice required"
  }
}
```

**Update:**

```
PUT /api/rental/store/{id}
Authorization: None
```

**Get existing store:**

```
GET /api/rental/store/profile/{profileId}
Authorization: None
```

### UX Notes
- On first visit, show "Create Store" form
- If store already exists, pre-fill fields and show "Update Store"
- Policies can be added/removed dynamically (key-value pairs)

---

## Screen 2: ProductListScreen

**Route:** `/rental/store/:storeId/products`

### UI Elements
- Header: Store name
- FAB: "+ Add Product"
- Grid or list of product cards:
  - Primary image thumbnail
  - Product name
  - Brand + Model
  - Condition badge
  - Availability indicator (green dot = available)
  - Daily rate
- Empty state: "No products yet. Add your first rental product!"

### Endpoint

```
GET /api/rental/store/{storeId}/products
Authorization: None
```

---

## Screen 3: AddProductScreen

**Route:** `/rental/products/add`

### UI Elements
- Text input: Name (required)
- Dropdown: Condition (New, LikeNew, Good, Fair)
- Text area: Description (optional)
- Text input: Category (e.g., Camera, Lens, Lighting)
- Text input: Brand (optional)
- Text input: Model (optional)
- Dynamic key-value: Specifications (sensor, megapixels, etc.)
- Button: "Add Product"

### Endpoint

```
POST /api/rental/store/{storeId}/products
Authorization: None
```

**Request Body:**

```json
{
  "name": "Sony A7IV",
  "condition": 1,
  "description": "Full frame mirrorless camera",
  "category": "Camera",
  "brand": "Sony",
  "model": "A7IV",
  "specifications": { "sensor": "Full Frame", "megapixels": 33 }
}
```

### UX Notes
- On success, navigate to ProductDetailScreen
- Specifications are dynamic key-value pairs the professional can add

---

## Screen 4: ProductDetailScreen

**Route:** `/rental/products/:productId`

### UI Elements
- Image carousel at top
- Product name, brand, model
- Condition badge
- Description
- Specifications list
- Availability toggle
- **Images section:** "Manage Images" button
- **Pricing section:** Shows rates if set, "Set Pricing" button
- Edit / Delete buttons

### Endpoints

```
GET /api/rental/products/{id}                    -- Full product detail
PATCH /api/rental/products/{id}/availability     -- Toggle availability
PUT /api/rental/products/{id}                    -- Update product
DELETE /api/rental/products/{id}                 -- Delete product
```

---

## Screen 5: ProductImagesScreen

**Route:** `/rental/products/:productId/images`

### UI Elements
- Grid of current images with delete (X) button on each
- "Add Image" button
- Image URL input (or image picker in future)
- Display order number
- "Set as Primary" toggle

### Endpoints

```
POST /api/rental/products/{productId}/images     -- Add image
DELETE /api/rental/products/images/{imageId}      -- Remove image
```

**Add Image Request:**

```json
{
  "imageUrl": "https://cdn.example.com/products/sony-a7iv.jpg",
  "displayOrder": 0,
  "isPrimary": true
}
```

---

## Screen 6: ProductPricingScreen

**Route:** `/rental/products/:productId/pricing`

### UI Elements
- Number input: Deposit Amount (required)
- Number input: Hourly Rate (optional)
- Number input: Daily Rate (optional)
- Number input: Weekly Rate (optional)
- Number input: Monthly Rate (optional)
- Button: "Save Pricing"

### Endpoint

```
POST /api/rental/products/{productId}/pricing
Authorization: None
```

**Request Body:**

```json
{
  "depositAmount": 5000,
  "hourlyRate": 200,
  "dailyRate": 1500,
  "weeklyRate": 8000,
  "monthlyRate": 25000
}
```

---

## State Management

- Store `storeId` after creation
- Refetch product list on focus
- Cache product details locally for quick display

## Enum Reference

| Enum | Values |
|------|--------|
| ProductCondition | `0`=New, `1`=LikeNew, `2`=Good, `3`=Fair |
