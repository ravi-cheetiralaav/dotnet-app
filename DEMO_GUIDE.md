# Task Manager Demo Applications

I've created two comprehensive .NET demo applications for you:

## 1. TaskManager (.NET Framework 4.7.2)
**Location**: `TaskManager/`

This is a full-featured **enterprise-grade .NET Framework application** that demonstrates:

### 🏗️ Architecture & Design
- **Clean Architecture** with Models, Services, and UI separation
- **Service Pattern** with interfaces and implementations
- **Repository Pattern** for data access
- **Dependency management** following SOLID principles

### 🔧 .NET Framework Best Practices
- **Async/Await patterns** with `ConfigureAwait(false)`
- **IDisposable implementation** for proper resource management
- **Configuration management** via App.config
- **XML-based persistence** with proper serialization
- **Exception handling** with specific exception types
- **Culture-aware operations** for dates and strings

### ✨ Business Features
- Complete CRUD operations for tasks
- Task prioritization (Low, Normal, High, Critical)
- Status tracking (Not Started, In Progress, Completed, Cancelled, On Hold)
- Due date management with overdue detection
- Time tracking (estimated vs actual hours)
- Task assignment and tagging system
- Advanced filtering and search capabilities
- Statistical reporting and analytics

### 📊 User Interface
- Interactive console menu system
- Formatted table displays
- Input validation and user feedback
- Real-time task statistics

**Note**: Requires .NET Framework 4.7.2 Developer Pack to build. The code follows C# 7.3 syntax limitations.

---

## 2. TaskManagerModern (.NET 8)
**Location**: `TaskManagerModern/TaskManagerModern/`

This is a **modern .NET 8 version** that you can run immediately:

### 🚀 Modern Features
- **Latest C# language features** (file-scoped namespaces, records, pattern matching)
- **JSON-based persistence** with System.Text.Json
- **Simplified architecture** with modern patterns
- **Nullable reference types** for better null safety
- **Collection expressions** and modern syntax

### 🏃‍♂️ Ready to Run
```powershell
cd "TaskManagerModern\TaskManagerModern"
dotnet run
```

---

## 🎯 Demo Highlights

Both applications showcase:

### Technical Excellence
- **Comprehensive error handling** with try-catch patterns
- **Resource management** with using statements and IDisposable
- **Asynchronous programming** with proper async/await usage
- **Data validation** and business rule enforcement
- **Clean code principles** with meaningful names and documentation

### Real-World Scenarios
- **Task lifecycle management** from creation to completion
- **Priority-based workflow** handling
- **Deadline tracking** with overdue alerts
- **Team collaboration** features with assignee management
- **Project categorization** with tags and descriptions

### Learning Objectives
1. **Enterprise Architecture Patterns**
2. **Async Programming Best Practices**
3. **Data Persistence Strategies**
4. **User Interface Design Principles**
5. **Configuration Management**
6. **Testing and Validation Approaches**

---

## 🔥 Key Differentiators from "Hello World"

This isn't just a simple console output - it's a **production-ready application foundation** that demonstrates:

- ✅ **Complex business logic** with multiple interacting components
- ✅ **Data persistence** with file-based storage
- ✅ **User interaction** with menu systems and input validation
- ✅ **Error handling** and recovery scenarios
- ✅ **Performance considerations** with async operations
- ✅ **Scalability patterns** ready for database integration
- ✅ **Professional code organization** with proper separation of concerns
- ✅ **Documentation** and maintainability features

---

## 🚀 Quick Start

### Run the Modern Version (Recommended)
```powershell
cd "c:\Users\ravi.cheetirala\OneDrive - Avanade\Work\GHCP ACM\Workshop\Foundations\dotnet-app\TaskManagerModern\TaskManagerModern"
dotnet run
```

### Explore the Framework Version
```powershell
cd "c:\Users\ravi.cheetirala\OneDrive - Avanade\Work\GHCP ACM\Workshop\Foundations\dotnet-app\TaskManager"
# Install .NET Framework 4.7.2 Developer Pack first
msbuild /t:rebuild
```

Both applications include sample data and interactive tutorials to demonstrate all features!

---

**Perfect for demos, learning, and as a foundation for real applications!** 🎉