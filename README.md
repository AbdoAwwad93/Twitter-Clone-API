# Twitter Clone API

A comprehensive REST API built with ASP.NET Core that replicates core Twitter functionality including user authentication, tweeting, following, commenting, and social interactions.

## 📋 Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Installation](#installation)
- [Configuration](#configuration)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Database Schema](#database-schema)
- [Response Format](#response-format)
- [Error Handling](#error-handling)
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

### Core Functionality
- **User Authentication & Authorization** - JWT-based secure authentication with 12-month token expiration
- **Profile Management** - Complete user profile CRUD operations with secure deletion
- **Tweet Operations** - Create, read, update, and delete tweets with ownership validation
- **Social Features** - Follow/unfollow users, view followers and followings
- **Comment System** - Comment on tweets with full CRUD operations and ownership validation
- **Social Interactions** - Like/unlike tweets and comments with duplicate prevention
- **Timeline** - View tweets from followed users (personalized feed)

### Advanced Features
- **Mutual Followers** - Discover mutual connections between users
- **User Search** - Search and view other user profiles (excluding own profile)
- **Follow Statistics** - Track follower and following counts
- **Account Management** - Secure account deletion with password verification
- **User Activity Tracking** - View personal likes and replies
- **Relationship Validation** - Prevent self-following and duplicate relationships

## 🛠️ Technologies Used

### Backend Framework
- **ASP.NET Core** - Modern, cross-platform web framework
- **Entity Framework Core** - Object-relational mapping (ORM)
- **ASP.NET Core Identity** - Authentication and authorization system

### Database
- **SQL Server** - Primary database management system
- **Entity Framework Core with Lazy Loading** - Database access layer

### Authentication & Security
- **JWT (JSON Web Tokens)** - Stateless authentication
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT authentication middleware
- **ASP.NET Core Identity** - User management

### Development Tools
- **AutoMapper** - Object-to-object mapping
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management

### Additional Libraries
- **Microsoft.IdentityModel.Tokens** - Token validation
- **Microsoft.EntityFrameworkCore.Proxies** - Lazy loading support
- **System.IdentityModel.Tokens.Jwt** - JWT token handling

## 🏗️ Architecture

### Design Patterns
- **Repository Pattern** - Abstracts data access logic
- **Unit of Work Pattern** - Manages database transactions
- **Dependency Injection** - Promotes loose coupling and testability
- **DTO Pattern** - Data transfer objects for API communication
- **AutoMapper Integration** - Seamless object mapping

### Project Structure
```
TwitterClone_API/
├── Controllers/          # API endpoints
│   ├── AccountController.cs      # Authentication & registration
│   ├── ProfileController.cs      # Profile management
│   ├── TweetController.cs        # Tweet operations
│   ├── CommentController.cs      # Comment system
│   └── FollowController.cs       # Social features
├── DataAccess/          # Data access layer
│   └── Repo/           # Repository implementations
│       └── UnitOfWork/ # Unit of work pattern
├── Models/             # Data models and DTOs
│   ├── AppModels/      # Entity models
│   ├── DTOs/           # Data transfer objects
│   ├── Mapping/        # AutoMapper profiles
│   └── Response/       # API response models
└── Program.cs          # Application configuration
```

## 🚀 Installation

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

### Setup Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd TwitterClone_API
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection string**
   - Open `appsettings.json`
   - Update the `ConnectionStrings:Constr` value

4. **Configure JWT settings**
   - Add JWT configuration in `appsettings.json`:
   ```json
   {
     "JWT": {
       "SecretKey": "your-secret-key-here-minimum-32-characters",
       "Issuer": "TwitterCloneAPI"
     }
   }
   ```

5. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

6. **Run the application**
   ```bash
   dotnet run
   ```

## ⚙️ Configuration

### JWT Configuration
```json
{
  "JWT": {
    "SecretKey": "your-super-secret-key-minimum-32-characters",
    "Issuer": "TwitterCloneAPI"
  }
}
```

### Database Configuration
```json
{
  "ConnectionStrings": {
    "Constr": "Server=(localdb)\\MSSQLLocalDB;Database=TwitterCloneDB;Trusted_Connection=true;"
  }
}
```

### CORS Configuration
The API is configured to allow cross-origin requests from any origin (development only).

## 📚 API Endpoints

### Authentication (No Auth Required)
- `POST /api/Account/register` - Register new user
- `POST /api/Account/Login` - User login

### Profile Management (Auth Required)
- `GET /api/Profile/MyProfile` - Get current user profile
- `GET /api/Profile/Search/Profile/{username}` - Search user profile
- `POST /api/Profile/EditMyProfile` - Update profile
- `DELETE /api/Profile/DeleteMyProfile` - Delete account (requires password confirmation)
- `GET /api/Profile/MyLikes` - Get current user's liked tweets
- `GET /api/Profile/MyReplies` - Get current user's comments

### Tweet Operations (Auth Required)
- `POST /api/Tweet/AddTweet` - Create new tweet
- `GET /api/Tweet/GetAllTweets` - Get timeline tweets from followed users
- `GET /api/Tweet/GetTweet{tweetId}` - Get specific tweet by ID
- `POST /api/Tweet/EditTweet/{tweetId}` - Update tweet (owner only)
- `DELETE /api/Tweet/DeleteTweet/{tweetId}` - Delete tweet (owner only)
- `POST /api/Tweet/Like/{tweetId}` - Like a tweet
- `POST /api/Tweet/Unlike/{tweetId}` - Unlike a tweet

### Comment System (Auth Required)
- `POST /api/Comment/AddComment/{tweetId}` - Add comment to tweet
- `GET /api/Comment/GetCommentsByTweetId/{tweetId}` - Get tweet comments
- `POST /api/Comment/EditComment/{commentId}` - Update comment (owner only)
- `DELETE /api/Comment/DeleteComment/{commentId}` - Delete comment (owner only)
- `POST /api/Comment/Like/{commentId}` - Like a comment
- `POST /api/Comment/Unlike/{commentId}` - Unlike a comment

### Social Features (Auth Required)
- `POST /api/Follow/followUser/{username}` - Follow user
- `POST /api/Follow/UnFollow/{username}` - Unfollow user
- `GET /api/Follow/GetFollowers/{username}` - Get user followers
- `GET /api/Follow/GetFollowings/{username}` - Get user followings
- `GET /api/Follow/FollowCount/{username}` - Get follow statistics
- `GET /api/Follow/getMutualFollowers/{username}` - Get mutual followers

## 🔐 Authentication

The API uses JWT (JSON Web Tokens) for authentication. After successful login, include the token in the Authorization header:

```
Authorization: Bearer <your-jwt-token>
```

### Token Details
- **Expiration**: 12 months from creation
- **Claims**: 
  - `jti` (JWT ID): Unique identifier for the token
  - `nameid` (Name Identifier): User ID
  - `unique_name`: Username
- **Algorithm**: HMAC SHA256
- **Issuer**: Configurable via appsettings

### Protected Endpoints
All endpoints except `/api/Account/register` and `/api/Account/Login` require authentication.

## 🗄️ Database Schema

### Core Entities
- **AppUser** - Extended Identity user with profile information
- **Tweet** - User tweets with text content and metadata
- **Comment** - Comments on tweets
- **Follow** - User follow relationships
- **LikedTweet** - Tweet likes tracking
- **LikedComment** - Comment likes tracking

### Key Relationships
- User → Tweets (One-to-Many)
- User → Comments (One-to-Many)
- Tweet → Comments (One-to-Many)
- User → Followers/Followings (Many-to-Many through Follow)
- User → Liked Tweets/Comments (Many-to-Many)

## 📋 Response Format

All API responses follow a consistent format using the `GeneralResponse` class:

### Success Response
```json
{
  "isSuccess": true,
  "data": {
    // Response data here
  },
  "message": "Success message"
}
```

### Error Response
```json
{
  "isSuccess": false,
  "message": "Error message",
  "errors": ["List of detailed errors"]
}
```

### Validation Error Response
```json
{
  "isSuccess": false,
  "message": "Validation failed",
  "errors": {
    "fieldName": ["Field-specific error messages"]
  }
}
```

## 🚨 Error Handling

### HTTP Status Codes
- `200 OK` - Successful operation
- `400 Bad Request` - Invalid input or business logic error
- `401 Unauthorized` - Authentication required or invalid token
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

### Common Error Scenarios
- **Authentication Errors**: Invalid credentials, expired tokens
- **Authorization Errors**: Attempting to modify resources owned by others
- **Validation Errors**: Invalid input data
- **Business Logic Errors**: Attempting to follow yourself, duplicate likes
- **Resource Not Found**: Invalid tweet/comment/user IDs

## 🧪 Testing

### Using Swagger UI
1. Run the application
2. Navigate to `/swagger` endpoint
3. Use the "Authorize" button to add JWT token
4. Test endpoints directly from the interface

### API Testing Tools
- **Postman** - Comprehensive API testing
- **curl** - Command-line testing
- **Swagger UI** - Built-in testing interface

### Sample Test Flow
1. Register a new user
2. Login to get JWT token
3. Create tweets
4. Follow other users
5. Like tweets and comments
6. View timeline and social features

## 📈 Performance Considerations

- **Lazy Loading** - Enabled for navigation properties
- **Repository Pattern** - Efficient data access
- **Unit of Work** - Optimized database transactions
- **DTO Mapping** - Reduced data transfer overhead

## 🔒 Security Features

- **JWT Authentication** - Stateless, secure token-based auth
- **Password Hashing** - ASP.NET Core Identity handles secure password storage
- **Authorization Filters** - Endpoint-level security
- **Input Validation** - Model validation attributes
- **Ownership Validation** - Users can only modify their own content
- **CORS Configuration** - Cross-origin request handling
- **Secure Account Deletion** - Password verification required
