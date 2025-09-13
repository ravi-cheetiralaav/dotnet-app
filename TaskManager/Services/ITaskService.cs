using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Services
{
    /// <summary>
    /// Interface for task management operations
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Creates a new task
        /// </summary>
        /// <param name="task">The task to create</param>
        /// <returns>Task representing the async operation</returns>
        Task<bool> CreateTaskAsync(Models.Task task);

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <param name="task">The task to update</param>
        /// <returns>Task representing the async operation</returns>
        Task<bool> UpdateTaskAsync(Models.Task task);

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
        Task<Models.Task> GetTaskByIdAsync(Guid taskId);

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <returns>Collection of all tasks</returns>
        Task<IEnumerable<Models.Task>> GetAllTasksAsync();

        /// <summary>
        /// Gets tasks by status
        /// </summary>
        /// <param name="status">The status to filter by</param>
        /// <returns>Collection of tasks with the specified status</returns>
        Task<IEnumerable<Models.Task>> GetTasksByStatusAsync(TaskStatus status);

        /// <summary>
        /// Gets tasks by priority
        /// </summary>
        /// <param name="priority">The priority to filter by</param>
        /// <returns>Collection of tasks with the specified priority</returns>
        Task<IEnumerable<Models.Task>> GetTasksByPriorityAsync(TaskPriority priority);

        /// <summary>
        /// Gets overdue tasks
        /// </summary>
        /// <returns>Collection of overdue tasks</returns>
        Task<IEnumerable<Models.Task>> GetOverdueTasksAsync();

        /// <summary>
        /// Gets tasks assigned to a specific person
        /// </summary>
        /// <param name="assignee">The person the tasks are assigned to</param>
        /// <returns>Collection of tasks assigned to the person</returns>
        Task<IEnumerable<Models.Task>> GetTasksByAssigneeAsync(string assignee);

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
}
