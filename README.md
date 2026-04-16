# 🚀 .NET CMS Project (Blazor WASM + Clean Architecture)

This is a full-stack Content Management System (CMS) featuring a role-based access model (Admin / Employee). 

**The main goal of this project** is the continuous development of my technical skills, diving deep into the .NET ecosystem, and honing architectural patterns. The project is actively evolving and serves as my personal playground for implementing software development best practices.

## 🛠 Tech Stack
* **Backend:** ASP.NET Core Web API (.NET 9)
* **Frontend:** Blazor WebAssembly + MudBlazor (UI Components)
* **Architecture:** Clean Architecture, CQRS
* **Database:** SQL + Dapper (micro-ORM)
* **Communication:** MediatR
* **Authorization:** JWT Tokens, Role-Based Access Control (RBAC)

## 📦 Architectural Highlights
The project is strictly divided into independent layers (Domain, Application, Infrastructure, Api). 
Recently, a major architectural refactoring was completed: **an independent `Shared` project was extracted**. Now, all Data Transfer Objects (DTOs), Commands, and Enums are stored in a single place and reused by both the backend and the frontend. This completely eliminated code duplication and made the API contracts absolutely transparent.

## 🗺 Roadmap

I treat this project as a "living" organism that will be continuously improved. Upcoming plans include:

- [x] Implementation of CQRS using MediatR.
- [x] Breaking down the monolith and extracting common contracts into a `Shared` library.
- [ ] **Database Architecture Refactoring:** Optimizing entity relationships (Many-to-Many), normalizing tables, and resolving technical debt.
- [ ] **Frontend Enhancements:** Improving UI/UX, implementing Skeleton loaders, enforcing strict role-based routing (redirects to `/login`), and polishing MudBlazor components.
- [ ] **Feature Expansion:** Adding comprehensive contract editing and status management capabilities.
- [ ] **Test Coverage:** Writing Unit tests for key services.

---
*Built for learning, experimentation, and a love for clean code.* 💻
