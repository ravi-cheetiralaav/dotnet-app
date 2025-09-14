# 🧪 **Unit Test Generation Exercise with NUnit**

## 📋 **Exercise Overview**

This exercise demonstrates how to generate comprehensive unit tests for a .NET 8 Task Management application using NUnit framework. You'll learn to create tests that cover model validation, service operations, and integration scenarios.

---

## 🎯 **Learning Objectives**

By the end of this exercise, you will be able to:

1. ✅ Set up NUnit testing framework in a .NET 8 project
2. ✅ Write unit tests for model classes with proper validation
3. ✅ Create service layer tests with mocking and dependency injection
4. ✅ Implement integration tests for complex workflows
5. ✅ Handle async operations in unit tests
6. ✅ Test error scenarios and edge cases
7. ✅ Use proper test organization and naming conventions

---

## 🏗️ **Project Structure**

The TaskManager application consists of:

```
TaskManagerModern/
├── Program.cs                  # Main console application
├── TaskManagerModern.csproj    # Project file with NUnit packages
├── Models/
│   └── TaskItem.cs            # Task entity with business logic
├── Services/
│   ├── ITaskService.cs        # Service interface
│   └── TaskService.cs         # Service implementation
└── ProgramTests.cs           # Comprehensive test suite
```

---

## 🚀 **Step-by-Step Implementation**

### **Step 1: Project Setup**

#### **Initial Prompt:**
```
"Generate unit test cases using NUnit?"
```

#### **Prerequisites Setup:**
1. Ensure you have a .NET 8 console application
2. Add the required NUnit packages to your project:

```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
<PackageReference Include="NUnit" Version="4.4.0" />
<PackageReference Include="NUnit3TestAdapter" Version="5.1.0" />
```

#### **Commands Executed:**
```powershell
# Add NUnit packages
dotnet add package NUnit
dotnet add package NUnit3TestAdapter  
dotnet add package Microsoft.NET.Test.Sdk

# Run tests
dotnet test
dotnet test --logger "console;verbosity=normal"
```

---

### **Step 2: Model Testing (TaskItem Tests)**

#### **Test Categories Implemented:**

**🔸 Basic Property Validation**
```csharp
[Test]
public void TaskItem_DefaultValues_ShouldBeSetCorrectly()
{
    // Act
    var task = new TaskItem();

    // Assert
    Assert.That(task.Id, Is.Not.EqualTo(Guid.Empty));
    Assert.That(task.Title, Is.EqualTo(string.Empty));
    Assert.That(task.Priority, Is.EqualTo(TaskPriority.Normal));
    Assert.That(task.Status, Is.EqualTo(TaskStatus.NotStarted));
}
```

**🔸 Business Logic Testing**
```csharp
[Test]
public void IsOverdue_WhenPastDueAndNotCompleted_ShouldReturnTrue()
{
    // Arrange
    var task = new TaskItem
    {
        DueDate = DateTimeOffset.Now.AddDays(-1),
        Status = TaskStatus.InProgress
    };

    // Act & Assert
    Assert.That(task.IsOverdue(), Is.True);
}
```

**🔸 Edge Case Testing**
```csharp
[Test]
public void IsOverdue_WhenTaskCompleted_ShouldReturnFalse()
{
    // Arrange
    var task = new TaskItem
    {
        DueDate = DateTimeOffset.Now.AddDays(-1),
        Status = TaskStatus.Completed
    };

    // Act & Assert
    Assert.That(task.IsOverdue(), Is.False);
}
```

---

### **Step 3: Service Layer Testing (TaskService Tests)**

#### **🔸 CRUD Operations Testing**

**Create Operation:**
```csharp
[Test]
public async Task CreateTaskAsync_WithValidTask_ShouldReturnTrue()
{
    // Arrange
    var task = new TaskItem
    {
        Title = "Test Task",
        Description = "Test Description"
    };

    // Act
    var result = await _taskService.CreateTaskAsync(task);

    // Assert
    Assert.That(result, Is.True);
    
    var allTasks = await _taskService.GetAllTasksAsync();
    Assert.That(allTasks.Count(), Is.EqualTo(1));
    Assert.That(allTasks.First().Title, Is.EqualTo("Test Task"));
}
```

**🔸 Error Handling Testing**
```csharp
[Test]
public void CreateTaskAsync_WithNullTask_ShouldThrowArgumentNullException()
{
    // Act & Assert
    Assert.ThrowsAsync<ArgumentNullException>(() => _taskService.CreateTaskAsync(null!));
}

[Test]
public void CreateTaskAsync_WithEmptyTitle_ShouldThrowArgumentException()
{
    // Arrange
    var task = new TaskItem { Title = "" };

    // Act & Assert
    var ex = Assert.ThrowsAsync<ArgumentException>(() => _taskService.CreateTaskAsync(task));
    Assert.That(ex?.ParamName, Is.EqualTo("task"));
}
```

