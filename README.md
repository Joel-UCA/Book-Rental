# Book Rental Management System

[![.NET Version](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-9.0.9-blue)](https://docs.microsoft.com/ef/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](CONTRIBUTING.md)

A comprehensive Book Rental Management System built with **ASP.NET Core 8.0** Web API, featuring JWT authentication, role-based authorization, and a complete book rental workflow.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Database Setup](#database-setup)
- [Project Structure](#project-structure)
- [API Documentation](#api-documentation)
- [Security](#security)
- [Database Schema](#database-schema)
- [Frontend Integration](#frontend-integration)
- [Development Guidelines](#development-guidelines)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

The Book Rental Management System is a modern web application that enables users to browse books, request rentals, and manage their rental history. Administrators can manage books, approve/reject rental requests, and monitor system statistics.

### Key Highlights

- **Clean Architecture**: Separation of concerns with Controllers, Services, Repositories, and Entities
- **Secure Authentication**: JWT-based authentication with BCrypt password hashing
- **Role-Based Access**: Admin and User roles with granular permissions
- **Complete Workflow**: Request → Approval → Rental → Return lifecycle
- **Performance Optimized**: Database indexes, AsNoTracking queries, and efficient data access
- **Production Ready**: Security headers, CORS configuration, exception handling

---

## Features

### User Features

- **Authentication**
  - User registration with email validation
  - Secure login with JWT token generation
  - Profile management (update info, change password)

- **Book Discovery**
  - Browse all available books
  - Search books by title or author
  - View book details and availability
  - Filter available books (stock > 0)

- **Rental Management**
  - Request books for rental
  - View request status (Pending/Approved/Rejected)
  - Cancel pending requests
  - View personal rental history
  - Track active and completed rentals

### Admin Features

- **User Management**
  - View all users with pagination
  - Update user information
  - Delete users
  - View user details

- **Book Management**
  - Add new books to catalog
  - Update book information
  - Delete books
  - Manage book stock levels
  - Direct book rental to users
  - Process book returns

- **Request Management**
  - View all rental requests
  - Filter requests by status
  - Approve rental requests (creates rental history, decreases stock)
  - Reject rental requests
  - Paginated request listing

- **Analytics**
  - Rental statistics (total, active, completed)
  - User rental histories
  - Active rental tracking

---

## Architecture

### Design Patterns

The application follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────────┐
│                  Presentation Layer                  │
│              (Controllers, DTOs)                     │
├─────────────────────────────────────────────────────┤
│                   Business Logic Layer               │
│              (Services, Interfaces)                  │
├─────────────────────────────────────────────────────┤
│                   Data Access Layer                  │
│          (Repositories, EF Core)                     │
├─────────────────────────────────────────────────────┤
│                     Database Layer                   │
│                 (SQL Server)                         │
└─────────────────────────────────────────────────────┘
```

### Key Patterns Implemented

1. **Repository Pattern**: Abstracts data access logic
2. **Dependency Injection**: All dependencies injected via constructor
3. **DTO Pattern**: Separates domain models from API contracts
4. **Middleware Pattern**: Cross-cutting concerns (exceptions, security)
5. **Service Layer Pattern**: Encapsulates business logic

---

## Tech Stack

### Backend

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server (with LocalDB support for development)
- **ORM**: Entity Framework Core 9.0.9
- **Authentication**: JWT (JSON Web Tokens)
- **Password Hashing**: BCrypt.Net-Next 4.0.3
- **API Documentation**: Swagger/OpenAPI

### Key NuGet Packages

```xml
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.9" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.9" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.9" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
```

---

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) or SQL Server LocalDB
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Postman](https://www.postman.com/) (optional, for API testing)

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/yourusername/book-rental.git
cd book-rental
```

2. **Restore dependencies**

```bash
cd "Book Rental"
dotnet restore
```

3. **Install Entity Framework tools** (if not already installed)

```bash
dotnet tool install --global dotnet-ef
```

### Configuration

#### Step 1: Set Up User Secrets (Development)

User Secrets keep sensitive data out of your source code. Configure them as follows:

```bash
cd "Book Rental"
dotnet user-secrets init
```

#### Step 2: Configure JWT Secret

Set a strong JWT secret key (minimum 32 characters):

```bash
dotnet user-secrets set "Jwt:Secret" "YourStrongSecretKeyHereAtLeast32CharactersLongForSecurity"
```

Example:

```bash
dotnet user-secrets set "Jwt:Secret" "A8f2K9mP3vN7xQ1wE5tR6yU4iO0pL2sD3gH8jK5lZ9xC7vB1nM4qW6eR8tY0uI3o"
```

#### Step 3: Configure Admin User

Set up the default admin user credentials:

```bash
dotnet user-secrets set "AdminUser:Email" "admin@bookrental.com"
dotnet user-secrets set "AdminUser:Password" "Admin@123"
dotnet user-secrets set "AdminUser:FullName" "System Administrator"
```

#### Step 4: Verify Secrets

```bash
dotnet user-secrets list
```

You should see:

```
Jwt:Secret = [Your secret key]
AdminUser:Email = admin@bookrental.com
AdminUser:Password = Admin@123
AdminUser:FullName = System Administrator
```

#### Production Configuration

For production, use **environment variables** instead of User Secrets:

**Azure App Service:**

```
Jwt__Secret = YourProductionSecretKey
Jwt__Issuer = BookRentalApp
Jwt__Audience = BookRentalUsers
Jwt__ExpirationHours = 1
AdminUser__Email = admin@yourdomain.com
AdminUser__Password = SecurePassword123!
```

**Note**: Use double underscore (`__`) for nested configuration in Azure.

### Database Setup

#### Step 1: Update Connection String (Optional)

The default connection string uses SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookRentalDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

For a different SQL Server instance, update `appsettings.json`.

#### Step 2: Run Migrations

Apply database migrations to create the schema:

```bash
dotnet ef database update
```

This will create:
- `Users` table
- `Books` table
- `BookRequests` table
- `RentalHistories` table
- Database indexes for performance

#### Step 3: Run the Application

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger`

#### Step 4: Test the Admin User

The admin user is automatically created on first run. Test login:

**Endpoint**: `POST /api/users/login`

**Body**:
```json
{
  "email": "admin@bookrental.com",
  "password": "Admin@123"
}
```

**Response**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

## Project Structure

```
Book Rental/
├── Controllers/              # API Controllers
│   ├── BooksController.cs
│   ├── BookRequestsController.cs
│   ├── RentalHistoriesController.cs
│   └── UsersController.cs
│
├── Services/                 # Business Logic Layer
│   ├── BookService.cs
│   ├── BookRequestService.cs
│   ├── RentalHistoryService.cs
│   └── UserService.cs
│
├── Repositories/             # Data Access Layer
│   ├── BookRepository.cs
│   ├── BookRequestRepository.cs
│   ├── RentalHistoryRepository.cs
│   └── UserRepository.cs
│
├── Entities/                 # Domain Models
│   ├── Book.cs
│   ├── BookRequest.cs
│   ├── RentalHistory.cs
│   └── User.cs
│
├── DTOs/                     # Data Transfer Objects
│   ├── Requests/
│   │   ├── BookRequestDto.cs
│   │   ├── ChangePasswordRequestDto.cs
│   │   ├── LoginRequestDto.cs
│   │   ├── UpdateProfileRequestDto.cs
│   │   └── UserRequestDto.cs
│   └── Responses/
│       ├── BookRequestResponseDto.cs
│       ├── BookResponseDto.cs
│       ├── LoginResponseDto.cs
│       ├── RentalHistoryResponseDto.cs
│       └── UserResponseDto.cs
│
├── Interfaces/               # Service and Repository Interfaces
│   ├── IBookRepository.cs
│   ├── IBookRequestRepository.cs
│   ├── IBookRequestService.cs
│   ├── IBookService.cs
│   ├── IRentalHistoryRepository.cs
│   ├── IRentalHistoryService.cs
│   ├── IUserRepository.cs
│   └── IUserService.cs
│
├── Middleware/               # Cross-Cutting Concerns
│   ├── GlobalExceptionHandlerMiddleware.cs
│   └── SecurityHeadersMiddleware.cs
│
├── Exceptions/               # Custom Exception Classes
│   ├── BadRequestException.cs
│   ├── ConflictException.cs
│   ├── ForbiddenException.cs
│   ├── NotFoundException.cs
│   └── UnauthorizedException.cs
│
├── Models/                   # Helper Models
│   ├── ErrorResponse.cs
│   ├── PagedResult.cs
│   └── PaginationParams.cs
│
├── Constants/                # Application Constants
│   ├── Roles.cs
│   ├── RequestStatus.cs
│   └── ValidationMessages.cs
│
├── Data/                     # Database Context
│   ├── AppDbContext.cs
│   └── DbSeeder.cs
│
├── Migrations/               # EF Core Migrations
│   └── [Migration files]
│
├── appsettings.json          # Application Configuration
├── appsettings.Development.json
├── Program.cs                # Application Entry Point
└── Book Rental.csproj        # Project File
```

---

## API Documentation

### Base URL

- Development: `https://localhost:5001/api`
- Production: `https://api.yourdomain.com/api`

### Authentication

Most endpoints require a JWT token in the Authorization header:

```
Authorization: Bearer {your-jwt-token}
```

### Endpoints Overview

#### Authentication & Users

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/users/register` | Register new user | Public |
| POST | `/users/login` | Login and get JWT token | Public |
| GET | `/users/me` | Get current user profile | Required |
| PUT | `/users/me` | Update profile | Required |
| PUT | `/users/me/password` | Change password | Required |
| GET | `/users` | Get all users (paginated) | Admin |
| GET | `/users/{id}` | Get user by ID | Admin |
| PUT | `/users/{id}` | Update user | Admin |
| DELETE | `/users/{id}` | Delete user | Admin |

#### Books

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/books` | Get all books (paginated) | Public |
| GET | `/books/{id}` | Get book by ID | Public |
| GET | `/books/search?query={term}` | Search books | Public |
| GET | `/books/available` | Get available books | Public |
| POST | `/books` | Add new book | Admin |
| PUT | `/books/{id}` | Update book | Admin |
| DELETE | `/books/{id}` | Delete book | Admin |
| POST | `/books/{id}/rent/{userId}` | Rent book | Admin |
| POST | `/books/{id}/return` | Return book | Admin |

#### Book Requests

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/book-requests/{bookId}` | Request a book | Required |
| GET | `/book-requests/my-requests` | Get user's requests | Required |
| GET | `/book-requests` | Get all requests (paginated) | Admin |
| GET | `/book-requests/status/{status}` | Filter by status | Admin |
| PUT | `/book-requests/{id}/approve` | Approve request | Admin |
| PUT | `/book-requests/{id}/reject` | Reject request | Admin |
| DELETE | `/book-requests/{id}` | Cancel request | Required |

#### Rental History

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/rental-histories/my-history` | Get user's history | Required |
| GET | `/rental-histories/user/{userId}` | Get user's history | Admin |
| GET | `/rental-histories` | Get all histories | Admin |
| GET | `/rental-histories/active` | Get active rentals | Admin |
| GET | `/rental-histories/statistics` | Get statistics | Admin |

### Request/Response Examples

#### Register User

**Request**: `POST /api/users/register`

```json
{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "Pass@123"
}
```

**Response**: `200 OK`

```json
{
  "message": "User registered successfully."
}
```

#### Login

**Request**: `POST /api/users/login`

```json
{
  "email": "john@example.com",
  "password": "Pass@123"
}
```

**Response**: `200 OK`

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI..."
}
```

#### Get Books (Paginated)

**Request**: `GET /api/books?pageNumber=1&pageSize=10`

**Response**: `200 OK`

```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "title": "Clean Code",
      "author": "Robert C. Martin",
      "stock": 5
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 50,
  "totalPages": 5,
  "hasPrevious": false,
  "hasNext": true
}
```

#### Request a Book

**Request**: `POST /api/book-requests/{bookId}`

**Headers**: `Authorization: Bearer {token}`

**Response**: `200 OK`

```json
{
  "message": "Book request submitted successfully."
}
```

### Error Responses

All errors follow a consistent format:

```json
{
  "statusCode": 400,
  "message": "Error message here",
  "details": "Stack trace (development only)",
  "timestamp": "2025-01-27T10:30:00Z"
}
```

**Status Codes**:
- `200 OK` - Success
- `201 Created` - Resource created
- `204 No Content` - Success with no body
- `400 Bad Request` - Validation error
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `409 Conflict` - Resource conflict (e.g., duplicate email)
- `500 Internal Server Error` - Server error

---

## Security

### Implemented Security Features

#### 1. Authentication & Authorization

- **JWT (JSON Web Tokens)** for stateless authentication
- **BCrypt** password hashing with salt
- **Role-based authorization** (Admin/User)
- **Claims-based identity** (UserId, Role in JWT)

#### 2. Password Security

- Minimum 8 characters required
- Must contain:
  - Uppercase letter
  - Lowercase letter
  - Number
  - Special character (`@$!%*?&`)
- Current password verification for password changes

#### 3. Security Headers

Implemented via `SecurityHeadersMiddleware`:

```
X-Content-Type-Options: nosniff
X-Frame-Options: DENY
X-XSS-Protection: 1; mode=block
Referrer-Policy: strict-origin-when-cross-origin
Strict-Transport-Security: max-age=31536000
Content-Security-Policy: default-src 'self'
```

#### 4. CORS Configuration

- **Development**: Allows `localhost:4200`, `localhost:3000`, `localhost:5173`
- **Production**: Configurable allowed origins in `appsettings.json`
- Credentials enabled for cookie-based auth

#### 5. Exception Handling

- Global exception handler middleware
- Custom exception types for different HTTP status codes
- Stack traces only shown in development mode
- Detailed error messages for debugging

#### 6. Input Validation

- Data annotations on all DTOs
- Email format validation
- Password complexity validation
- Pagination parameter validation
- Model state validation in controllers

### Security Best Practices

**Secrets Management**:
- ✅ JWT secrets stored in User Secrets (dev) or environment variables (prod)
- ✅ No hardcoded credentials in source code
- ✅ Admin credentials configurable via secrets

**Database Security**:
- ✅ Parameterized queries (EF Core)
- ✅ SQL injection protection
- ✅ Unique indexes on email

**API Security**:
- ✅ HTTPS redirection enforced
- ✅ CORS properly configured
- ✅ Authorization checks on all protected endpoints

### Pending Security Enhancements

⚠️ **Recommended for Production**:

1. **Rate Limiting** - Prevent brute force attacks
2. **Account Lockout** - Lock accounts after failed login attempts
3. **Input Sanitization** - XSS protection
4. **Request Size Limits** - DoS protection
5. **Audit Logging** - Track critical operations

See [COMPREHENSIVE_NEXT_PLAN.md](COMPREHENSIVE_NEXT_PLAN.md) for implementation details.

---

## Database Schema

### Tables

#### Users

| Column | Type | Constraints |
|--------|------|-------------|
| Id | UNIQUEIDENTIFIER | PRIMARY KEY |
| FullName | NVARCHAR(100) | NOT NULL |
| Email | NVARCHAR(100) | NOT NULL, UNIQUE |
| Password | NVARCHAR(MAX) | NOT NULL (BCrypt hash) |
| Role | NVARCHAR(50) | NOT NULL, DEFAULT 'User' |

**Indexes**:
- Unique index on `Email`

#### Books

| Column | Type | Constraints |
|--------|------|-------------|
| Id | UNIQUEIDENTIFIER | PRIMARY KEY |
| Title | NVARCHAR(200) | NOT NULL |
| Author | NVARCHAR(100) | NOT NULL |
| Stock | INT | NOT NULL, >= 0 |

#### BookRequests

| Column | Type | Constraints |
|--------|------|-------------|
| Id | UNIQUEIDENTIFIER | PRIMARY KEY |
| BookId | UNIQUEIDENTIFIER | FOREIGN KEY → Books(Id) |
| UserId | UNIQUEIDENTIFIER | FOREIGN KEY → Users(Id) |
| RequestDate | DATETIME2 | NOT NULL |
| Status | NVARCHAR(50) | NOT NULL ('Pending', 'Approved', 'Rejected') |

**Indexes**:
- Index on `Status`
- Index on `UserId`
- Index on `BookId`
- Composite index on `(UserId, Status)`

#### RentalHistories

| Column | Type | Constraints |
|--------|------|-------------|
| Id | UNIQUEIDENTIFIER | PRIMARY KEY |
| BookId | UNIQUEIDENTIFIER | FOREIGN KEY → Books(Id) |
| UserId | UNIQUEIDENTIFIER | FOREIGN KEY → Users(Id) |
| RentalDate | DATETIME2 | NOT NULL |
| ReturnDate | DATETIME2 | NULLABLE |

**Indexes**:
- Index on `UserId`
- Index on `BookId`
- Composite index on `(BookId, ReturnDate)`
- Composite index on `(UserId, ReturnDate)`

### Relationships

```
Users (1) ←──→ (N) BookRequests
Users (1) ←──→ (N) RentalHistories
Books (1) ←──→ (N) BookRequests
Books (1) ←──→ (N) RentalHistories
```

---

## Frontend Integration

### Quick Start for Angular/React/Vue

#### 1. API Configuration

**Environment Setup** (Angular example):

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

#### 2. Models/Interfaces

```typescript
export interface Book {
  id: string;
  title: string;
  author: string;
  stock: number;
}

export interface User {
  id: string;
  fullName: string;
  email: string;
  role: 'Admin' | 'User';
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}
```

#### 3. HTTP Service Example

```typescript
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({ providedIn: 'root' })
export class BookService {
  private apiUrl = `${environment.apiUrl}/books`;

  constructor(private http: HttpClient) {}

  getBooks(pageNumber?: number, pageSize?: number): Observable<any> {
    const params: any = {};
    if (pageNumber) params.pageNumber = pageNumber;
    if (pageSize) params.pageSize = pageSize;

    return this.http.get(this.apiUrl, { params });
  }

  getBookById(id: string): Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }

  createBook(book: any): Observable<void> {
    return this.http.post<void>(this.apiUrl, book);
  }
}
```

#### 4. Auth Interceptor

```typescript
import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = localStorage.getItem('token');

    if (token) {
      const cloned = req.clone({
        headers: req.headers.set('Authorization', `Bearer ${token}`)
      });
      return next.handle(cloned);
    }

    return next.handle(req);
  }
}
```

For complete integration guide, see:
- [ANGULAR_INTEGRATION_GUIDE.md](ANGULAR_INTEGRATION_GUIDE.md)
- [API_CONNECTION_GUIDE.md](API_CONNECTION_GUIDE.md)

---

## Development Guidelines

### Code Standards

#### Naming Conventions

- **PascalCase**: Classes, methods, properties, public members
- **camelCase**: Private fields, local variables, parameters
- **UPPER_CASE**: Constants

```csharp
public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public async Task<BookResponseDto> GetByIdAsync(Guid id)
    {
        var book = await _repository.GetByIdAsync(id);
        return book;
    }
}
```

#### Async/Await

Always use async/await for I/O operations:

```csharp
// ✅ Good
public async Task<Book> GetBookAsync(Guid id)
{
    return await _repository.GetByIdAsync(id);
}

// ❌ Bad
public Book GetBook(Guid id)
{
    return _repository.GetByIdAsync(id).Result;
}
```

#### Exception Handling

Use custom exceptions for domain errors:

```csharp
if (book == null)
    throw new NotFoundException("Book", id);

if (book.Stock <= 0)
    throw new BadRequestException("Book is out of stock");
```

#### Validation

Use data annotations for input validation:

```csharp
public class UserRequestDto
{
    [Required(ErrorMessage = ValidationMessages.FullNameRequired)]
    [MaxLength(100)]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = ValidationMessages.EmailRequired)]
    [EmailAddress]
    public string Email { get; set; } = "";
}
```

### Repository Pattern

```csharp
public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task<Book> AddAsync(Book book);
    Task<Book> UpdateAsync(Book book);
    Task DeleteAsync(Guid id);
}

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .ToListAsync();
    }
}
```

### Service Layer

```csharp
public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public async Task<BookResponseDto> GetByIdAsync(Guid id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book == null)
            throw new NotFoundException("Book", id);

        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Stock = book.Stock
        };
    }
}
```

### Dependency Injection

Register all services in `Program.cs`:

```csharp
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
```

---

## Testing

### Unit Tests (Recommended)

**Create Test Project**:

```bash
dotnet new xunit -n BookRental.Tests
cd BookRental.Tests
dotnet add reference "../Book Rental/Book Rental.csproj"
dotnet add package Moq
dotnet add package FluentAssertions
```

**Example Test**:

```csharp
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _mockConfig = new Mock<IConfiguration>();
        _service = new UserService(_mockRepo.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowConflictException_WhenUserExists()
    {
        // Arrange
        var existingUser = new User { Email = "test@example.com" };
        _mockRepo.Setup(r => r.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(existingUser);

        var userDto = new UserRequestDto
        {
            Email = "test@example.com",
            FullName = "Test User",
            Password = "Password123!"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(
            () => _service.RegisterUserAsync(userDto));
    }
}
```

**Run Tests**:

```bash
dotnet test
```

**Target**: 80%+ code coverage

---

## Deployment

### Production Checklist

Before deploying to production:

- [ ] Set JWT secret in environment variables
- [ ] Set admin credentials in environment variables
- [ ] Configure CORS with production frontend URLs
- [ ] Update connection string for production database
- [ ] Enable HTTPS redirection
- [ ] Set up logging (Serilog or Application Insights)
- [ ] Configure health checks
- [ ] Add rate limiting
- [ ] Set up monitoring/alerting
- [ ] Review and update security headers
- [ ] Run database migrations in production
- [ ] Test all endpoints in production environment

### Environment Variables (Production)

```bash
# JWT Configuration
Jwt__Secret=YourProductionSecretKeyHere
Jwt__Issuer=BookRentalApp
Jwt__Audience=BookRentalUsers
Jwt__ExpirationHours=1

# Admin User
AdminUser__Email=admin@yourdomain.com
AdminUser__Password=SecurePassword123!
AdminUser__FullName=System Administrator

# Database
ConnectionStrings__DefaultConnection=Server=...;Database=...;
```

### Azure Deployment

1. **Create Azure Resources**
   - Azure App Service (ASP.NET Core 8.0)
   - Azure SQL Database

2. **Configure App Service**
   - Set environment variables in Application Settings
   - Enable HTTPS only
   - Configure CORS in Azure portal

3. **Deploy Application**

```bash
dotnet publish -c Release
# Upload to Azure App Service
```

4. **Run Migrations**

```bash
dotnet ef database update --connection "YourProductionConnectionString"
```

### Docker Deployment (Optional)

**Dockerfile**:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Book Rental/Book Rental.csproj", "Book Rental/"]
RUN dotnet restore "Book Rental/Book Rental.csproj"
COPY . .
WORKDIR "/src/Book Rental"
RUN dotnet build "Book Rental.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Book Rental.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Book Rental.dll"]
```

**Build and Run**:

```bash
docker build -t book-rental-api .
docker run -p 8080:80 -e Jwt__Secret="YourSecret" book-rental-api
```

---

## Contributing

We welcome contributions! Please follow these guidelines:

### How to Contribute

1. **Fork the repository**
2. **Create a feature branch**

```bash
git checkout -b feature/YourFeatureName
```

3. **Make your changes**
   - Follow code standards
   - Add tests for new features
   - Update documentation

4. **Commit your changes**

```bash
git add .
git commit -m "Add feature: YourFeatureName"
```

5. **Push to your fork**

```bash
git push origin feature/YourFeatureName
```

6. **Create a Pull Request**

### Code Review Process

- All PRs require review before merging
- Ensure all tests pass
- Follow existing code style
- Update documentation as needed

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## Additional Resources

### Documentation

- [Backend Analysis Summary](BACKEND_ANALYSIS_SUMMARY.md) - Comprehensive backend overview
- [Code Analysis Report](CODE_ANALYSIS_REPORT.md) - Detailed code quality analysis
- [Security Issues Quick Reference](SECURITY_ISSUES_QUICK_REFERENCE.md) - Security checklist
- [Best Practices & Improvements](BEST_PRACTICES_AND_IMPROVEMENTS.md) - Code quality guide
- [Setup Secrets Guide](SETUP_SECRETS_GUIDE.md) - Secrets configuration
- [Fixes Summary](FIXES_SUMMARY.md) - Applied fixes and improvements
- [Production Bug Fixes](PRODUCTION_BUG_FIXES.md) - Bug fix documentation
- [Next Steps](NEXT_STEPS.md) - Immediate action items
- [Comprehensive Next Plan](COMPREHENSIVE_NEXT_PLAN.md) - Future roadmap
- [Final Concerns & Recommendations](FINAL_CONCERNS_AND_RECOMMENDATIONS.md) - Final review
- [Angular Integration Guide](ANGULAR_INTEGRATION_GUIDE.md) - Frontend integration
- [API Connection Guide](API_CONNECTION_GUIDE.md) - Detailed API usage

### Support

- **Issues**: [GitHub Issues](https://github.com/yourusername/book-rental/issues)
- **Discussions**: [GitHub Discussions](https://github.com/yourusername/book-rental/discussions)
- **Email**: support@yourdomain.com

---

## Acknowledgments

- Built with [ASP.NET Core](https://docs.microsoft.com/aspnet/core/)
- Authentication with [JWT](https://jwt.io/)
- Password hashing with [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net)
- ORM with [Entity Framework Core](https://docs.microsoft.com/ef/core/)

---

**Last Updated**: 2025-01-27
**Version**: 1.0.0
**Status**: Production Ready (with recommended enhancements)

---

Made with ❤️ by Your Team
