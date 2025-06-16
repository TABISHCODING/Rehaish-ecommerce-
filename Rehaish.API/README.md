# ریہائش API

This is the backend API for the ریہائش (Rehaish) e-commerce application. It provides RESTful endpoints for managing users, products, categories, carts, wishlists, orders, and addresses.

## Table of Contents

- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Authentication](#authentication)
- [API Endpoints](#api-endpoints)
- [Database Schema](#database-schema)
- [Error Handling](#error-handling)

## Technologies

- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI

## Project Structure

The project follows a clean architecture approach with the following components:

- **Controllers**: Handle HTTP requests and responses
- **Models**: Define the database entities
- **DTOs**: Data Transfer Objects for API requests and responses
- **Services**: Implement business logic
- **Data**: Database context and configurations

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or Visual Studio Code

### Setup

1. Clone the repository
2. Update the connection string in `appsettings.json` to point to your SQL Server instance
3. Run the following commands to set up the database:

```bash
dotnet ef database update
```

4. Run the application:

```bash
dotnet run
```

5. Access the Swagger documentation at `https://localhost:7000/swagger`

## Authentication

The API uses JWT (JSON Web Token) for authentication. To access protected endpoints:

1. Register a new user or use the default admin account:
   - Email: admin@example.com
   - Password: Admin@123

2. Obtain a JWT token by making a POST request to `/api/Auth/login` with your credentials

3. Include the token in the Authorization header of subsequent requests:
   ```
   Authorization: Bearer {your_token}
   ```

## API Endpoints

### Authentication

- `POST /api/Auth/register` - Register a new user
- `POST /api/Auth/login` - Login and get JWT token
- `POST /api/Auth/forgot-password` - Request password reset
- `POST /api/Auth/reset-password` - Reset password

### Users

- `GET /api/Users` - Get all users (Admin only)
- `GET /api/Users/{id}` - Get user by ID
- `PUT /api/Users/{id}` - Update user
- `DELETE /api/Users/{id}` - Delete user (Admin only)
- `GET /api/Users/me` - Get current user profile

### Addresses

- `GET /api/Users/{userId}/addresses` - Get all addresses for a user
- `GET /api/Users/{userId}/addresses/{id}` - Get address by ID
- `POST /api/Users/{userId}/addresses` - Create a new address
- `PUT /api/Users/{userId}/addresses/{id}` - Update an address
- `DELETE /api/Users/{userId}/addresses/{id}` - Delete an address
- `PATCH /api/Users/{userId}/addresses/{id}/default` - Set an address as default

#### Address Implementation Details

The address management system follows RESTful principles and includes:

**Address Model:**
```csharp
public class Address
{
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    [MinLength(3)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string State { get; set; } = string.Empty;
    [Required]
    [RegularExpression(@"^\d{6}$")]
    public string PostalCode { get; set; } = string.Empty;
    [Required]
    public string Country { get; set; } = string.Empty;
    [Required]
    [RegularExpression(@"^\d{10}$")]
    public string Phone { get; set; } = string.Empty;
    public bool IsDefault { get; set; } = false;
    public virtual User User { get; set; } = null!;
}
```

**Address Service Interface:**
```csharp
public interface IAddressService
{
    Task<List<AddressDto>> GetAddressesByUserIdAsync(int userId);
    Task<AddressDto?> GetAddressByIdAsync(int userId, int addressId);
    Task<AddressDto> CreateAddressAsync(int userId, CreateAddressDto dto);
    Task<AddressDto?> UpdateAddressAsync(int userId, int addressId, UpdateAddressDto dto);
    Task<bool> DeleteAddressAsync(int userId, int addressId);
    Task<AddressDto?> SetDefaultAddressAsync(int userId, int addressId);
}
```

**Frontend Integration:**
```typescript
// Address management in OrderService
getUserAddresses(): Observable<Address[]> {
  const currentUser = this.authService.getCurrentUser();
  if (!currentUser || !currentUser.id) {
    return throwError(() => new Error('User ID not found'));
  }
  return this.apiService.get<Address[]>(`/users/${currentUser.id}/addresses`);
}

// Address component form validation
this.addressForm = this.fb.group({
  name: ['', [Validators.required, Validators.minLength(3)]],
  addressLine1: ['', [Validators.required]],
  addressLine2: [''],
  city: ['', [Validators.required]],
  state: ['', [Validators.required]],
  postalCode: ['', [Validators.required, Validators.pattern('^[0-9]{6}$')]],
  country: ['India', [Validators.required]],
  phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
  isDefault: [false]
});
```

**Security Features:**
- Authentication required for all address endpoints
- Users can only access their own addresses (unless they are admins)
- Comprehensive validation on both frontend and backend
- Proper error handling with appropriate HTTP status codes

### Products

- `GET /api/Products` - Get all products
- `GET /api/Products/{id}` - Get product by ID
- `POST /api/Products` - Create a new product (Admin only)
- `PUT /api/Products/{id}` - Update a product (Admin only)
- `DELETE /api/Products/{id}` - Delete a product (Admin only)
- `POST /api/Products/upload` - Upload product image (Admin only)

### Categories

- `GET /api/Categories` - Get all categories
- `GET /api/Categories/{id}` - Get category by ID
- `POST /api/Categories` - Create a new category (Admin only)
- `PUT /api/Categories/{id}` - Update a category (Admin only)
- `DELETE /api/Categories/{id}` - Delete a category (Admin only)

### Cart

- `GET /api/Cart` - Get current user's cart
- `POST /api/Cart/items` - Add item to cart
- `PUT /api/Cart/items/{id}` - Update cart item
- `DELETE /api/Cart/items/{id}` - Remove item from cart
- `DELETE /api/Cart/clear` - Clear cart

### Wishlist

- `GET /api/Wishlist` - Get current user's wishlist
- `POST /api/Wishlist/items` - Add item to wishlist
- `DELETE /api/Wishlist/items/{id}` - Remove item from wishlist

### Orders

- `GET /api/Orders` - Get current user's orders
- `GET /api/Orders/{id}` - Get order by ID
- `POST /api/Orders` - Create a new order
- `PUT /api/Orders/{id}/status` - Update order status (Admin only)
- `GET /api/Orders/reports/sales` - Get sales report (Admin only)

## Database Schema

The database includes the following main entities:

- **User**: Stores user information and authentication details
- **Address**: Stores user addresses
- **Product**: Stores product information
- **Category**: Stores product categories
- **Cart/CartItem**: Manages user shopping carts
- **Wishlist/WishlistItem**: Manages user wishlists
- **Order/OrderItem**: Manages user orders

### Address Entity Relationships

The Address entity has a one-to-many relationship with the User entity:

```csharp
// In ApplicationDbContext.cs OnModelCreating method
modelBuilder.Entity<User>()
    .HasMany(u => u.Addresses)               // A User can have many Addresses
    .WithOne(a => a.User)                    // Each Address belongs to one User
    .HasForeignKey(a => a.UserId);           // FK in Address.UserId
```

To apply the Address entity migration:

```bash
# Create a new migration for the Address entity
dotnet ef migrations add AddAddressEntity

# Apply the migration to the database
dotnet ef database update
```

## Error Handling

The API uses standard HTTP status codes:

- **200 OK**: Request succeeded
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data
- **401 Unauthorized**: Authentication required
- **403 Forbidden**: Insufficient permissions
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server-side error

Error responses include a message explaining the error and, when applicable, validation details.

## Security

- All endpoints that modify data require authentication
- Role-based authorization restricts access to admin-only endpoints
- Passwords are hashed using ASP.NET Core's password hasher
- JWT tokens expire after a configurable time period
- HTTPS is enforced in production

## Frontend Integration

### Address Management in Angular

The frontend implementation of address management includes:

1. **Address Model Interface**:
```typescript
export interface Address {
  id?: number;
  userId?: number;
  name: string;
  addressLine1: string;
  addressLine2?: string;
  city: string;
  state: string;
  postalCode: string;
  country: string;
  phone: string;
  isDefault?: boolean;
}
```

2. **Order Service Methods**:
```typescript
// Get all addresses for the current user
getUserAddresses(): Observable<Address[]> {
  const currentUser = this.authService.getCurrentUser();
  if (!currentUser || !currentUser.id) {
    return throwError(() => new Error('User ID not found'));
  }
  return this.apiService.get<Address[]>(`/users/${currentUser.id}/addresses`);
}

// Add a new address
addAddress(address: Address): Observable<Address> {
  const currentUser = this.authService.getCurrentUser();
  if (!currentUser || !currentUser.id) {
    return throwError(() => new Error('User ID not found'));
  }
  return this.apiService.post<Address>(`/users/${currentUser.id}/addresses`, address);
}

// Update an existing address
updateAddress(id: number, address: Address): Observable<Address> {
  const currentUser = this.authService.getCurrentUser();
  if (!currentUser || !currentUser.id) {
    return throwError(() => new Error('User ID not found'));
  }
  return this.apiService.put<Address>(`/users/${currentUser.id}/addresses/${id}`, address);
}

// Delete an address
deleteAddress(id: number): Observable<void> {
  const currentUser = this.authService.getCurrentUser();
  if (!currentUser || !currentUser.id) {
    return throwError(() => new Error('User ID not found'));
  }
  return this.apiService.delete<void>(`/users/${currentUser.id}/addresses/${id}`);
}

// Set an address as default
setDefaultAddress(id: number): Observable<Address> {
  const currentUser = this.authService.getCurrentUser();
  if (!currentUser || !currentUser.id) {
    return throwError(() => new Error('User ID not found'));
  }
  return this.apiService.patch<Address>(`/users/${currentUser.id}/addresses/${id}/default`, {});
}
```

3. **Addresses Component**:
The component handles:
- Displaying all user addresses
- Adding new addresses
- Editing existing addresses
- Deleting addresses
- Setting a default address

4. **Form Validation**:
```typescript
this.addressForm = this.fb.group({
  name: ['', [Validators.required, Validators.minLength(3)]],
  addressLine1: ['', [Validators.required]],
  addressLine2: [''],
  city: ['', [Validators.required]],
  state: ['', [Validators.required]],
  postalCode: ['', [Validators.required, Validators.pattern('^[0-9]{6}$')]],
  country: ['India', [Validators.required]],
  phone: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
  isDefault: [false]
});
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a new Pull Request