**🔸 Filtering and Query Testing**
```csharp
[Test]
public async Task GetTasksByStatusAsync_ShouldReturnCorrectTasks()
{
    // Arrange
    await _taskService.CreateTaskAsync(new TaskItem { Title = "Task 1", Status = TaskStatus.InProgress });
    await _taskService.CreateTaskAsync(new TaskItem { Title = "Task 2", Status = TaskStatus.Completed });
    await _taskService.CreateTaskAsync(new TaskItem { Title = "Task 3", Status = TaskStatus.InProgress });

    // Act
    var inProgressTasks = await _taskService.GetTasksByStatusAsync(TaskStatus.InProgress);
    var completedTasks = await _taskService.GetTasksByStatusAsync(TaskStatus.Completed);

    // Assert
    Assert.That(inProgressTasks.Count(), Is.EqualTo(2));
    Assert.That(completedTasks.Count(), Is.EqualTo(1));
    Assert.That(inProgressTasks.All(t => t.Status == TaskStatus.InProgress), Is.True);
}
```

---

### **Step 4: Integration Testing**

#### **🔸 End-to-End Workflow Testing**
```csharp
[Test]
public async Task TaskWorkflow_CreateUpdateDelete_ShouldWorkCorrectly()
{
    // Arrange & Act - Create
    var newTask = new TaskItem
    {
        Title = "Workflow Test Task",
        Description = "Testing the complete workflow",
        Priority = TaskPriority.High,
        AssignedTo = "Test User"
    };
    
    var createResult = await _taskService.CreateTaskAsync(newTask);
    Assert.That(createResult, Is.True);

    // Act & Assert - Update
    var createdTask = await _taskService.GetTaskByIdAsync(newTask.Id);
    createdTask!.Status = TaskStatus.InProgress;
    
    var updateResult = await _taskService.UpdateTaskAsync(createdTask);
    Assert.That(updateResult, Is.True);

    // Act & Assert - Delete
    var deleteResult = await _taskService.DeleteTaskAsync(newTask.Id);
    Assert.That(deleteResult, Is.True);

    var deletedTask = await _taskService.GetTaskByIdAsync(newTask.Id);
    Assert.That(deletedTask, Is.Null);
}
```

#### **🔸 Data Persistence Testing**
```csharp
[Test]
public async Task SaveAndLoadAsync_ShouldPersistTasks()
{
    // Arrange
    var task1 = new TaskItem { Title = "Persistent Task 1" };
    var task2 = new TaskItem { Title = "Persistent Task 2" };
    
    await _taskService.CreateTaskAsync(task1);
    await _taskService.CreateTaskAsync(task2);

    // Act - Save and create new service instance
    await _taskService.SaveAsync();
    _taskService.Dispose();

    var newService = new TaskService();
    var loadResult = await newService.LoadAsync();
    var loadedTasks = await newService.GetAllTasksAsync();

    // Assert
    Assert.That(loadResult, Is.True);
    Assert.That(loadedTasks.Count(), Is.EqualTo(2));
    Assert.That(loadedTasks.Any(t => t.Title == "Persistent Task 1"), Is.True);
    Assert.That(loadedTasks.Any(t => t.Title == "Persistent Task 2"), Is.True);

    // Cleanup
    newService.Dispose();
}
```

---

## 🧪 **Test Organization Best Practices**

### **1. Test Class Structure**
```csharp
[TestFixture]
public class TaskServiceTests
{
    private TaskService _taskService = null!;
    private string _testFilePath = null!;

    [SetUp]
    public void Setup()
    {
        _testFilePath = "test_tasks.json";
        _taskService = new TaskService();
        
        // Clean up any existing test files
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [TearDown]
    public void TearDown()
    {
        _taskService?.Dispose();
        
        // Clean up test files
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
}
```

### **2. Test Naming Conventions**
```
MethodName_StateUnderTest_ExpectedBehavior

Examples:
- CreateTaskAsync_WithValidTask_ShouldReturnTrue
- IsOverdue_WhenPastDueAndNotCompleted_ShouldReturnTrue
- GetTasksByStatus_WithInProgressFilter_ShouldReturnCorrectTasks
```

### **3. AAA Pattern (Arrange, Act, Assert)**
```csharp
[Test]
public async Task ExampleTest()
{
    // Arrange - Set up test data and conditions
    var task = new TaskItem { Title = "Test Task" };

    // Act - Execute the method being tested
    var result = await _taskService.CreateTaskAsync(task);

    // Assert - Verify the expected outcome
    Assert.That(result, Is.True);
}
```

