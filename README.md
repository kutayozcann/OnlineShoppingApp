# 🛍️ Online Shopping App API

This is a graduation project developed for the Patika Web Back-End Development Bootcamp.  
It is a multi-layered ASP.NET Core Web API simulating the backend of an e-commerce system.  
The application is built with clean architecture principles and includes authentication, role management, order processing, and system-level middleware.

---

## 📦 Project Features

- ✅ **Layered Architecture**
  - Presentation Layer (Controllers, Filters, Middleware)
  - Business Layer (Service interfaces and implementations)
  - Data Access Layer (Repositories, UnitOfWork, EF Core context)
- ✅ **Entity Framework Core** with Code First Migrations
- ✅ **CRUD operations** for Products and Orders
- ✅ **JWT Authentication**
  - Secure login using JSON Web Tokens
  - Claims include email, name, role, ID
- ✅ **Role-based Authorization**
  - `[Authorize(Roles = "Admin")]` on protected routes
- ✅ **User Registration & Login**
  - Includes `Data Protection` to securely store encrypted passwords
- ✅ **Middleware**
  - Maintenance Mode Middleware (checks DB for system availability)
  - Global Exception Middleware (handles unexpected server errors)
- ✅ **Action Filter**
  - Restricts access to endpoints based on system time
- ✅ **Soft Delete Implementation**
  - Logical deletion using `IsDeleted` flag and global query filters
- ✅ **Model Validation**
  - Uses `[Required]`, `[EmailAddress]`, `[MaxLength]` etc.
- ✅ **Dependency Injection**
  - Services and repositories are registered via `Program.cs`
- ✅ **Global Exception Handling**
  - Standard error response for all uncaught exceptions
- ✅ **Swagger UI** for testing and documentation

---

## 🛠️ Technologies Used

- **ASP.NET Core Web API**
- **Entity Framework Core (Code First)**
- **SQL Server**
- **JWT Bearer Authentication**
- **Microsoft Data Protection API**
- **Custom Middleware & Filters**
- **Swagger / Swashbuckle**

---

## 🚀 Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/yourusername/online-shopping-api.git
cd online-shopping-api
```

### 2. Update Database Configuration
In `appsettings.json`, set your SQL Server connection string.

### 3. Apply Migrations
```bash
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```

Then visit:
```
https://localhost:{port}/swagger
```

---

## 👤 User Roles

- `Admin`
  - Add / edit / delete products
  - Toggle system maintenance
- `Customer`
  - Register / login
  - Create orders from products

JWT tokens are required for most endpoints. Admin-only routes are protected using role-based authorization.

---

## 🔍 Folder Structure

```
├── Controllers/
│   └── ProductsController.cs
│   └── OrdersController.cs
│   └── AuthController.cs
│
├── Business/
│   ├── Operations/
│   ├── Services/
│   └── Managers/
│
├── Data/
│   ├── Context/
│   ├── Entities/
│   ├── Repositories/
│   └── UnitOfWork/
│
├── Middleware/
│   └── GlobalExceptionMiddleware.cs
│   └── MaintenanceMiddleware.cs
```

---

## 🧪 Key Concepts Demonstrated

- Clean, layered backend design
- Separation of concerns
- Secure login and role management
- Transaction-safe order handling
- Action-based endpoint restriction
- Centralized error handling
- System toggle via database (maintenance mode)

---

## 🎓 Author

**Rahmi Kutay Özcan**  

---

## 📄 License

This project is for educational purposes and does not include a commercial license.
