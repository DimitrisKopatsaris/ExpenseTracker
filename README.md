# 💰 Expense Tracker API

A production-style **RESTful Expense Tracker API** built with **ASP.NET Core** and **Entity Framework Core**, designed following **clean architecture principles**.

This project demonstrates how to build a **maintainable backend service** with proper layering, dependency injection, and database persistence using EF Core.

---

## 🚀 Features
- Create, read, update, and delete expenses
- Accounts & categories management
- Clean architecture (Domain / Application / Infrastructure / API)
- Repository & service pattern
- DTOs and mapping with AutoMapper
- Centralized error handling
- Validation & business rules enforcement
- Swagger / OpenAPI documentation

---

## 🧱 Architecture
src/
├── ExpenseTracker.Api
│ ├── Controllers
│ ├── Program.cs
│ └── Middleware
│
├── ExpenseTracker.Application
│ ├── Interfaces
│ ├── Services
│ └── DTOs
│
├── ExpenseTracker.Domain
│ └── Entities
│
└── ExpenseTracker.Infrastructure
├── Repositories
└── Migrations

### Architecture Principles
- Controllers handle HTTP concerns only
- Services implement business logic
- Repositories abstract database access
- Domain contains core entities
- Infrastructure handles EF Core and database access

---

## 🛠 Tech Stack
- **C# / ASP.NET Core**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **Swagger / OpenAPI**
- **Dependency Injection**

---

## ▶️ Run Locally

### Prerequisites
- .NET SDK 8+
- SQL Server (local or Docker)

### Steps
git clone https://github.com/YOUR_GITHUB_USERNAME/expense-tracker-api.git
cd expense-tracker-api
dotnet restore
dotnet ef database update
dotnet run

---

Swagger UI is available at:
https://localhost:<configured-port>/swagger

---

📌 Example Endpoints
GET    /api/expenses
POST   /api/expenses
PUT    /api/expenses/{id}
DELETE /api/expenses/{id}

---

🧠 What This Project Demonstrates
Structuring a real-world ASP.NET Core API
Proper use of dependency injection and abstraction
Separation of concerns for maintainability
EF Core migrations and relational modeling
Writing APIs suitable for production environments

---

🔮 Future Improvements
Introduce enums for category types
Authentication & authorization (JWT)
Docker & Docker Compose setup
Observability (logging & metrics)
Pagination & filtering
Integration tests

---

📎 Why This Project Exists
This project was built as part of my backend engineering portfolio to demonstrate professional API design, not just basic CRUD functionality.
