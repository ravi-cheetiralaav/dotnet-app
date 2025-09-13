using System;
using System.Globalization;

namespace TaskManager.Models
{
    /// <summary>
    /// Represents a task in the task management system
    /// </summary>
    public class Task : IEquatable<Task>
    {
        /// <summary>
        /// Unique identifier for the task
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title of the task
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Detailed description of the task
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Priority level of the task
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// Current status of the task
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Date and time when the task was created
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Date and time when the task was last updated
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// Optional due date for the task
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }

        /// <summary>
        /// Person assigned to the task
        /// </summary>
        public string AssignedTo { get; set; }

        /// <summary>
        /// Additional tags for categorization
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Estimated hours to complete the task
        /// </summary>
        public double? EstimatedHours { get; set; }

        /// <summary>
        /// Actual hours spent on the task
        /// </summary>
        public double ActualHours { get; set; }

        /// <summary>
        /// Initializes a new instance of the Task class
        /// </summary>
        public Task()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTimeOffset.Now;
            UpdatedAt = DateTimeOffset.Now;
            Status = TaskStatus.NotStarted;
            Priority = TaskPriority.Normal;
            ActualHours = 0.0;
        }

        /// <summary>
        /// Updates the task's timestamp
        /// </summary>
        public void Touch()
        {
            UpdatedAt = DateTimeOffset.Now;
        }

        /// <summary>
        /// Checks if the task is overdue
        /// </summary>
        /// <returns>True if the task is overdue, false otherwise</returns>
        public bool IsOverdue()
        {
            if (!DueDate.HasValue || Status == TaskStatus.Completed || Status == TaskStatus.Cancelled)
            {
                return false;
            }
            
            return DateTimeOffset.Now > DueDate.Value;
        }

        /// <summary>
        /// Gets the number of days until the due date
        /// </summary>
        /// <returns>Number of days until due date, or null if no due date is set</returns>
        public int? DaysUntilDue()
        {
            if (!DueDate.HasValue)
            {
                return null;
            }

            var timeSpan = DueDate.Value - DateTimeOffset.Now;
            return (int)Math.Ceiling(timeSpan.TotalDays);
        }

        /// <summary>
        /// Gets a display-friendly status string
        /// </summary>
        /// <returns>Formatted status string</returns>
        public string GetStatusDisplay()
        {
            var statusText = Status.ToString();
            
            // Add spacing for camelCase enum values
            if (Status == TaskStatus.NotStarted)
                statusText = "Not Started";
            else if (Status == TaskStatus.InProgress)
                statusText = "In Progress";
            else if (Status == TaskStatus.OnHold)
                statusText = "On Hold";

            if (IsOverdue())
            {
                statusText += " (OVERDUE)";
            }

            return statusText;
        }

        /// <summary>
        /// Returns a string representation of the task
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            var dueInfo = DueDate.HasValue 
                ? $" | Due: {DueDate.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}" 
                : string.Empty;
            
            return $"[{Id.ToString().Substring(0, 8)}] {Title} | {GetStatusDisplay()} | Priority: {Priority}{dueInfo}";
        }

        /// <summary>
        /// Determines whether the specified Task is equal to the current Task
        /// </summary>
        /// <param name="other">The Task to compare with the current Task</param>
        /// <returns>true if the specified Task is equal to the current Task; otherwise, false</returns>
        public bool Equals(Task other)
        {
            if (other == null)
                return false;

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current Task
        /// </summary>
        /// <param name="obj">The object to compare with the current Task</param>
        /// <returns>true if the specified object is equal to the current Task; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Task);
        }

        /// <summary>
        /// Serves as the default hash function
        /// </summary>
        /// <returns>A hash code for the current Task</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
