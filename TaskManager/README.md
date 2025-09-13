# Task Manager Demo Application

A comprehensive .NET Framework 4.7.2 Task Management application demonstrating best practices, patterns, and features for enterprise-level development.

## 🎯 Overview

This application showcases a full-featured task management system with the following characteristics:

- **Architecture**: Clean separation of concerns with Models, Services, and Console UI
- **Data Persistence**: XML-based file storage with async/await patterns
- **Configuration Management**: App.config for settings and connection strings
- **Error Handling**: Comprehensive exception handling and resource disposal
- **Best Practices**: Following .NET Framework 4.7.2 and C# 7.3 guidelines

## 🏗️ Architecture

```
TaskManager/
├── Models/
│   ├── Task.cs              # Main task entity with business logic
│   ├── TaskPriority.cs      # Priority enumeration
│   └── TaskStatus.cs        # Status enumeration
├── Services/
│   ├── ITaskService.cs      # Service interface
│   ├── TaskService.cs       # Main business logic service
│   └── FileStorageService.cs # XML file persistence
├── Properties/
│   └── AssemblyInfo.cs      # Assembly metadata
├── Program.cs               # Console application entry point
├── App.config               # Configuration settings
└── TaskManager.csproj       # .NET Framework project file
```

## ✨ Key Features

### Task Management
- ✅ Create, Read, Update, Delete (CRUD) operations
- ✅ Task prioritization (Low, Normal, High, Critical)
- ✅ Status tracking (Not Started, In Progress, Completed, Cancelled, On Hold)
- ✅ Due date management with overdue detection
- ✅ Time tracking (estimated vs. actual hours)
- ✅ Task assignment and tagging
- ✅ Search functionality

### Data & Persistence
- ✅ XML-based storage with proper serialization
- ✅ Async file operations with ConfigureAwait(false)
- ✅ Auto-save functionality
- ✅ Data validation and error handling

### User Interface
- ✅ Interactive console menu system
- ✅ Formatted data display with tables
- ✅ Input validation and user feedback
- ✅ Comprehensive search and filtering

### Configuration
- ✅ App.config for application settings
- ✅ Configurable limits and behaviors
- ✅ Connection string management for future database integration

## 🛠️ Technical Highlights

### .NET Framework Best Practices Demonstrated

1. **Async/Await Pattern**
   - Proper use of `ConfigureAwait(false)` to prevent deadlocks
   - Async file I/O operations
   - Task-based asynchronous programming

2. **Resource Management**
   - Proper implementation of `IDisposable` pattern
   - Using statements for automatic resource cleanup
   - Memory-efficient operations

3. **Configuration Management**
   - `ConfigurationManager.AppSettings` usage
   - Connection strings configuration
   - Environment-specific settings support

4. **String Operations**
   - Culture-aware string comparisons using `StringComparison.OrdinalIgnoreCase`
   - Invariant culture for serialization
   - Proper string formatting

5. **DateTime Handling**
   - `DateTimeOffset` for timezone-aware operations
   - Culture-invariant date parsing and formatting
   - Proper date arithmetic

6. **Exception Handling**
   - Specific exception types instead of generic `Exception`
   - Proper error logging patterns
   - Graceful degradation

7. **Type Safety**
   - Strong typing with enums
   - Generic collections usage
   - GUID-based unique identifiers

## 🚀 Sample Data

The application includes sample tasks demonstrating:
- Various priority levels and status states
- Overdue task scenarios
- Different assignees and work patterns
- Realistic time tracking examples
- Tagged categorization

## 📊 Features Showcase

### Dashboard Statistics
- Task count by status
- Completion rate calculation
- Overdue task alerts
- High-priority task indicators

### Advanced Filtering
- Filter by status, priority, assignee
- Overdue task identification
- Tag-based searching
- Full-text search across title/description

### Data Validation
- Required field validation
- Date format validation
- Numeric input validation
- Business rule enforcement (max tasks, etc.)

## 🎓 Learning Objectives

This demo application teaches:

1. **Clean Architecture Principles**
   - Separation of concerns
   - Dependency inversion
   - Service layer patterns

2. **.NET Framework Development**
   - Project file structure (non-SDK style)
   - Assembly configuration
   - Framework-specific patterns

3. **Asynchronous Programming**
   - Task-based operations
   - Avoiding deadlocks
   - Performance considerations

4. **Data Persistence**
   - File-based storage
   - XML serialization
   - Data integrity

5. **User Experience**
   - Console application design
   - Interactive menu systems
   - Data presentation

## 🔧 Build Instructions

### Prerequisites
- .NET Framework 4.7.2 Developer Pack
- Visual Studio 2017 or later
- MSBuild 15.0 or later

### Building the Application
```powershell
# Using MSBuild (recommended for .NET Framework)
msbuild /t:rebuild

# Alternative using dotnet CLI (if targeting pack is installed)
dotnet build
```

### Running the Application
```powershell
# From bin/Debug or bin/Release directory
./TaskManager.exe
```

## 📝 Configuration Options

Edit `App.config` to customize:

```xml
<appSettings>
  <add key="TaskStorageFile" value="tasks.xml" />
  <add key="MaxTasksPerUser" value="100" />
  <add key="AutoSaveEnabled" value="true" />
</appSettings>
```

## 🧪 Testing Scenarios

The application supports testing of:
- CRUD operations
- Concurrent access patterns
- Large dataset handling
- Error recovery scenarios
- Performance under load

## 🔮 Extension Possibilities

This foundation supports adding:
- Database persistence (SQL Server, SQLite)
- REST API endpoints
- Web UI interface
- Authentication/authorization
- Real-time notifications
- Export/import functionality
- Advanced reporting

## 📚 Code Quality Features

- Comprehensive XML documentation
- Consistent naming conventions
- SOLID principles implementation
- Error handling best practices
- Performance optimizations
- Memory management patterns

---

*This demo application serves as a comprehensive example of professional .NET Framework development, showcasing enterprise-ready patterns and practices.*
