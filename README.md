# KyberKlass 🎓  
**School Management System — Built with ASP.NET Core MVC**

**KyberKlass** is a modular, extensible school management platform built with ASP.NET Core MVC and Entity Framework Core. The current version provides full administrative control over users, schools, classrooms, teachers, students, and guardians. Future support is planned for Guardians, Teachers, and Students with tailored views and functionality.

## 🧠 Project Philosophy

KyberKlass is designed to be:
- 🔐 **Secure**: Role-based access control using ASP.NET Identity
- ⚙️ **Clean Architecture** — Service-layer abstraction for domain logic
- 🧩 **Modularity** — Organized ViewModels, Extension Methods, and Scalable Routing
- 🔍 **Maintainable**: Separation of concerns, ViewModels, and extensions

## 🔐 Authentication & Roles

- Managed by ASP.NET Identity with GUID-based users
- Roles: `Admin`, `Teacher`, `Guardian`, `Student`
- Role-specific access guards in views and controllers

## ✅ Core Features (Admin)

Currently, the `Admin` role provides full CRUD and user management access. Admins can:

### 🏫 School Management
- Create, update, delete, and view schools
- Assign classrooms to schools

### 🧑‍🏫 Teacher Management
- Create, view teacher profiles
- Assign to classrooms

### 🎓 Student Management
- Create and manage students
- Assign/change guardians

### 🏠 Classroom Management
- Add/edit/delete classrooms
- Associate with schools and teachers

### 👥 User Management
- Assign roles to users
- Update user info and access
- View user details

## 🔜 Planned Features

### 👨‍🏫 Teacher Role
- View assigned classrooms
- View students in those classrooms
- Write grades based on subjects (requires **Subjects** model)

### 👪 Guardian Role
- View list of their children
- See children's classrooms
- View children's grades (after grade module is ready)

### 🎓 Student Role
- View own information (name, guardian, classroom)
- View grades (after grade module is ready)

### 🔍 Admin Role Enhancements
- Implement a **search/filter** system for users (by name, ID, etc.) to streamline user management.

## 🛠️ Technology Stack
| Layer         | Technology                              |
|---------------|-----------------------------------------|
| Backend       | ASP.NET Core MVC, Entity Framework Core |
| Authentication| ASP.NET Core Identity (Role-Based)      |
| Frontend      | Razor Views, Partial Views              |
| Database      | SQL Server                              |
| Dev Tools     | Visual Studio                           |


