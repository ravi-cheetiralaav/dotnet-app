# 📋 **NUnit Testing Exercise Worksheet**

## 🎯 **Objective**
Generate comprehensive unit tests for a .NET 8 Task Management application using NUnit framework.

---

## 📝 **Exercise Instructions**

### **Part 1: Setup (5 minutes)**
1. **Add NUnit packages to your project:**
   ```bash
   dotnet add package NUnit
   dotnet add package NUnit3TestAdapter
   dotnet add package Microsoft.NET.Test.Sdk
   ```

2. **Verify your project file includes:**
   ```xml
   <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
   <PackageReference Include="NUnit" Version="4.4.0" />
   <PackageReference Include="NUnit3TestAdapter" Version="5.1.0" />
   ```

### **Part 2: Model Testing (15 minutes)**
Create tests for the `TaskItem` model class:

**✅ Task 1:** Test default values
```csharp
[Test]
public void TaskItem_DefaultValues_ShouldBeSetCorrectly()
{
    // Your code here
}
```

**✅ Task 2:** Test overdue logic
```csharp
[Test] 
public void IsOverdue_WhenPastDueAndNotCompleted_ShouldReturnTrue()
{
    // Your code here
}
```

**✅ Task 3:** Test edge cases
```csharp
[Test]
public void IsOverdue_WhenTaskCompleted_ShouldReturnFalse()
{
    // Your code here  
}
```

### **Part 3: Service Testing (20 minutes)**
Create tests for the `TaskService` class:

**✅ Task 4:** Test CRUD operations
```csharp
[Test]
public async Task CreateTaskAsync_WithValidTask_ShouldReturnTrue()
{
    // Your code here
}
```

**✅ Task 5:** Test error handling
```csharp
[Test]
public void CreateTaskAsync_WithNullTask_ShouldThrowArgumentNullException()
{
    // Your code here
}
```

**✅ Task 6:** Test filtering
```csharp
[Test]
public async Task GetTasksByStatusAsync_ShouldReturnCorrectTasks()
{
    // Your code here
}
```

### **Part 4: Integration Testing (15 minutes)**
Create end-to-end tests:

**✅ Task 7:** Test complete workflow
```csharp
[Test]
public async Task TaskWorkflow_CreateUpdateDelete_ShouldWorkCorrectly()
{
    // Your code here
}
```

**✅ Task 8:** Test data persistence
```csharp
[Test]
public async Task SaveAndLoadAsync_ShouldPersistTasks()
{
    // Your code here
}
```

---

## 🧪 **Test Structure Template**

```csharp
using NUnit.Framework;
using TaskManagerModern.Models;
using TaskManagerModern.Services;

namespace TaskManagerModern.Tests
{
    [TestFixture]
    public class YourTestClass
    {
        private TaskService _taskService = null!;

        [SetUp]
        public void Setup()
        {
            _taskService = new TaskService();
            // Add cleanup logic
        }

        [TearDown]  
        public void TearDown()
        {
            _taskService?.Dispose();
            // Add cleanup logic
        }

        [Test]
        public async Task YourTestMethod()
        {
            // Arrange
            
            // Act
            
            // Assert
        }
    }
}
```

---

## 📊 **Success Criteria**

**🎯 Target:** Pass all tests with this command:
```bash
dotnet test
```

**Expected Results:**
- ✅ **Total Tests:** 33
- ✅ **Passed:** 33
- ✅ **Failed:** 0
- ✅ **Duration:** < 5 seconds

---

## 💡 **Key Testing Patterns**

### **1. AAA Pattern**
```csharp
// Arrange - Setup test data
var task = new TaskItem { Title = "Test" };

// Act - Execute the method
var result = await service.CreateAsync(task);

// Assert - Verify outcome  
Assert.That(result, Is.True);
```

### **2. Exception Testing**
```csharp
Assert.ThrowsAsync<ArgumentNullException>(() => 
    service.CreateAsync(null!));
```

### **3. Async Testing**
```csharp
[Test]
public async Task AsyncMethod_ShouldWork()
{
    var result = await service.GetAsync();
    Assert.That(result, Is.Not.Null);
}
```

---

## 🏆 **Bonus Challenges**

1. **Add performance tests** for large datasets
2. **Test concurrent operations** with multiple tasks
3. **Add code coverage** measurement
4. **Create parameterized tests** for different scenarios
5. **Implement test data builders** for complex objects

---

## ✅ **Completion Checklist**

- [ ] NUnit packages installed
- [ ] Test project compiles without errors  
- [ ] All 33 tests pass
- [ ] Tests cover models, services, and integration
- [ ] Proper test organization with SetUp/TearDown
- [ ] Error handling and edge cases tested
- [ ] Async operations properly tested

---

## 🆘 **Need Help?**

**Common Issues:**
- **Tests not discovered?** Check `[TestFixture]` and `[Test]` attributes
- **Null reference warnings?** Use `= null!` for test fields
- **File conflicts?** Ensure proper cleanup in `[TearDown]`
- **Async failures?** Always `await` async operations

**Quick Commands:**
```bash
# Run tests with detailed output
dotnet test --logger "console;verbosity=normal"

# Run specific test class
dotnet test --filter "TaskItemTests"

# Run with code coverage  
dotnet test --collect:"XPlat Code Coverage"
```

---

**Time Allocation:** 60 minutes total
- Setup: 5 min
- Model Tests: 15 min  
- Service Tests: 20 min
- Integration Tests: 15 min
- Review & Debug: 5 min

**Good luck! 🚀**