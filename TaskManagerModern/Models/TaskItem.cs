namespace TaskManagerModern.Models;

/// <summary>
/// Represents the priority levels for tasks
/// </summary>
public enum TaskPriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Critical = 4
}

/// <summary>
/// Represents the status of a task
/// </summary>
public enum TaskStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3,
    OnHold = 4
}

/// <summary>
/// Represents a task in the task management system
/// </summary>
public class TaskItem : IEquatable<TaskItem>
{
    /// <summary>
    /// Unique identifier for the task
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Title of the task
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the task
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Priority level of the task
    /// </summary>
    public TaskPriority Priority { get; set; } = TaskPriority.Normal;

    /// <summary>
    /// Current status of the task
    /// </summary>
    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

    /// <summary>
    /// Date and time when the task was created
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Date and time when the task was last updated
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// Optional due date for the task
    /// </summary>
    public DateTimeOffset? DueDate { get; set; }

    /// <summary>
    /// Person assigned to the task
    /// </summary>
    public string AssignedTo { get; set; } = string.Empty;

    /// <summary>
    /// Additional tags for categorization
    /// </summary>
    public string Tags { get; set; } = string.Empty;

    /// <summary>
    /// Estimated hours to complete the task
    /// </summary>
    public double? EstimatedHours { get; set; }

    /// <summary>
    /// Actual hours spent on the task
    /// </summary>
    public double ActualHours { get; set; }

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
        if (!DueDate.HasValue || Status is TaskStatus.Completed or TaskStatus.Cancelled)
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
        var statusText = Status switch
        {
            TaskStatus.NotStarted => "Not Started",
            TaskStatus.InProgress => "In Progress",
            TaskStatus.OnHold => "On Hold",
            _ => Status.ToString()
        };

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
            ? $" | Due: {DueDate.Value:MM/dd/yyyy}" 
            : string.Empty;
        
        return $"[{Id.ToString()[..8]}] {Title} | {GetStatusDisplay()} | Priority: {Priority}{dueInfo}";
    }

    /// <summary>
    /// Determines whether the specified TaskItem is equal to the current TaskItem
    /// </summary>
    /// <param name="other">The TaskItem to compare with the current TaskItem</param>
    /// <returns>true if the specified TaskItem is equal to the current TaskItem; otherwise, false</returns>
    public bool Equals(TaskItem? other)
    {
        return other is not null && Id.Equals(other.Id);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current TaskItem
    /// </summary>
    /// <param name="obj">The object to compare with the current TaskItem</param>
    /// <returns>true if the specified object is equal to the current TaskItem; otherwise, false</returns>
    public override bool Equals(object? obj)
    {
        return Equals(obj as TaskItem);
    }

    /// <summary>
    /// Serves as the default hash function
    /// </summary>
    /// <returns>A hash code for the current TaskItem</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