---

## 📊 **Test Results and Coverage**

### **Final Test Execution Results:**
```
Test Run Successful.
Total tests: 33
     Passed: 33 ✅
     Failed: 0 ❌
 Total time: ~3.08 seconds
```

### **Test Coverage Breakdown:**

| **Test Category** | **Test Count** | **Coverage Areas** |
|------------------|----------------|-------------------|
| **TaskItem Model** | 9 tests | Property validation, business logic, edge cases |
| **TaskService CRUD** | 14 tests | Create, Read, Update, Delete, error handling |
| **Integration** | 10 tests | Workflows, persistence, cross-functional features |

### **Key Features Tested:**
- ✅ **Input Validation**: Null checks, empty strings, required fields
- ✅ **Business Logic**: Overdue calculations, status transitions
- ✅ **Error Handling**: Exception types and messages
- ✅ **Data Persistence**: Save/load operations with JSON files  
- ✅ **Async Operations**: Proper async/await patterns
- ✅ **Edge Cases**: Boundary conditions and special scenarios
- ✅ **Integration Flows**: End-to-end task lifecycle management

---

## 🎓 **Key Learning Points**

### **1. NUnit Framework Features Used:**
- `[TestFixture]` - Class-level test organization
- `[Test]` - Method-level test identification
- `[SetUp]`/`[TearDown]` - Test lifecycle management
- `Assert.That()` - Fluent assertion syntax
- `Assert.ThrowsAsync<T>()` - Exception testing
- `Is.EqualTo()`, `Is.GreaterThan()` - Constraint-based assertions

### **2. Testing Patterns Implemented:**
- **Repository Pattern Testing** - Service layer abstraction
- **Dependency Injection** - Interface-based testing
- **Test Data Builders** - Helper methods for test data creation
- **Test Isolation** - Each test runs independently
- **Resource Cleanup** - Proper disposal and file cleanup

### **3. Common Challenges Addressed:**
- **Nullable Reference Types** - Proper null handling in .NET 8
- **Async Testing** - Testing asynchronous operations correctly  
- **File System Testing** - Creating and cleaning up test files
- **Test Data Management** - Sample data creation and isolation
- **Exception Testing** - Verifying proper error handling

---

## 🏃‍♂️ **Exercise Execution Steps**

### **To Complete This Exercise:**

1. **Clone the Repository:**
   ```bash
   git clone [repository-url]
   cd dotnet-app/TaskManagerModern
   ```

2. **Install Dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the Application:**
   ```bash
   dotnet run
   ```

4. **Execute Tests:**
   ```bash
   dotnet test
   dotnet test --logger "console;verbosity=normal"
   ```

5. **Analyze Results:**
   - Review test coverage
   - Identify any failing tests
   - Understand test patterns used

### **Extended Exercises:**

1. **Add More Test Cases:**
   - Test the overdue task notifications
   - Add performance tests for large datasets
   - Test concurrent access scenarios

2. **Implement Mock Testing:**
   - Use Moq framework for service dependencies
   - Create integration tests with test databases
   - Mock file system operations

3. **Add Code Coverage:**
   ```bash
   dotnet test --collect:"XPlat Code Coverage"
   ```

---

## 🔧 **Troubleshooting Common Issues**

### **Issue 1: Tests Not Discovered**
**Solution:** Ensure NUnit packages are properly referenced and `[TestFixture]` attributes are applied.

### **Issue 2: File System Conflicts**
**Solution:** Use unique test file names and proper cleanup in `[TearDown]` methods.

### **Issue 3: Async Test Failures**
**Solution:** Always use `async Task` return types and `await` all async operations.

### **Issue 4: Nullable Reference Warnings**
**Solution:** Use null-forgiving operator (`!`) or proper null checks in test code.

---

## 📚 **Additional Resources**

- **NUnit Documentation:** https://nunit.org/
- **.NET Testing Best Practices:** https://docs.microsoft.com/en-us/dotnet/core/testing/
- **Unit Testing Patterns:** https://martinfowler.com/articles/practical-test-pyramid.html

---

## 📝 **Exercise Summary**

This exercise demonstrates a complete unit testing implementation for a real-world .NET application, covering:

- **33 comprehensive test cases** with 100% pass rate
- **Model, Service, and Integration testing** patterns  
- **Async operation testing** with proper error handling
- **Test organization and best practices**
- **Real-world scenarios** including file persistence and business logic validation

The exercise provides a solid foundation for implementing unit testing in .NET applications using NUnit framework.

---

*Exercise completed successfully with all 33 tests passing! 🎉*