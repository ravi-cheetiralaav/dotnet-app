using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Services
{
    /// <summary>
    /// Implementation of ITaskService for managing tasks
    /// </summary>
    public class TaskService : ITaskService, IDisposable
    {
        private readonly List<Models.Task> _tasks;
        private readonly FileStorageService _storageService;
        private readonly int _maxTasks;
        private readonly bool _autoSaveEnabled;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the TaskService
        /// </summary>
        public TaskService()
        {
            _tasks = new List<Models.Task>();
            _storageService = new FileStorageService();
            
            // Read configuration settings
            if (!int.TryParse(ConfigurationManager.AppSettings["MaxTasksPerUser"], out _maxTasks))
            {
                _maxTasks = 100; // Default value
            }

            if (!bool.TryParse(ConfigurationManager.AppSettings["AutoSaveEnabled"], out _autoSaveEnabled))
            {
                _autoSaveEnabled = true; // Default value
            }
        }

        /// <summary>
        /// Creates a new task
        /// </summary>
        /// <param name="task">The task to create</param>
        /// <returns>Task representing the async operation</returns>
        public async Task<bool> CreateTaskAsync(Models.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title cannot be empty", nameof(task));

            if (_tasks.Count >= _maxTasks)
                throw new InvalidOperationException($"Maximum number of tasks ({_maxTasks}) reached");

            // Ensure the task doesn't already exist
            if (_tasks.Any(t => t.Id == task.Id))
                throw new ArgumentException("Task with the same ID already exists", nameof(task));

            _tasks.Add(task);

            if (_autoSaveEnabled)
            {
                // Use ConfigureAwait(false) to avoid deadlocks
                await SaveAsync().ConfigureAwait(false);
            }

            return true;
        }

        /// <summary>
        /// Updates an existing task
        /// </summary>
        /// <param name="task">The task to update</param>
        /// <returns>Task representing the async operation</returns>
        public async Task<bool> UpdateTaskAsync(Models.Task task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var existingTaskIndex = _tasks.FindIndex(t => t.Id == task.Id);
            if (existingTaskIndex == -1)
                return false;

            task.Touch(); // Update the timestamp
            _tasks[existingTaskIndex] = task;

            if (_autoSaveEnabled)
            {
                // Use ConfigureAwait(false) to avoid deadlocks
                await SaveAsync().ConfigureAwait(false);
            }

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

            if (_autoSaveEnabled)
            {
                // Use ConfigureAwait(false) to avoid deadlocks
                await SaveAsync().ConfigureAwait(false);
            }

            return true;
        }

        /// <summary>
        /// Gets a task by ID
        /// </summary>
        /// <param name="taskId">The ID of the task</param>
        /// <returns>The task if found, null otherwise</returns>
        public async Task<Models.Task> GetTaskByIdAsync(Guid taskId)
        {
            // Simulate async operation for consistency
            await Task.Yield();
            return _tasks.FirstOrDefault(t => t.Id == taskId);
        }

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <returns>Collection of all tasks</returns>
        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            // Simulate async operation for consistency
            await Task.Yield();
            return _tasks.ToList(); // Return a copy to prevent external modification
        }

        /// <summary>
        /// Gets tasks by status
        /// </summary>
        /// <param name="status">The status to filter by</param>
        /// <returns>Collection of tasks with the specified status</returns>
        public async Task<IEnumerable<Models.Task>> GetTasksByStatusAsync(TaskStatus status)
        {
            // Simulate async operation for consistency
            await Task.Yield();
            return _tasks.Where(t => t.Status == status).ToList();
        }

        /// <summary>
        /// Gets tasks by priority
        /// </summary>
        /// <param name="priority">The priority to filter by</param>
        /// <returns>Collection of tasks with the specified priority</returns>
        public async Task<IEnumerable<Models.Task>> GetTasksByPriorityAsync(TaskPriority priority)
        {
            // Simulate async operation for consistency
            await Task.Yield();
            return _tasks.Where(t => t.Priority == priority).ToList();
        }

        /// <summary>
        /// Gets overdue tasks
        /// </summary>
        /// <returns>Collection of overdue tasks</returns>
        public async Task<IEnumerable<Models.Task>> GetOverdueTasksAsync()
        {
            // Simulate async operation for consistency
            await Task.Yield();
            return _tasks.Where(t => t.IsOverdue()).ToList();
        }

        /// <summary>
        /// Gets tasks assigned to a specific person
        /// </summary>
        /// <param name="assignee">The person the tasks are assigned to</param>
        /// <returns>Collection of tasks assigned to the person</returns>
        public async Task<IEnumerable<Models.Task>> GetTasksByAssigneeAsync(string assignee)
        {
            if (string.IsNullOrWhiteSpace(assignee))
                throw new ArgumentException("Assignee cannot be null or empty", nameof(assignee));

            // Simulate async operation for consistency
            await Task.Yield();
            return _tasks.Where(t => string.Equals(t.AssignedTo, assignee, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Saves all tasks to persistent storage
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        public async Task<bool> SaveAsync()
        {
            // Use ConfigureAwait(false) to avoid deadlocks
            return await _storageService.SaveTasksAsync(_tasks).ConfigureAwait(false);
        }

        /// <summary>
        /// Loads all tasks from persistent storage
        /// </summary>
        /// <returns>Task representing the async operation</returns>
        public async Task<bool> LoadAsync()
        {
            try
            {
                // Use ConfigureAwait(false) to avoid deadlocks
                var loadedTasks = await _storageService.LoadTasksAsync().ConfigureAwait(false);
                
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
        /// Gets task statistics
        /// </summary>
        /// <returns>Dictionary containing task statistics</returns>
        public Dictionary<string, int> GetTaskStatistics()
        {
            var stats = new Dictionary<string, int>
            {
                ["Total"] = _tasks.Count,
                ["NotStarted"] = _tasks.Count(t => t.Status == TaskStatus.NotStarted),
                ["InProgress"] = _tasks.Count(t => t.Status == TaskStatus.InProgress),
                ["Completed"] = _tasks.Count(t => t.Status == TaskStatus.Completed),
                ["Cancelled"] = _tasks.Count(t => t.Status == TaskStatus.Cancelled),
                ["OnHold"] = _tasks.Count(t => t.Status == TaskStatus.OnHold),
                ["Overdue"] = _tasks.Count(t => t.IsOverdue()),
                ["High Priority"] = _tasks.Count(t => t.Priority == TaskPriority.High || t.Priority == TaskPriority.Critical)
            };

            return stats;
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
            if (!_disposed)
            {
                if (disposing)
                {
                    _storageService?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
