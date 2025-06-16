Here is a comprehensive chart mapping each backend API endpoint (from your API documentation) to customer and admin access, and whether a corresponding frontend UI exists in your Angular project. This will help you ensure your frontend fully utilizes your backend and covers all use cases.

Customer Access – Endpoint/UI Mapping
Area	Endpoint (HTTP)	Customer Access	Frontend UI Available?	UI Location/Component	Description/Task
Auth	/api/Auth/register (POST)	✅	✅	register.component.ts	Register new account
Auth	/api/Auth/login (POST)	✅	✅	login.component.ts	Login
Auth	/api/Auth/change-password (POST)	✅	✅	settings.component.ts	Change password in account settings
Cart	/api/Cart (GET, POST, DELETE/clear)	✅	✅	cart.component.ts	View/add/remove/clear cart items
Cart	/api/Cart/items/{itemId} (DELETE)	✅	✅	cart.component.ts	Remove item from cart
Categories	/api/Categories (GET)	✅	✅	product-list.component.ts	View all categories
Categories	/api/Categories/{id} (GET)	✅	✅	product-list.component.ts	View category details
Orders	/api/Orders (POST)	✅	✅	checkout.component.ts	Place order
Orders	/api/Orders/user/{userId} (GET)	✅*	✅	profile.component.ts (orders tab)	View own orders
Orders	/api/Orders/{orderId} (GET)	✅*	✅	profile.component.ts (orders tab)	View order details
Products	/api/Products (GET)	✅	✅	product-list.component.ts	View all products
Products	/api/Products/{id} (GET)	✅	✅	product-detail.component.ts	View product details
Users	/api/Users/me (GET)	✅	✅	profile.component.ts	View own profile
Users	/api/Users/{id} (GET, PUT)	✅*	✅	profile.component.ts	View/update own profile
Wishlist	/api/Wishlist/{userId} (GET)	✅*	✅	wishlist.component.ts	View wishlist
Wishlist	/api/Wishlist/items (POST, DELETE)	✅	✅	wishlist.component.ts	Add/remove wishlist items
Addresses	/api/users/{userId}/addresses (GET, POST, PUT, DELETE, PATCH)	✅*	✅	addresses.component.ts	Manage shipping addresses
Admin Access – Endpoint/UI Mapping
Area	Endpoint (HTTP)	Admin Access	Frontend UI Available?	UI Location/Component	Description/Task
Auth	/api/Auth/register, login, change-password	✅	✅	register.component.ts, login.component.ts, settings.component.ts	Register/login/change password
Categories	/api/Categories (GET, POST, PUT, DELETE)	✅	✅	admin-category-list, admin-category-form	Full CRUD for categories
Products	/api/Products (GET, POST, PUT, DELETE)	✅	✅	admin-product-list, admin-product-form	Full CRUD for products
Products	/api/Products/upload (POST)	✅	✅	admin-product-form, admin-settings	Upload product images
Orders	/api/Orders (GET)	✅	✅	admin-orders-list, admin-order-detail	View all orders
Users	/api/Users (GET, POST, PUT, DELETE)	✅	✅	admin-user-list, admin-user-form	Manage users (CRUD)
Wishlist	/api/Wishlist/{userId} (GET)	✅	(Partial)	(Not always exposed in admin UI)	View any user's wishlist
Addresses	/api/users/{userId}/addresses (all)	✅	(Partial)	(Not always exposed in admin UI)	Manage any user's addresses
Reports	(Custom, e.g. sales/category)	✅	✅	admin-reports.component.ts	View sales, category, and other reports
Notes
✅* means user/admin can only access their own resources (for customers).
All major CRUD operations for products, categories, users, and orders are available in the admin UI.
Customer UI covers all typical e-commerce flows: browse, cart, wishlist, checkout, profile, addresses, and order history.
Some admin endpoints (like viewing any user's wishlist/addresses) may not have a direct UI but can be added if needed.
Recommendations
Review the above chart and ensure every backend endpoint you want exposed has a matching UI component.
For any endpoint marked as (Partial) or missing, consider if you want to add a UI for it.
This mapping ensures your frontend and backend are in sync and your app is feature-complete for both customers and admins.
If you want a visual chart or need this in a different format (CSV, diagram), let me know!




🛠️ Admin Role – Full Access
✅ This section details all the endpoints an Admin can access, mapped to the frontend UI with real-world examples and descriptions.

*************************************************************👤 Resource: User
🔹 GET
📍 By: none (get all users)
Endpoint: /api/Users

Access: Admin

UI Action/Button: View All Users

UI Component: admin-user-list.component.ts

Description: Displays a list of all registered users in a table. Admin can click “Edit” or “Delete” from the table for each user.

📍 By: id
Endpoint: /api/Users/{id}

Access: Admin

UI Action/Button: View/Edit User Details

UI Component: admin-user-form.component.ts

Description: Used when admin clicks “Edit” on a user. Fetches user data and populates a form for editing.

🔹 POST
Endpoint: /api/Users

Access: Admin

UI Action/Button: Add New User

UI Component: admin-user-form.component.ts

Description: Admin creates a new user manually (for example, when onboarding staff internally). A form is presented with fields like name, email, role, etc.

🔹 PUT
📍 By: id
Endpoint: /api/Users/{id}

Access: Admin

UI Action/Button: Save Changes

UI Component: admin-user-form.component.ts

Description: When admin edits user data (like role change or blocking), the updated form is submitted via PUT.

🔹 DELETE
📍 By: id
Endpoint: /api/Users/{id}

Access: Admin

UI Action/Button: Delete User

UI Component: admin-user-list.component.ts

Description: Admin clicks “Delete” next to a user. A confirmation dialog appears; if confirmed, user is deleted via API.
 

 *********************************************************🛍️ Resource: Product
🔹 GET
📍 By: none (get all products)
Endpoint: /api/Products

Access: Admin

UI Action/Button: View All Products

UI Component: admin-product-list.component.ts

Description: Admin sees a table of all products. Each row includes buttons like Edit, Delete, and View.

📍 By: id
Endpoint: /api/Products/{id}

Access: Admin

UI Action/Button: View/Edit Product

UI Component: admin-product-form.component.ts

Description: When admin clicks Edit on a product from the product list, this API fetches product details to populate the edit form.

🔹 POST
📍 Create a product
Endpoint: /api/Products

Access: Admin

UI Action/Button: Add New Product

UI Component: admin-product-form.component.ts

Description: Admin fills out the form for a new product (title, price, description, stock, category, etc.) and submits it to add the product.

📍 Upload image(s)
Endpoint: /api/Products/upload

Access: Admin

UI Action/Button: Upload Product Image

UI Component: admin-product-form.component.ts, admin-settings.component.ts

Description: Used to upload product images. May support multiple image uploads with drag-and-drop or file input.

🔹 PUT
📍 By: id
Endpoint: /api/Products/{id}

Access: Admin

UI Action/Button: Save Changes

UI Component: admin-product-form.component.ts

Description: Used to update an existing product’s details. The form is pre-filled with current values, which can be changed and saved.

🔹 DELETE
📍 By: id
Endpoint: /api/Products/{id}

Access: Admin

UI Action/Button: Delete Product

UI Component: admin-product-list.component.ts

Description: Admin clicks delete on a product row. A confirmation prompt shows before sending the DELETE request.





*************************************************************🗂️ Resource: Category
🔹 GET
📍 By: none (get all categories)
Endpoint: /api/Categories

Access: Admin

UI Action/Button: View All Categories

UI Component: admin-category-list.component.ts

Description: Displays a list of all product categories in a table. Admin can manage them with Edit and Delete buttons.

📍 By: id
Endpoint: /api/Categories/{id}

Access: Admin

UI Action/Button: Edit Category

UI Component: admin-category-form.component.ts

Description: When admin wants to edit a category, this endpoint is called to fetch category details and fill the edit form.

🔹 POST
Endpoint: /api/Categories

Access: Admin

UI Action/Button: Add New Category

UI Component: admin-category-form.component.ts

Description: Admin enters a category name, slug, image, or metadata and clicks “Add.” The form sends this data to create a new category.

🔹 PUT
📍 By: id
Endpoint: /api/Categories/{id}

Access: Admin

UI Action/Button: Save Changes

UI Component: admin-category-form.component.ts

Description: When editing a category, updated values are submitted via PUT to update category data (e.g., name, slug, icon).

🔹 DELETE
📍 By: id
Endpoint: /api/Categories/{id}

Access: Admin

UI Action/Button: Delete Category

UI Component: admin-category-list.component.ts

Description: Admin clicks delete next to a category in the list. Confirmation appears before deleting.






*******************************************************************📦 Resource: Order
🔹 GET
📍 By: none (get all orders)
Endpoint: /api/Orders

Access: Admin

UI Action/Button: View All Orders

UI Component: admin-orders-list.component.ts

Description: Shows a list of all customer orders. Each row includes order status, user, total price, and action buttons like View, Update Status, or Delete.

📍 By: orderId
Endpoint: /api/Orders/{orderId}

Access: Admin

UI Action/Button: View Order Details

UI Component: admin-order-detail.component.ts

Description: When the admin clicks View on an order, this endpoint fetches the order's full details including products, user, shipping address, payment status, etc.


 Note: Admin typically does not create or update orders; this is done by customers at checkout. Admin can optionally manage status updates if you build that in.



🔹 PUT (optional, if status updates are supported)
📍 By: orderId (e.g., update status)
Endpoint: /api/Orders/{orderId}

Access: Admin

UI Action/Button: Update Order Status, Mark as Shipped/Delivered

UI Component: admin-order-detail.component.ts

Description: Admin updates the status of an order (e.g., from "Pending" to "Shipped"). Triggered by dropdown or status buttons in order detail view.

****************************************************💖 Resource: Wishlist (Admin Partial Access)
🔹 GET
📍 By: userId
Endpoint: /api/Wishlist/{userId}

Access: Admin (Partial)

UI Action/Button: Not always exposed in admin UI

UI Component: (Optional) admin-user-wishlist.component.ts

Description: Admin can view the wishlist of a specific user by their ID. Typically used for customer support or user insights, but UI may not always be available by default.

🔹 POST / DELETE
Note: Usually wishlist modification is done by the user themselves, so admin UI for adding/removing items is uncommon.


************************************************📬 Resource: Addresses (Admin Access – Manage Any User’s Addresses)
🔹 GET
📍 By: userId
Endpoint: /api/users/{userId}/addresses

Access: Admin

UI Action/Button: (Optional) View User Addresses

UI Component: (Optional component – can be added as) admin-user-addresses.component.ts

Description: Admin can view all addresses saved by a specific user. Useful for resolving shipping issues or verifying user information.

🔹 POST
📍 By: userId
Endpoint: /api/users/{userId}/addresses

Access: Admin

UI Action/Button: (Optional) Add Address for User

UI Component: admin-user-addresses-form.component.ts (can be added if needed)

Description: Admin manually adds a new address for the user (e.g., during account setup or address correction).

🔹 PUT
📍 By: userId & Address ID
Endpoint: /api/users/{userId}/addresses/{addressId}

Access: Admin

UI Action/Button: Edit Address

UI Component: admin-user-addresses-form.component.ts (optional UI)

Description: Admin updates an existing address for a user. This can be useful if the user reports incorrect info.

🔹 DELETE
📍 By: userId & Address ID
Endpoint: /api/users/{userId}/addresses/{addressId}

Access: Admin

UI Action/Button: Delete Address

UI Component: admin-user-addresses.component.ts (optional UI)

Description: Admin deletes a user’s address — for example, if it’s outdated or user requests removal.

🔹 PATCH
📍 By: userId & Address ID
Endpoint: /api/users/{userId}/addresses/{addressId}

Access: Admin

UI Action/Button: Set as Default Address

UI Component: admin-user-addresses.component.ts (optional UI)

Description: Allows admin to mark an address as the user’s default for shipping.


**********************************************************🔐 Resource: Auth (Admin Access – Register/Login/Password)
🔹 POST
📍 Register
Endpoint: /api/Auth/register

Access: Admin (for own registration, or creating other admin accounts manually)

UI Action/Button: Register

UI Component: register.component.ts

Description: Admin can register a new account. If role is assigned as "admin", this becomes a privileged account. Used during setup or to create more admin users.

📍 Login
Endpoint: /api/Auth/login

Access: Admin

UI Action/Button: Login

UI Component: login.component.ts

Description: Admin enters email and password to log into the admin dashboard. On success, receives JWT token and redirects to admin home.

📍 Change Password
Endpoint: /api/Auth/change-password

Access: Admin

UI Action/Button: Change Password

UI Component: settings.component.ts

Description: Admin can change their password from account settings or security page.




************************************************📊 Resource: Reports (Admin Access – Sales, Category, Inventory Insights)
⚠️ Note: These endpoints are often custom and vary between implementations, but typically used for business insights, dashboards, and analytics.

🔹 GET
📍 Sales Report
Endpoint: /api/Reports/sales

Access: Admin

UI Action/Button: View Sales Report

UI Component: admin-reports.component.ts

Description: Displays sales data over time — daily, weekly, or monthly revenue. Admin uses this to track performance, revenue trends, and identify high/low-performing products.

📍 Category-wise Report
Endpoint: /api/Reports/category-sales

Access: Admin

UI Action/Button: Category Report

UI Component: admin-reports.component.ts

Description: Shows which product categories are performing best. Helps admin in inventory and marketing decisions.

📍 Inventory Report (optional/custom)
Endpoint: /api/Reports/inventory-status

Access: Admin

UI Action/Button: Stock Overview

UI Component: admin-reports.component.ts or admin-inventory.component.ts

Description: Overview of products in stock, low-stock alerts, and out-of-stock items.








**************************************************************************************************************************************************************************************************************************************************************************************************************************************************
📘 Admin Endpoint-to-UI Quick Reference (Full Access)
🔍 Resource	🛠️ HTTP Method	📎 Endpoint	🎯 Action/Use Case	🧑‍💼 Who Can Access	🖱️ UI Action/Button	🧩 UI Component
Auth	POST	/api/Auth/register	Register admin account	Admin	Register	register.component.ts
POST	/api/Auth/login	Login admin	Admin	Login	login.component.ts
POST	/api/Auth/change-password	Change admin password	Admin	Change Password	settings.component.ts
Categories	GET	/api/Categories	List all categories	Admin	View Categories	admin-category-list.component.ts
POST	/api/Categories	Add new category	Admin	Add Category	admin-category-form.component.ts
PUT	/api/Categories/{id}	Edit category	Admin	Edit Category	admin-category-form.component.ts
DELETE	/api/Categories/{id}	Delete category	Admin	Delete Category	admin-category-list.component.ts
Products	GET	/api/Products	List all products	Admin	View Products	admin-product-list.component.ts
POST	/api/Products	Add new product	Admin	Add Product	admin-product-form.component.ts
PUT	/api/Products/{id}	Edit product	Admin	Edit Product	admin-product-form.component.ts
DELETE	/api/Products/{id}	Delete product	Admin	Delete Product	admin-product-list.component.ts
POST	/api/Products/upload	Upload product image	Admin	Upload Image	admin-product-form.component.ts
Orders	GET	/api/Orders	View all orders	Admin	View Orders	admin-orders-list.component.ts
GET	/api/Orders/{orderId}	View order details	Admin	View Order Details	admin-order-detail.component.ts
Users	GET	/api/Users	View all users	Admin	View Users	admin-user-list.component.ts
POST	/api/Users	Create new user (optional)	Admin	Add User	admin-user-form.component.ts
PUT	/api/Users/{id}	Update user info	Admin	Edit User	admin-user-form.component.ts
DELETE	/api/Users/{id}	Delete user	Admin	Delete User	admin-user-list.component.ts
Wishlist	GET	/api/Wishlist/{userId}	View any user's wishlist	Admin	(Optional) View Wishlist	(optional UI)
Addresses	GET	/api/users/{userId}/addresses	View any user’s addresses	Admin	(Optional) View Addresses	(optional UI)
POST	/api/users/{userId}/addresses	Add new address for a user	Admin	Add Address	(optional UI)
PUT	/api/users/{userId}/addresses/{addressId}	Update address for a user	Admin	Edit Address	(optional UI)
DELETE	/api/users/{userId}/addresses/{addressId}	Delete user address	Admin	Delete Address	(optional UI)
PATCH	/api/users/{userId}/addresses/{addressId}	Set address as default	Admin	Set as Default	(optional UI)
Reports	GET	/api/Reports/sales	View sales report	Admin	Sales Report	admin-reports.component.ts
GET	/api/Reports/category-sales	View sales by category	Admin	Category Report	admin-reports.component.ts
GET	/api/Reports/inventory-status (optional)	Inventory stock report	Admin	Inventory Overview	admin-reports.component.ts


**************************************************************************************************************************************************************************************************************************************************************************************************************************************************
.

📘 Customer Endpoint-to-UI Quick Reference (Customer Access)
🔍 Resource	🛠️ HTTP Method	📎 Endpoint	🎯 Action/Use Case	🧑‍💼 Who Can Access	🖱️ UI Action/Button	🧩 UI Component
Auth	POST	/api/Auth/register	Register new user	Customer	Register	register.component.ts
POST	/api/Auth/login	Login user	Customer	Login	login.component.ts
POST	/api/Auth/change-password	Change password in account settings	Customer	Change Password	settings.component.ts
Cart	GET	/api/Cart	View cart items	Customer	View Cart	cart.component.ts
POST	/api/Cart	Add item to cart	Customer	Add to Cart	Product detail/list component button
DELETE (clear cart)	/api/Cart	Clear all items from cart	Customer	Clear Cart	cart.component.ts
DELETE (by itemId)	/api/Cart/items/{itemId}	Remove specific item from cart	Customer	Remove Item	cart.component.ts
Categories	GET	/api/Categories	List all categories	Customer	Browse Categories	product-list.component.ts
GET (by id)	/api/Categories/{id}	View category details	Customer	Select Category	product-list.component.ts
Orders	POST	/api/Orders	Place order	Customer	Place Order	checkout.component.ts
GET (user-specific)	/api/Orders/user/{userId}	View own orders	Customer	View Orders	profile.component.ts (Orders Tab)
GET (by orderId)	/api/Orders/{orderId}	View order details	Customer	View Order Details	profile.component.ts (Orders Tab)
Products	GET	/api/Products	List all products	Customer	Browse Products	product-list.component.ts
GET (by id)	/api/Products/{id}	View product details	Customer	View Product Details	product-detail.component.ts
Users	GET (own profile)	/api/Users/me	View own profile	Customer	View Profile	profile.component.ts
GET, PUT (own profile)	/api/Users/{id}	View/update own profile	Customer	Edit Profile	profile.component.ts
Wishlist	GET (own wishlist)	/api/Wishlist/{userId}	View own wishlist	Customer	View Wishlist	wishlist.component.ts
POST, DELETE	/api/Wishlist/items	Add/remove items to/from wishlist	Customer	Add/Remove Wishlist Items	wishlist.component.ts
Addresses	GET	/api/users/{userId}/addresses	View own shipping addresses	Customer	View Addresses	addresses.component.ts
POST	/api/users/{userId}/addresses	Add new shipping address	Customer	Add Address	addresses.component.ts
PUT	/api/users/{userId}/addresses/{addressId}	Update shipping address	Customer	Edit Address	addresses.component.ts
DELETE	/api/users/{userId}/addresses/{addressId}	Delete shipping address	Customer	Delete Address	addresses.component.ts
PATCH	/api/users/{userId}/addresses/{addressId}	Set default shipping address	Customer	Set Default Address	addresses.component.ts  



**************************************************************************************************************************************************************************************************************************************************************************************************************************************************


# API_ENDPOINTS_UI_README.md

# API Endpoints and Frontend UI Mapping for Admin and Customer Roles

This document provides a comprehensive mapping of backend API endpoints to frontend UI components and actions for both **Admin** and **Customer** roles in your e-commerce app. It explains the purpose of each endpoint, who can access it, what frontend UI element corresponds, and typical real-world tasks involved.

---

## Table of Contents

1. [Admin Role Endpoints](#admin-role-endpoints)  
   - User  
   - Product  
   - Category  
   - Order  
   - Wishlist  
   - Address  
   - Auth  
   - Reports

2. [Customer Role Endpoints](#customer-role-endpoints)  
   - Auth  
   - Product  
   - Category  
   - Cart  
   - Wishlist  
   - Orders  
   - Users  
   - Addresses

3. [Quick Reference Tables](#quick-reference-tables)  
4. [Notes & Best Practices](#notes--best-practices)  

---

# Admin Role Endpoints

_All endpoints below are accessible only to admin users unless otherwise noted._

---

## User Resource

### GET

- **/api/Users**  
  - Access: Admin  
  - UI: **admin-user-list.component.ts**  
  - Action/Button: "View Users" button or page  
  - Description: Retrieve list of all users for management and monitoring.

- **/api/Users/{id}**  
  - Access: Admin  
  - UI: **admin-user-form.component.ts**  
  - Action/Button: "Edit User" on user list  
  - Description: Get details of a specific user by ID to view or edit profile.

### POST

- **/api/Users**  
  - Access: Admin  
  - UI: **admin-user-form.component.ts**  
  - Action/Button: "Add New User" form submit  
  - Description: Create a new user (admin can add users manually).

### PUT

- **/api/Users/{id}**  
  - Access: Admin  
  - UI: **admin-user-form.component.ts**  
  - Action/Button: "Save Changes" on user edit form  
  - Description: Update user profile details by ID.

### DELETE

- **/api/Users/{id}**  
  - Access: Admin  
  - UI: **admin-user-list.component.ts**  
  - Action/Button: "Delete User" button in user list  
  - Description: Remove user from system by ID.

---

## Product Resource

### GET

- **/api/Products**  
  - Access: Admin  
  - UI: **admin-product-list.component.ts**  
  - Action/Button: "View Products" page/tab  
  - Description: List all products available in the system.

- **/api/Products/{id}**  
  - Access: Admin  
  - UI: **admin-product-form.component.ts**  
  - Action/Button: "Edit Product" from product list  
  - Description: Get detailed info of a product by ID for editing.

### POST

- **/api/Products**  
  - Access: Admin  
  - UI: **admin-product-form.component.ts**  
  - Action/Button: "Add Product" form submit  
  - Description: Add new product details to catalog.

- **/api/Products/upload**  
  - Access: Admin  
  - UI: **admin-product-form.component.ts**, **admin-settings.component.ts**  
  - Action/Button: "Upload Image" button  
  - Description: Upload product images to associate with products.

### PUT

- **/api/Products/{id}**  
  - Access: Admin  
  - UI: **admin-product-form.component.ts**  
  - Action/Button: "Save Changes" on edit product form  
  - Description: Update product information by ID.

### DELETE

- **/api/Products/{id}**  
  - Access: Admin  
  - UI: **admin-product-list.component.ts**  
  - Action/Button: "Delete Product" button  
  - Description: Delete product from catalog by ID.

---

## Category Resource

### GET

- **/api/Categories**  
  - Access: Admin  
  - UI: **admin-category-list.component.ts**  
  - Action/Button: "View Categories" page  
  - Description: List all product categories.

- **/api/Categories/{id}**  
  - Access: Admin  
  - UI: **admin-category-form.component.ts**  
  - Action/Button: "Edit Category" button  
  - Description: Get details of a category by ID.

### POST

- **/api/Categories**  
  - Access: Admin  
  - UI: **admin-category-form.component.ts**  
  - Action/Button: "Add Category" submit  
  - Description: Add a new product category.

### PUT

- **/api/Categories/{id}**  
  - Access: Admin  
  - UI: **admin-category-form.component.ts**  
  - Action/Button: "Save Category" on edit form  
  - Description: Update existing category by ID.

### DELETE

- **/api/Categories/{id}**  
  - Access: Admin  
  - UI: **admin-category-list.component.ts**  
  - Action/Button: "Delete Category" button  
  - Description: Remove category by ID.

---

## Order Resource

### GET

- **/api/Orders**  
  - Access: Admin  
  - UI: **admin-orders-list.component.ts**  
  - Action/Button: "View Orders" page  
  - Description: View all customer orders for management.

- **/api/Orders/{orderId}**  
  - Access: Admin  
  - UI: **admin-order-detail.component.ts**  
  - Action/Button: "View Order Details" on order list  
  - Description: View details of a specific order.

---

## Wishlist Resource

### GET

- **/api/Wishlist/{userId}**  
  - Access: Admin  
  - UI: Partial/Optional (may not exist)  
  - Description: View wishlist for any user (not always exposed).

---

## Address Resource

### GET, POST, PUT, DELETE, PATCH

- **/api/users/{userId}/addresses**  
  - Access: Admin  
  - UI: Partial/Optional (may not exist)  
  - Description: Manage addresses for any user (view/add/edit/delete).

---

## Auth Resource

### POST

- **/api/Auth/register**  
  - Access: Admin  
  - UI: **register.component.ts**  
  - Action/Button: "Register" form  
  - Description: Register a new admin or user account.

- **/api/Auth/login**  
  - Access: Admin  
  - UI: **login.component.ts**  
  - Action/Button: "Login" form  
  - Description: Login to admin or user portal.

- **/api/Auth/change-password**  
  - Access: Admin  
  - UI: **settings.component.ts**  
  - Action/Button: "Change Password" in profile/settings  
  - Description: Change password for logged-in account.

---

## Reports (Custom)

- **Admin-specific report endpoints**  
  - Access: Admin  
  - UI: **admin-reports.component.ts**  
  - Description: View sales reports, category-wise analytics, and other metrics.

---

# Customer Role Endpoints

---

## Auth Resource

### POST

- **/api/Auth/register**  
  - Access: Customer  
  - UI: **register.component.ts**  
  - Action/Button: "Sign Up" form  
  - Description: Register a new customer account.

- **/api/Auth/login**  
  - Access: Customer  
  - UI: **login.component.ts**  
  - Action/Button: "Sign In" form  
  - Description: Login to customer account.

- **/api/Auth/change-password**  
  - Access: Customer  
  - UI: **settings.component.ts**  
  - Action/Button: "Change Password" in profile  
  - Description: Update account password.

---

## Product Resource

### GET

- **/api/Products**  
  - Access: Customer  
  - UI: **product-list.component.ts**  
  - Action/Button: "Browse Products" page  
  - Description: View all available products.

- **/api/Products/{id}**  
  - Access: Customer  
  - UI: **product-detail.component.ts**  
  - Action/Button: "View Details" on product list  
  - Description: View detailed product info.

---

## Category Resource

### GET

- **/api/Categories**  
  - Access: Customer  
  - UI: **product-list.component.ts**  
  - Action/Button: "Filter by Category" dropdown  
  - Description: View all categories.

- **/api/Categories/{id}**  
  - Access: Customer  
  - UI: **product-list.component.ts**  
  - Action/Button: Category filter selection  
  - Description: View products within a category.

---

## Cart Resource

### GET, POST, DELETE

- **/api/Cart**  
  - Access: Customer  
  - UI: **cart.component.ts**  
  - Action/Button: "View Cart" page  
  - Description: View current cart items, add new products, or clear cart.

- **/api/Cart/items/{itemId} (DELETE)**  
  - Access: Customer  
  - UI: **cart.component.ts**  
  - Action/Button: "Remove Item" button in cart  
  - Description: Remove a specific item from cart.

---

## Wishlist Resource

### GET, POST, DELETE

- **/api/Wishlist/{userId} (GET)**  
  - Access: Customer  
  - UI: **wishlist.component.ts**  
  - Action/Button: "View Wishlist" page  
  - Description: View user’s saved wishlist items.

- **/api/Wishlist/items (POST, DELETE)**  
  - Access: Customer  
  - UI: **wishlist.component.ts**  
  - Action/Button: "Add to Wishlist" or "Remove from Wishlist"  
  - Description: Add or remove items from wishlist.

---

## Orders Resource

### POST

- **/api/Orders**  
  - Access: Customer  
  - UI: **checkout.component.ts**  
  - Action/Button: "Place Order" on checkout page  
  - Description: Submit new order.

### GET

- **/api/Orders/user/{userId}**  
  - Access: Customer (own only)  
  - UI: **profile.component.ts (orders tab)**  
  - Action/Button: "View My Orders"  
  - Description: View list of own past orders.

- **/api/Orders/{orderId}**  
  - Access: Customer (own only)  
  - UI: **profile.component.ts (orders tab)**  
  - Action/Button: "Order Details" link  
  - Description: View details of a specific order.

---

## Users Resource

### GET

- **/api/Users/me**  
  - Access: Customer  
  - UI: **profile.component.ts**  
  - Action/Button: "My Profile"  
  - Description: View logged-in user’s profile.

- **/api/Users/{id} (GET, PUT)**  
  - Access: Customer (own only)  
  - UI: **profile.component.ts**  
  - Action/Button: "Edit Profile"  
  - Description: View and update own profile.

---

## Addresses Resource

### GET, POST, PUT, DELETE, PATCH

- **/api/users/{userId}/addresses**  
  - Access: Customer (own only)  
  - UI: **addresses.component.ts**  
  - Action/Button: "Manage Shipping Addresses"  
  - Description: Manage multiple shipping addresses.

---

# Quick Reference Tables

| Area      | Endpoint               | Admin Access | Customer Access | Frontend UI Available | UI Component                  | Description                          |
|-----------|------------------------|--------------|-----------------|----------------------|-------------------------------|------------------------------------|
| Auth      | /api/Auth/register     | ✅           | ✅              | ✅                   | register.component.ts         | Register account                   |
| Auth      | /api/Auth/login        | ✅           | ✅              | ✅                   | login.component.ts            | Login                             |
| Auth      | /api/Auth/change-password | ✅         | ✅              | ✅                   | settings.component.ts         | Change password                   |
| Products  | /api/Products          | ✅           | ✅              | ✅                   | admin-product-list / product-list.component.ts | View products                    |
| Products  | /api/Products/{id}     | ✅           | ✅              | ✅                   | admin-product-form / product-detail.component.ts | View/edit product details        |
| Products  | /api/Products/upload   | ✅           | ❌              | ✅                   | admin-product-form             | Upload product images             |
| Categories| /api/Categories        | ✅           | ✅              | ✅                   | admin-category-list / product-list.component.ts | View categories                  |
| Categories| /api/Categories/{id}   | ✅           | ✅              | ✅                   | admin-category-form / product-list.component.ts | View/edit category details       |
| Orders    | /api/Orders            | ✅           | ❌              | ✅                   | admin-orders-list             | View all orders                  |
| Orders    | /api/Orders/user/{userId} | ❌         | ✅ (own only)   | ✅                   | profile.component.ts          | View own orders                 |
| Cart      | /api/Cart              | ❌           | ✅              | ✅                   | cart.component.ts             | Manage cart                     |
| Wishlist  | /api/Wishlist/{userId} | ✅           | ✅              | ✅                   | wishlist.component.ts         | View/manage wishlist            |
| Users     | /api/Users             | ✅           | ❌              | ✅                   | admin-user-list.component.ts | Manage users                   |
| Users     | /api/Users/me          | ❌           | ✅              | ✅                   | profile.component.ts          | View own profile               |
| Addresses | /api/users/{userId}/addresses | ✅     | ✅ (own only)   | ✅                   | addresses.component.ts        | Manage addresses               |

---

# Notes & Best Practices

- **Role-based Access Control:** All endpoints are secured to allow access only for allowed roles. Frontend UI components should implement route guards accordingly.
- **Consistency:** Use same naming conventions in UI and API endpoints to ease maintenance.
- **Pagination & Filtering:** For list endpoints, ensure backend supports pagination and frontend provides UI filters to handle large data.
- **Error Handling:** Frontend should gracefully handle 4xx/5xx errors with user-friendly messages.
- **Secure Image Uploads:** Admin image uploads should validate and sanitize files to avoid security risks.
- **State Management:** Use Angular services and observables to manage state between components efficiently.

---

If you want me to generate diagrams or example API call snippets later, just ask!

---

*End of API_ENDPOINTS_UI_README.md*



