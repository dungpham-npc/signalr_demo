# SignalR Demo - FU News Management System

A real-time news management system built with ASP.NET Core Razor Pages and SignalR, demonstrating live updates when news articles are created, updated, or deleted.

## 🚀 Features

- **Real-time Updates**: Live notifications when news articles are modified using SignalR
- **News Management**: Full CRUD operations for news articles
- **Category Management**: Organize news by categories
- **Tag System**: Tag articles for better organization
- **User Management**: Role-based access control (Admin/Staff)
- **Responsive Design**: Works on desktop and mobile devices

## 🏗️ Project Structure

```
├── PhamAnhDungRazorPages/     # Main web application (ASP.NET Core Razor Pages)
├── BusinessLayer/             # Business logic layer
├── DAL/                      # Data Access Layer (Entity Framework Core)
└── PhamAnhDung_NET1707_A02.sln # Solution file
```

## 📋 Prerequisites

Before running this project, make sure you have the following installed:

- **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** (LocalDB, Express, or Full version)
  - SQL Server Express: [Download here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
  - Or use SQL Server LocalDB which comes with Visual Studio

## 🚀 Quick Start

For those who want to get up and running immediately:

```bash
# 1. Clone the repository
git clone https://github.com/dungpham-npc/signalr_demo.git
cd signalr_demo

# 2. Restore dependencies
dotnet restore

# 3. Build the solution
dotnet build

# 4. Run the application
cd PhamAnhDungRazorPages
dotnet run

# 5. Open browser to https://localhost:7055
# 6. Login with: admin@FUNewsManagementSystem.org / @@abc123@@
```

**Note**: This assumes you have SQL Server LocalDB installed (comes with Visual Studio). If not, follow the detailed setup instructions below.

## 🛠️ Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/dungpham-npc/signalr_demo.git
cd signalr_demo
```

### 2. Database Setup

This application uses Entity Framework Core with SQL Server. The database schema will be created automatically on first run.

#### Option A: Using SQL Server LocalDB (Recommended for development)

1. Install SQL Server LocalDB (comes with Visual Studio or can be downloaded separately)
2. Update the connection string in `PhamAnhDungRazorPages/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnectionStringDB": "Server=(localdb)\\mssqllocaldb;Database=funewsmanagement;Trusted_Connection=true;TrustServerCertificate=True"
  }
}
```

#### Option B: Using SQL Server Express/Full

1. Ensure SQL Server is running
2. Create a database named `funewsmanagement` or let the application create it automatically
3. Update the connection string in `PhamAnhDungRazorPages/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnectionStringDB": "Server=localhost;Database=funewsmanagement;Trusted_Connection=true;TrustServerCertificate=True"
  }
}
```

#### Option C: Using SQL Server with Authentication

If you're using SQL Server with username/password authentication (default configuration):

```json
{
  "ConnectionStrings": {
    "DefaultConnectionStringDB": "server=(local);database=funewsmanagement;uid=sa;pwd=123456;TrustServerCertificate=True"
  }
}
```

**Note**: Make sure to update the password to match your SQL Server SA account password.

#### Database Schema Creation

The application will automatically create the database schema when you first run it. The database includes:
- `Category` - News categories
- `NewsArticle` - News articles with content
- `SystemAccount` - User accounts
- `Tag` - Tags for articles
- Junction tables for many-to-many relationships

### 3. Configure Application Settings

Update the connection string in `PhamAnhDungRazorPages/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnectionStringDB": "Server=(localdb)\\mssqllocaldb;Database=funewsmanagement;Trusted_Connection=true;TrustServerCertificate=True"
  }
}
```

**Note**: The default configuration assumes:
- Server: `(local)` 
- Database: `funewsmanagement`
- Username: `sa`
- Password: `123456`

### 4. Restore Dependencies

```bash
dotnet restore
```

### 5. Build the Solution

```bash
dotnet build
```

## 🚀 Running the Application

### Development Mode

```bash
cd PhamAnhDungRazorPages
dotnet run
```

The application will start and be available at:
- HTTPS: `https://localhost:7055`
- HTTP: `http://localhost:5074`

### Production Mode

```bash
cd PhamAnhDungRazorPages
dotnet run --configuration Release
```

## 🔐 Default Login Credentials

The application comes with a default admin account:

- **Email**: `admin@FUNewsManagementSystem.org`
- **Password**: `@@abc123@@`

## 📊 Sample Data

When you first run the application, the database will be empty except for the admin account. To fully test the features:

1. **Create Categories**: Add some news categories (e.g., Technology, Sports, Politics)
2. **Create Tags**: Add some tags for organizing articles (e.g., breaking-news, featured, local)
3. **Create Articles**: Add news articles and assign them to categories and tags
4. **Test Real-time Updates**: Open multiple browser tabs to see SignalR in action

## 📖 How to Use

### 1. Access the Application
Open your browser and navigate to `https://localhost:7055`

### 2. Login
Use the default admin credentials to log in.

### 3. Manage News Articles
- **Create**: Click "Add New Article" to create news articles
- **Edit**: Click on any article to edit it
- **Delete**: Use the delete button to remove articles
- **Real-time Updates**: Open multiple browser tabs to see live updates

### 4. Manage Categories and Tags
- Navigate to the Categories section to manage article categories
- Add tags to articles for better organization

### 5. User Management
- Admin users can create and manage other user accounts
- Different user roles have different permissions

## 🔧 Development

### Database Migrations

If you make changes to the data models, create and apply migrations:

```bash
cd DAL
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

### Adding New Features

1. **Models**: Add new entities in the `DAL/Models` folder
2. **Services**: Implement business logic in the `BusinessLayer/Services` folder
3. **Pages**: Create new Razor Pages in the `PhamAnhDungRazorPages/Pages` folder
4. **SignalR**: Extend the `NewsHub` for real-time features

## 🧪 Testing the SignalR Functionality

1. Open two browser windows/tabs with the application
2. Log in to both
3. In one window, create or edit a news article
4. Watch the other window automatically update with the changes
5. Check the browser console for SignalR connection messages

## 🐛 Troubleshooting

### Database Connection Issues
- Ensure SQL Server is running
- Verify the connection string in `appsettings.json`
- Check if the database `funewsmanagement` exists

### Port Already in Use
```bash
# Kill the process using the port
netstat -ano | findstr :7055
taskkill /PID <PID> /F

# Or change the port in launchSettings.json
```

### Build Errors
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

## 🌐 Browser Compatibility

This application works best with modern browsers that support:
- WebSockets (for SignalR)
- ES6+ JavaScript features
- Modern CSS features

Recommended browsers:
- Chrome 80+
- Firefox 75+
- Safari 13+
- Edge 80+

## 📚 Technologies Used

- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core 9.0** - ORM
- **SignalR** - Real-time communication
- **SQL Server** - Database
- **Bootstrap** - CSS framework
- **jQuery** - JavaScript library

## 📝 License

This project is for educational purposes and demonstration of SignalR functionality.

## 🤝 Contributing

This is a demo project, but feel free to:
1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## 📞 Support

If you encounter any issues:
1. Check the troubleshooting section above
2. Review the console logs in your browser
3. Check the application logs in the console where you're running the app