# Todo WebAPI

This project is a web API for managing a to-do list (Todo Items) with support for categories and user authentication. It uses an **InMemory** database, so no external database setup is required.

## Key Features

- **Todo Items Management**:
  - Add, edit, delete, and retrieve todo items.
  - Mark todo items as completed.
- **Categories**:
  - Each todo item can belong to a specific category (e.g., "Work", "Personal").
- **Authentication and Authorization**:
  - User registration and login.
  - JWT (JSON Web Token) for API security.

## Technologies

- **Backend**: ASP.NET Core
- **Database**: Entity Framework Core (InMemory)
- **Authentication**: JWT (JSON Web Token)
- **API Documentation**: Swagger/OpenAPI

## How to Run the Project

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- IDE (e.g., [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/))

### Steps

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/todo_webapi.git
   cd todo_webapi
2. Run the project:
   '''bash
   dotnet run
