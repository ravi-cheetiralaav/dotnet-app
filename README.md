# Task Manager Demo Applications

This workspace contains two comprehensive .NET task management applications showcasing different frameworks and modern development practices.

## 🏗️ Project Structure

```
dotnet-app/
├── TaskManager/                    # .NET Framework 4.7.2 Version
│   ├── Models/                     # Domain models and enums
│   ├── Services/                   # Business logic and data access
│   ├── Properties/                 # Assembly information
│   ├── App.config                  # Configuration settings
│   ├── TaskManager.csproj          # .NET Framework project file
│   ├── Program.cs                  # Main application entry point
│   └── README.md                   # Detailed project documentation
├── TaskManagerModern/              # .NET 8 Version (Recommended)
│   ├── Models/                     # Domain models with modern C# features
│   ├── Services/                   # Business services with async patterns
│   ├── TaskManagerModern.csproj    # Modern SDK-style project file
│   └── Program.cs                  # Main application with latest C# syntax
├── .github/
│   └── instructions/               # Development guidelines
├── .gitignore                      # Git ignore rules
├── dotnet-app.sln                  # Visual Studio solution file
└── DEMO_GUIDE.md                   # Comprehensive demo documentation
```

## 🚀 Quick Start

### Run the Modern Version (Recommended)
```bash
cd TaskManagerModern
dotnet run
```

### Build the Framework Version
```bash
cd TaskManager
# Requires .NET Framework 4.7.2 Developer Pack
msbuild /t:rebuild
```

## 📖 Features

Both applications include:
- ✅ Complete CRUD operations
- ✅ Task prioritization and status tracking
- ✅ Due date management with overdue detection
- ✅ Time tracking (estimated vs actual hours)
- ✅ Assignment and tagging system
- ✅ Advanced filtering and search
- ✅ Statistical reporting
- ✅ Interactive console UI
- ✅ Persistent data storage

## 🎯 Learning Objectives

- Enterprise-grade application architecture
- Async/await programming patterns
- Data persistence strategies
- Configuration management
- Error handling and validation
- Clean code principles
- Modern C# language features

## 📚 Documentation

- See `DEMO_GUIDE.md` for comprehensive feature walkthrough
- See `TaskManager/README.md` for detailed .NET Framework documentation
- See `.github/instructions/` for development guidelines

---

Perfect for demos, learning, and as a foundation for production applications! 🎉