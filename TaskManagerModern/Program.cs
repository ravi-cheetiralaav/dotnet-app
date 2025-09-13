using TaskManagerModern.Models;
using TaskManagerModern.Services;
using TaskStatus = TaskManagerModern.Models.TaskStatus;

namespace TaskManagerModern;

class Program
{
    private static ITaskService _taskService = null!;

    static async Task Main(string[] args)
    {
        Console.WriteLine("=================================");
        Console.WriteLine("   Task Manager Modern (.NET 8)");
        Console.WriteLine("=================================");
        Console.WriteLine();

        // Initialize the task service
        using (_taskService = new TaskService())
        {
            // Load existing tasks
            Console.WriteLine("Loading existing tasks...");
            var loadResult = await _taskService.LoadAsync();
            
            if (loadResult)
            {
                var allTasks = await _taskService.GetAllTasksAsync();
                Console.WriteLine($"Loaded {allTasks.Count()} existing tasks.");
            }
            else
            {
                Console.WriteLine("No existing tasks found. Starting fresh.");
            }

            Console.WriteLine();

            // Create some sample data if no tasks exist
            await CreateSampleDataIfNeeded();

            // Main application loop
            var running = true;
            while (running)
            {
                try
                {
                    DisplayMenu();
                    var choice = Console.ReadLine();
                    running = await ProcessMenuChoice(choice);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("\nSaving tasks before exit...");
            await _taskService.SaveAsync();
            Console.WriteLine("Thank you for using Task Manager Modern!");
        }
    }

    private static void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Task Manager Modern ===");
        Console.WriteLine("1. View All Tasks");
        Console.WriteLine("2. Create New Task");
        Console.WriteLine("3. View Tasks by Status");
        Console.WriteLine("4. View Overdue Tasks");
        Console.WriteLine("5. View Task Statistics");
        Console.WriteLine("0. Exit");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }

    private static async Task<bool> ProcessMenuChoice(string? choice)
    {
        switch (choice)
        {
            case "1":
                await ViewAllTasks();
                break;
            case "2":
                await CreateNewTask();
                break;
            case "3":
                await ViewTasksByStatus();
                break;
            case "4":
                await ViewOverdueTasks();
                break;
            case "5":
                await ViewTaskStatistics();
                break;
            case "0":
                return false;
            default:
                Console.WriteLine("Invalid choice. Press any key to continue...");
                Console.ReadKey();
                break;
        }
        return true;
    }

    private static async Task CreateSampleDataIfNeeded()
    {
        var existingTasks = await _taskService.GetAllTasksAsync();
        if (existingTasks.Any())
        {
            return; // Tasks already exist
        }

        Console.WriteLine("Creating sample tasks for demonstration...");

        var sampleTasks = new[]
        {
            new TaskItem
            {
                Title = "Complete Project Documentation",
                Description = "Write comprehensive documentation for the new project",
                Priority = TaskPriority.High,
                Status = TaskStatus.InProgress,
                AssignedTo = "John Doe",
                DueDate = DateTimeOffset.Now.AddDays(3),
                EstimatedHours = 8.0,
                ActualHours = 4.5,
                Tags = "documentation, project, api"
            },
            new TaskItem
            {
                Title = "Fix Critical Login Bug",
                Description = "Users unable to login with special characters",
                Priority = TaskPriority.Critical,
                Status = TaskStatus.NotStarted,
                AssignedTo = "Jane Smith",
                DueDate = DateTimeOffset.Now.AddDays(1),
                EstimatedHours = 4.0,
                Tags = "bug, authentication, security"
            },
            new TaskItem
            {
                Title = "Database Performance Optimization",
                Description = "Optimize slow queries and improve response times",
                Priority = TaskPriority.Normal,
                Status = TaskStatus.Completed,
                AssignedTo = "Bob Johnson",
                DueDate = DateTimeOffset.Now.AddDays(-5),
                EstimatedHours = 6.0,
                ActualHours = 5.5,
                Tags = "database, performance, optimization"
            }
        };

        foreach (var task in sampleTasks)
        {
            await _taskService.CreateTaskAsync(task);
        }

        Console.WriteLine($"Created {sampleTasks.Length} sample tasks.");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static async Task ViewAllTasks()
    {
        Console.Clear();
        Console.WriteLine("=== All Tasks ===");
        Console.WriteLine();

        var tasks = await _taskService.GetAllTasksAsync();
        
        if (!tasks.Any())
        {
            Console.WriteLine("No tasks found.");
        }
        else
        {
            DisplayTaskList(tasks);
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static async Task CreateNewTask()
    {
        Console.Clear();
        Console.WriteLine("=== Create New Task ===");
        Console.WriteLine();

        try
        {
            var task = new TaskItem();

            Console.Write("Title: ");
            task.Title = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(task.Title))
            {
                Console.WriteLine("Title is required!");
                Console.ReadKey();
                return;
            }

            Console.Write("Description: ");
            task.Description = Console.ReadLine() ?? "";

            Console.Write("Assigned to: ");
            task.AssignedTo = Console.ReadLine() ?? "";

            // Priority selection
            Console.WriteLine("\nSelect Priority:");
            Console.WriteLine("1. Low");
            Console.WriteLine("2. Normal");
            Console.WriteLine("3. High");
            Console.WriteLine("4. Critical");
            Console.Write("Choice (1-4): ");
            
            if (int.TryParse(Console.ReadLine(), out var priorityChoice) && 
                priorityChoice >= 1 && priorityChoice <= 4)
            {
                task.Priority = (TaskPriority)priorityChoice;
            }

            // Due date
            Console.Write("Due date (MM/dd/yyyy) or press Enter to skip: ");
            var dueDateInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(dueDateInput) && DateTime.TryParse(dueDateInput, out var dueDate))
            {
                task.DueDate = new DateTimeOffset(dueDate, TimeZoneInfo.Local.GetUtcOffset(dueDate));
            }

            Console.Write("Tags (comma-separated): ");
            task.Tags = Console.ReadLine() ?? "";

            await _taskService.CreateTaskAsync(task);
            Console.WriteLine("\nTask created successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError creating task: {ex.Message}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static async Task ViewTasksByStatus()
    {
        Console.Clear();
        Console.WriteLine("=== View Tasks by Status ===");
        Console.WriteLine();
        Console.WriteLine("Select status:");
        Console.WriteLine("0. Not Started");
        Console.WriteLine("1. In Progress");
        Console.WriteLine("2. Completed");
        Console.WriteLine("3. Cancelled");
        Console.WriteLine("4. On Hold");
        Console.Write("Choice (0-4): ");

        if (int.TryParse(Console.ReadLine(), out var statusChoice) && 
            statusChoice >= 0 && statusChoice <= 4)
        {
            var status = (TaskStatus)statusChoice;
            var tasks = await _taskService.GetTasksByStatusAsync(status);
            
            Console.Clear();
            Console.WriteLine($"=== Tasks with status: {status} ===");
            Console.WriteLine();
            
            if (!tasks.Any())
            {
                Console.WriteLine("No tasks found with this status.");
            }
            else
            {
                DisplayTaskList(tasks);
            }
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static async Task ViewOverdueTasks()
    {
        Console.Clear();
        Console.WriteLine("=== Overdue Tasks ===");
        Console.WriteLine();

        var tasks = await _taskService.GetOverdueTasksAsync();
        
        if (!tasks.Any())
        {
            Console.WriteLine("No overdue tasks found. Great job!");
        }
        else
        {
            Console.WriteLine("⚠️  The following tasks are overdue:");
            Console.WriteLine();
            DisplayTaskList(tasks);
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static async Task ViewTaskStatistics()
    {
        Console.Clear();
        Console.WriteLine("=== Task Statistics ===");
        Console.WriteLine();

        var allTasks = await _taskService.GetAllTasksAsync();
        var taskList = allTasks.ToList();

        if (!taskList.Any())
        {
            Console.WriteLine("No tasks available for statistics.");
        }
        else
        {
            var stats = new Dictionary<string, int>
            {
                ["Total"] = taskList.Count,
                ["Not Started"] = taskList.Count(t => t.Status == TaskStatus.NotStarted),
                ["In Progress"] = taskList.Count(t => t.Status == TaskStatus.InProgress),
                ["Completed"] = taskList.Count(t => t.Status == TaskStatus.Completed),
                ["Cancelled"] = taskList.Count(t => t.Status == TaskStatus.Cancelled),
                ["On Hold"] = taskList.Count(t => t.Status == TaskStatus.OnHold),
                ["Overdue"] = taskList.Count(t => t.IsOverdue()),
                ["High Priority"] = taskList.Count(t => t.Priority == TaskPriority.High || t.Priority == TaskPriority.Critical)
            };

            foreach (var stat in stats)
            {
                Console.WriteLine($"{stat.Key}: {stat.Value}");
            }

            // Calculate completion percentage
            if (stats["Total"] > 0)
            {
                var completionPercentage = (double)stats["Completed"] / stats["Total"] * 100;
                Console.WriteLine($"Completion Rate: {completionPercentage:F1}%");
            }
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private static void DisplayTaskList(IEnumerable<TaskItem> tasks)
    {
        var taskList = tasks.ToList();
        if (!taskList.Any())
        {
            Console.WriteLine("No tasks to display.");
            return;
        }

        // Table headers
        Console.WriteLine($"{"ID",-10} {"Title",-25} {"Status",-15} {"Priority",-10} {"Assignee",-15} {"Due Date",-12}");
        Console.WriteLine(new string('-', 87));

        foreach (var task in taskList)
        {
            var id = task.Id.ToString()[..8];
            var title = task.Title.Length > 22 ? task.Title[..22] + "..." : task.Title;
            var status = task.GetStatusDisplay();
            var priority = task.Priority.ToString();
            var assignee = string.IsNullOrWhiteSpace(task.AssignedTo) ? "Unassigned" : task.AssignedTo;
            var dueDate = task.DueDate?.ToString("MM/dd/yyyy") ?? "No due date";

            // Truncate long values to fit in columns
            if (status.Length > 14) status = status[..14];
            if (priority.Length > 9) priority = priority[..9];
            if (assignee.Length > 14) assignee = assignee[..14];
            if (dueDate.Length > 11) dueDate = dueDate[..11];

            Console.WriteLine($"{id,-10} {title,-25} {status,-15} {priority,-10} {assignee,-15} {dueDate,-12}");
        }
    }
}
