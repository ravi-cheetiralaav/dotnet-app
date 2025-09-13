using System;

namespace TaskManager.Models
{
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
}
