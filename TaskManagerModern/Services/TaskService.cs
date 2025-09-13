using System.Text.Json;
using TaskManagerModern.Models;

namespace TaskManagerModern.Services;

/// <summary>
/// Implementation of ITaskService for managing tasks with JSON storage
/// </summary>
public class TaskService : ITaskService
{
    private readonly List<TaskItem> _tasks = [];
    private readonly string _filePath = "tasks.json";
    private readonly int _maxTasks = 1000;
    private bool _disposed;

    /// <summary>
    /// Creates a new task
    /// </summary>
    /// <param name="task">The task to create</param>
    /// <returns>Task representing the async operation</returns>
    public async Task<bool> CreateTaskAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        if (string.IsNullOrWhiteSpace(task.Title))
            throw new ArgumentException("Task title cannot be empty", nameof(task));

        if (_tasks.Count >= _maxTasks)
            throw new InvalidOperationException($"Maximum number of tasks ({_maxTasks}) reached");

        // Ensure the task doesn't already exist
        if (_tasks.Any(t => t.Id == task.Id))
            throw new ArgumentException("Task with the same ID already exists", nameof(task));

        _tasks.Add(task);

        // Auto-save
        await SaveAsync();

        return true;
    }

    /// <summary>
    /// Updates an existing task
    /// </summary>
    /// <param name="task">The task to update</param>
    /// <returns>Task representing the async operation</returns>
    public async Task<bool> UpdateTaskAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        var existingTaskIndex = _tasks.FindIndex(t => t.Id == task.Id);
        if (existingTaskIndex == -1)
            return false;

        task.Touch(); // Update the timestamp
        _tasks[existingTaskIndex] = task;

        // Auto-save
        await SaveAsync();

        return true;
    }

    /// <summary>
    /// Deletes a task by ID
    /// </summary>
    /// <param name="taskId">The ID of the task to delete</param>
    /// <returns>Task representing the async operation</returns>
    public async Task<bool> DeleteTaskAsync(Guid taskId)
    {
        var taskToRemove = _tasks.FirstOrDefault(t => t.Id == taskId);
        if (taskToRemove == null)
            return false;

        _tasks.Remove(taskToRemove);

        // Auto-save
        await SaveAsync();

        return true;
    }

    /// <summary>
    /// Gets a task by ID
    /// </summary>
    /// <param name="taskId">The ID of the task</param>
    /// <returns>The task if found, null otherwise</returns>
    public Task<TaskItem?> GetTaskByIdAsync(Guid taskId)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
        return Task.FromResult(task);
    }

    /// <summary>
    /// Gets all tasks
    /// </summary>
    /// <returns>Collection of all tasks</returns>
    public Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return Task.FromResult(_tasks.AsEnumerable());
    }

    /// <summary>
    /// Gets tasks by status
    /// </summary>
    /// <param name="status">The status to filter by</param>
    /// <returns>Collection of tasks with the specified status</returns>
    public Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Models.TaskStatus status)
    {
        var tasks = _tasks.Where(t => t.Status == status);
        return Task.FromResult(tasks);
    }

    /// <summary>
    /// Gets tasks by priority
    /// </summary>
    /// <param name="priority">The priority to filter by</param>
    /// <returns>Collection of tasks with the specified priority</returns>
    public Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(TaskPriority priority)
    {
        var tasks = _tasks.Where(t => t.Priority == priority);
        return Task.FromResult(tasks);
    }

    /// <summary>
    /// Gets overdue tasks
    /// </summary>
    /// <returns>Collection of overdue tasks</returns>
    public Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
    {
        var tasks = _tasks.Where(t => t.IsOverdue());
        return Task.FromResult(tasks);
    }

    /// <summary>
    /// Gets tasks assigned to a specific person
    /// </summary>
    /// <param name="assignee">The person the tasks are assigned to</param>
    /// <returns>Collection of tasks assigned to the person</returns>
    public Task<IEnumerable<TaskItem>> GetTasksByAssigneeAsync(string assignee)
    {
        if (string.IsNullOrWhiteSpace(assignee))
            throw new ArgumentException("Assignee cannot be null or empty", nameof(assignee));

        var tasks = _tasks.Where(t => string.Equals(t.AssignedTo, assignee, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(tasks);
    }

    /// <summary>
    /// Saves all tasks to persistent storage
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    public async Task<bool> SaveAsync()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(_tasks, options);
            await File.WriteAllTextAsync(_filePath, json);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving tasks: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Loads all tasks from persistent storage
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    public async Task<bool> LoadAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return true; // No file exists, that's okay
            }

            var json = await File.ReadAllTextAsync(_filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var loadedTasks = JsonSerializer.Deserialize<List<TaskItem>>(json, options) ?? [];
            
            _tasks.Clear();
            _tasks.AddRange(loadedTasks);
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading tasks: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Disposes the TaskService
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Protected dispose method
    /// </summary>
    /// <param name="disposing">True if disposing managed resources</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            // Save before disposing
            SaveAsync().Wait();
        }
        _disposed = true;
    }
}
