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
- [Contributing](#contributing)
- [License](#license)

## ✨ Features

### Core Functionality
- **User Authentication & Authorization** - JWT-based secure authentication
- **Profile Management** - Complete user profile CRUD operations
- **Tweet Operations** - Create, read, update, and delete tweets
- **Social Features** - Follow/unfollow users, view followers and followings
- **Comment System** - Comment on tweets with full CRUD operations
- **Social Interactions** - Like tweets and comments
- **Timeline** - View tweets from followed users

### Advanced Features
- **Mutual Followers** - Discover mutual connections between users
- **User Search** - Search and view other user profiles
- **Follow Statistics** - Track follower and following counts
- **Account Management** - Secure account deletion with password verification

## 🛠️ Technologies Used

### Backend Framework
- **ASP.NET Core 9.0** - , cross-platform web framework
- **Entity Framework Core** - Object-relational mapping (ORM)
- **ASP.NET Core Identity** - Authentication and authorization system

### Database
- **SQL Server** - Primary database management system
- **Entity Framework Core with Lazy Loading** - Database access layer

### Authentication & Security
- **JWT (JSON Web Tokens)** - Stateless authentication
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT authentication middleware
- **ASP.NET Core Identity** - User management and role-based security

### Development Tools
- **AutoMapper** - Object-to-object mapping
- **Swagger/OpenAPI** - API documentation and testing
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

### Project Structure
```
TwitterClone_API/
├── Controllers/          # API endpoints
├── DataAccess/          # Data access layer
│   └── Repo/           # Repository implementations
├── Models/             # Data models and DTOs
│   ├── AppModels/      # Entity models
│   ├── DTOs/           # Data transfer objects
│   ├── Mapping/        # AutoMapper profiles
│   └── Response/       # API response models
└── Program.cs          # Application configuration
```

## 🚀 Installation

### Prerequisites
- .NET 9.0 SDK or later
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
       "SecretKey": "your-secret-key-here",
       "Issuer": "your-issuer-here"
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

### Authentication
- `POST /api/Account/register` - Register new user
- `POST /api/Account/Login` - User login

### Profile Management
- `GET /api/Profile/MyProfile` - Get current user profile
- `GET /api/Profile/Search/Profile/{username}` - Search user profile
- `POST /api/Profile/EditMyProfile` - Update profile
- `DELETE /api/Profile/DeleteMyProfile` - Delete account

### Tweet Operations
- `POST /api/Tweet/AddTweet` - Create new tweet
- `GET /api/Tweet/GetAllTweets` - Get timeline tweets
- `GET /api/Tweet/GetTweet{tweetId}` - Get specific tweet
- `POST /api/Tweet/EditTweet/{tweetId}` - Update tweet
- `DELETE /api/Tweet/DeleteTweet/{tweetId}` - Delete tweet

### Comment System
- `POST /api/Comment/AddComment/{tweetId}` - Add comment to tweet
- `GET /api/Comment/GetCommentsByTweetId/{tweetId}` - Get tweet comments
- `GET /api/Comment/MyReplies` - Get user's comments
- `POST /api/Comment/EditComment/{commentId}` - Update comment
- `DELETE /api/Comment/DeleteComment/{commentId}` - Delete comment

### Social Features
- `POST /api/Follow/Follow/{username}` - Follow user
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
- **Expiration**: 12 months
- **Claims**: User ID, Username, and JTI (JWT ID)
- **Algorithm**: HMAC SHA256

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
- **CORS Configuration** - Cross-origin request handling


## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
---

**Built with ❤️ using ASP.NET Core**
