# TwitterClone API

A RESTful API for a Twitter-like social platform, built with ASP.NET Core 9, Entity Framework Core, and JWT authentication. This project is currently in progress. The following features and endpoints are already implemented.

---

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
  - [Authentication](#authentication)
  - [Profile](#profile)
  - [Tweets](#tweets)
- [Project Structure](#project-structure)
- [Configuration](#configuration)
- [Planned Features](#planned-features)
- [License](#license)

---

## Features

- User registration and login with JWT authentication
- User profile management (view, edit, search)
- Tweet creation, editing, deletion, and timeline retrieval
- Entity Framework Core with SQL Server
- AutoMapper for DTO mapping
- Swagger/OpenAPI documentation

---

## Technologies Used

- **.NET 9 (ASP.NET Core 9)**
- **Entity Framework Core 9** (with SQL Server and Proxies)
- **AutoMapper** (object mapping)
- **Microsoft.AspNetCore.Identity** (user management)
- **JWT Bearer Authentication**
- **Swagger / Swashbuckle** (API documentation)
- **Microsoft.AspNetCore.OpenApi**
- **SwaggerUi** (interactive API docs)

NuGet packages (from `.csproj`):

- AutoMapper
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.OpenApi
- Microsoft.EntityFrameworkCore.Proxies
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- SwaggerUi
- Swashbuckle.AspNetCore

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local or remote)

### Setup

1. **Clone the repository:**
   ```bash
   git clone <repo-url>
   cd TwitterClone-API
   ```

2. **Configure the database:**
   - Update the `ConnectionStrings:Constr` in `appsettings.json` with your SQL Server details.

3. **Apply migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Run the API:**
   ```bash
   dotnet run
   ```
   The API will be available at `http://localhost:5206`.

5. **Swagger UI:**
   - Visit `http://localhost:5206/swagger` for interactive API documentation.

---

## API Endpoints

### Authentication

#### Register

- **Endpoint:** `POST /api/Account/register`
- **Description:** Registers a new user.
- **Request Body:**
  ```json
  {
    "firstName": "John",
    "lastName": "Doe",
    "userName": "johndoe",
    "email": "john@example.com",
    "phoneNumber": "1234567890",
    "password": "Password123",
    "confirmPassword": "Password123",
    "dateOfBirth": "2000-01-01",
    "profilePictureUrl": "https://example.com/profile.jpg"
  }
  ```
- **Response Example (Success):**
  ```json
  {
    "success": true,
    "response": "User registered successfully"
  }
  ```
- **Response Example (Failure):**
  ```json
  {
    "success": false,
    "response": ["User already exists"]
  }
  ```

#### Login

- **Endpoint:** `POST /api/Account/Login`
- **Description:** Authenticates a user and returns a JWT token.
- **Request Body:**
  ```json
  {
    "email": "john@example.com",
    "password": "Password123"
  }
  ```
- **Response Example (Success):**
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
  ```
- **Response Example (Failure):**
  ```json
  "Wrong Email Or Password"
  ```

---

### Profile

#### Get My Profile

- **Endpoint:** `GET /api/Profile/MyProfile`
- **Description:** Returns the authenticated user's profile.
- **Headers:**  
  `Authorization: Bearer <JWT_TOKEN>`
- **Response Example:**
  ```json
  {
    "success": true,
    "response": {
      "firstName": "John",
      "lastName": "Doe",
      "userName": "johndoe",
      "email": "john@example.com",
      "phoneNumber": "1234567890",
      "dateOfBirth": "2000-01-01",
      "bio": "Hello, I'm John!",
      "location": "New York",
      "tweets": [
        {
          "tweetText": "This is my first tweet!",
          "createdAt": "2024-07-07T12:00:00",
          "likesCount": 5,
          "commentsCount": 2
        }
      ]
    }
  }
  ```

#### Edit My Profile

- **Endpoint:** `POST /api/Profile/EditMyProfile`
- **Description:** Updates the authenticated user's profile.
- **Headers:**  
  `Authorization: Bearer <JWT_TOKEN>`
- **Request Body:**
  ```json
  {
    "firstName": "John",
    "lastName": "Doe",
    "userName": "johndoe",
    "email": "john@example.com",
    "phoneNumber": "1234567890",
    "dateOfBirth": "2000-01-01",
    "bio": "Updated bio",
    "location": "San Francisco",
    "tweets": []
  }
  ```
- **Response Example:**
  ```json
  {
    "success": true,
    "response": "Profile updated successfully"
  }
  ```

#### Search Profile

- **Endpoint:** `GET /api/Profile/Search/Profile/{username}`
- **Description:** Searches for a user profile by username.
- **Headers:**  
  `Authorization: Bearer <JWT_TOKEN>`
- **Response Example (Found):**
  ```json
  {
    "success": true,
    "response": {
      "userName": "janesmith",
      "dateOfBirth": "1995-05-10",
      "bio": "Hi, I'm Jane!",
      "location": "Los Angeles",
      "tweets": [
        {
          "tweetText": "Hello world!",
          "createdAt": "2024-07-07T13:00:00",
          "likesCount": 3,
          "commentsCount": 1
        }
      ]
    }
  }
  ```
- **Response Example (Not Found):**
  ```json
  {
    "success": false,
    "response": "User not found"
  }
  ```

---

### Tweets

#### Add Tweet

- **Endpoint:** `POST /api/Tweet/AddTweet`
- **Description:** Creates a new tweet for the authenticated user.
- **Headers:**  
  `Authorization: Bearer <JWT_TOKEN>`
- **Request Body:**
  ```json
  {
    "tweetText": "This is my first tweet!"
  }
  ```
- **Response Example:**
  ```json
  {
    "response": {
      "success": true,
      "response": "Tweet added successfully"
    },
    "newTweet": {
      "id": 1
    }
  }
  ```

#### Edit Tweet

- **Endpoint:** `POST /api/Tweet/EditTweet/{tweetId}`
- **Description:** Edits an existing tweet (only by the owner).
- **Headers:**  
  `Authorization: Bearer <JWT_TOKEN>`
- **Request Body:**
  ```json
  {
    "tweetText": "Updated tweet text"
  }
  ```
- **Response Example:**
  ```json
  {
    "success": true,
    "response": "Tweet updated successfully"
  }
  ```

#### Delete Tweet

- **Endpoint:** `DELETE /api/Tweet/DeleteTweet/{tweetId}`
- **Description:** Deletes a tweet (only by the owner).
- **Headers:**  
  `Authorization: Bearer <JWT_TOKEN>`
- **Response Example:**
  ```json
  {
    "success": true,
    "response": "Tweet deleted successfully"
  }
  ```

#### Get Timeline

- **Endpoint:** `GET /api/Tweet/GetAllTweets`
- **Description:** Returns tweets from users the authenticated user follows.
- **Headers:**  
  `Authorization: Bearer <JWT_TOKEN>`
- **Response Example:**
  ```json
  [
    [
      {
        "id": 1,
        "tweetText": "This is my first tweet!",
        "createdAt": "2024-07-07T12:00:00",
        "userId": "user-guid",
        "user": null,
        "comments": [],
        "likes": []
      }
    ]
    // ... more arrays for each followed user
  ]
  ```

---

## Project Structure

```
Controllers/         // API controllers
DataAccess/          // EF Core DbContext, repositories, migrations
Models/              // Domain models, DTOs, mapping profiles, responses
Program.cs           // Main entry point, service configuration
appsettings.json     // Configuration (connection strings, JWT, etc.)
```

---

## Configuration

- **Database:**  
  Set your SQL Server connection string in `appsettings.json` under `ConnectionStrings:Constr`.

- **JWT:**  
  Set your JWT secret and issuer in `appsettings.json` under the `JWT` section.

  Example:
  ```json
  {
    "ConnectionStrings": {
      "Constr": "Data Source=.;Initial Catalog=TwitterClone;Integrated Security=SSPI;TrustServerCertificate=true"
    },
    "JWT": {
      "SecretKey": "YourSuperSecretKey",
      "Issuer": "http://localhost:5206/"
    }
  }
  ```

---

## Planned Features

- Comments and likes on tweets
- Following/unfollowing users
---

## License

This project is for educational purposes and is not production-ready.

---

**Note:**  
This README covers only the features and endpoints that are currently implemented. As development progresses, more features and documentation will be added. 
