using TaskManagerModern.Models;

namespace TaskManagerModern.Services;

/// <summary>
/// Interface for task management operations
/// </summary>
public interface ITaskService : IDisposable
{
    /// <summary>
    /// Creates a new task
    /// </summary>
    /// <param name="task">The task to create</param>
    /// <returns>Task representing the async operation</returns>
    Task<bool> CreateTaskAsync(TaskItem task);

    /// <summary>
    /// Updates an existing task
    /// </summary>
    /// <param name="task">The task to update</param>
    /// <returns>Task representing the async operation</returns>
    Task<bool> UpdateTaskAsync(TaskItem task);

    /// <summary>
    /// Deletes a task by ID
    /// </summary>
    /// <param name="taskId">The ID of the task to delete</param>
    /// <returns>Task representing the async operation</returns>
    Task<bool> DeleteTaskAsync(Guid taskId);

    /// <summary>
    /// Gets a task by ID
    /// </summary>
    /// <param name="taskId">The ID of the task</param>
    /// <returns>The task if found, null otherwise</returns>
    Task<TaskItem?> GetTaskByIdAsync(Guid taskId);

    /// <summary>
    /// Gets all tasks
    /// </summary>
    /// <returns>Collection of all tasks</returns>
    Task<IEnumerable<TaskItem>> GetAllTasksAsync();

    /// <summary>
    /// Gets tasks by status
    /// </summary>
    /// <param name="status">The status to filter by</param>
    /// <returns>Collection of tasks with the specified status</returns>
    Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(Models.TaskStatus status);

    /// <summary>
    /// Gets tasks by priority
    /// </summary>
    /// <param name="priority">The priority to filter by</param>
    /// <returns>Collection of tasks with the specified priority</returns>
    Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(TaskPriority priority);

    /// <summary>
    /// Gets overdue tasks
    /// </summary>
    /// <returns>Collection of overdue tasks</returns>
    Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();

    /// <summary>
    /// Gets tasks assigned to a specific person
    /// </summary>
    /// <param name="assignee">The person the tasks are assigned to</param>
    /// <returns>Collection of tasks assigned to the person</returns>
    Task<IEnumerable<TaskItem>> GetTasksByAssigneeAsync(string assignee);

    /// <summary>
    /// Saves all tasks to persistent storage
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    Task<bool> SaveAsync();

    /// <summary>
    /// Loads all tasks from persistent storage
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    Task<bool> LoadAsync();
}
