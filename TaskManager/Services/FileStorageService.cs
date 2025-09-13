using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskManager.Models;
using System.Globalization;

namespace TaskManager.Services
{
    /// <summary>
    /// Service for handling file storage operations using XML
    /// </summary>
    public class FileStorageService : IDisposable
    {
        private readonly string _filePath;
        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the FileStorageService
        /// </summary>
        public FileStorageService()
        {
            _filePath = ConfigurationManager.AppSettings["TaskStorageFile"] ?? "tasks.xml";
        }

        /// <summary>
        /// Saves tasks to XML file
        /// </summary>
        /// <param name="tasks">Collection of tasks to save</param>
        /// <returns>Task representing the async operation</returns>
        public async Task<bool> SaveTasksAsync(IEnumerable<Models.Task> tasks)
        {
            try
            {
                var doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Tasks",
                        tasks.Select(task => CreateTaskElement(task))
                    )
                );

                // Use ConfigureAwait(false) to avoid deadlocks
                await Task.Run(() => doc.Save(_filePath)).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                // In a real application, you would use a logging framework
                Console.WriteLine($"Error saving tasks: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads tasks from XML file
        /// </summary>
        /// <returns>Collection of loaded tasks</returns>
        public async Task<IEnumerable<Models.Task>> LoadTasksAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return Enumerable.Empty<Models.Task>();
                }

                // Use ConfigureAwait(false) to avoid deadlocks
                var doc = await Task.Run(() => XDocument.Load(_filePath)).ConfigureAwait(false);
                
                var tasks = doc.Root.Elements("Task")
                    .Select(ParseTaskElement)
                    .Where(task => task != null)
                    .ToList();

                return tasks;
            }
            catch (Exception ex)
            {
                // In a real application, you would use a logging framework
                Console.WriteLine($"Error loading tasks: {ex.Message}");
                return Enumerable.Empty<Models.Task>();
            }
        }

        /// <summary>
        /// Creates an XML element from a task
        /// </summary>
        /// <param name="task">The task to convert</param>
        /// <returns>XElement representing the task</returns>
        private XElement CreateTaskElement(Models.Task task)
        {
            return new XElement("Task",
                new XElement("Id", task.Id),
                new XElement("Title", task.Title ?? string.Empty),
                new XElement("Description", task.Description ?? string.Empty),
                new XElement("Priority", (int)task.Priority),
                new XElement("Status", (int)task.Status),
                new XElement("CreatedAt", task.CreatedAt.ToString("O", CultureInfo.InvariantCulture)),
                new XElement("UpdatedAt", task.UpdatedAt.ToString("O", CultureInfo.InvariantCulture)),
                new XElement("DueDate", task.DueDate?.ToString("O", CultureInfo.InvariantCulture) ?? string.Empty),
                new XElement("AssignedTo", task.AssignedTo ?? string.Empty),
                new XElement("Tags", task.Tags ?? string.Empty),
                new XElement("EstimatedHours", task.EstimatedHours?.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
                new XElement("ActualHours", task.ActualHours.ToString(CultureInfo.InvariantCulture))
            );
        }

        /// <summary>
        /// Parses an XML element to create a task
        /// </summary>
        /// <param name="element">The XML element to parse</param>
        /// <returns>Task object or null if parsing fails</returns>
        private Models.Task ParseTaskElement(XElement element)
        {
            try
            {
                var task = new Models.Task();

                // Parse ID
                if (Guid.TryParse(element.Element("Id")?.Value, out var id))
                {
                    task.Id = id;
                }

                // Parse basic properties
                task.Title = element.Element("Title")?.Value ?? string.Empty;
                task.Description = element.Element("Description")?.Value ?? string.Empty;

                // Parse enums
                if (int.TryParse(element.Element("Priority")?.Value, out var priority))
                {
                    task.Priority = (TaskPriority)priority;
                }

                if (int.TryParse(element.Element("Status")?.Value, out var status))
                {
                    task.Status = (TaskStatus)status;
                }

                // Parse dates using DateTimeOffset for proper timezone handling
                if (DateTimeOffset.TryParse(element.Element("CreatedAt")?.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var createdAt))
                {
                    task.CreatedAt = createdAt;
                }

                if (DateTimeOffset.TryParse(element.Element("UpdatedAt")?.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var updatedAt))
                {
                    task.UpdatedAt = updatedAt;
                }

                var dueDateValue = element.Element("DueDate")?.Value;
                if (!string.IsNullOrEmpty(dueDateValue) && 
                    DateTimeOffset.TryParse(dueDateValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var dueDate))
                {
                    task.DueDate = dueDate;
                }

                // Parse other properties
                task.AssignedTo = element.Element("AssignedTo")?.Value ?? string.Empty;
                task.Tags = element.Element("Tags")?.Value ?? string.Empty;

                var estimatedHoursValue = element.Element("EstimatedHours")?.Value;
                if (!string.IsNullOrEmpty(estimatedHoursValue) && 
                    double.TryParse(estimatedHoursValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var estimatedHours))
                {
                    task.EstimatedHours = estimatedHours;
                }

                if (double.TryParse(element.Element("ActualHours")?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var actualHours))
                {
                    task.ActualHours = actualHours;
                }

                return task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing task element: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Disposes the FileStorageService
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
                    // Dispose managed resources here if any
                }
                _disposed = true;
            }
        }
    }
}
