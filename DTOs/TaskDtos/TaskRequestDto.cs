using static Project_Management_System.enums;

namespace Project_Management_System.DTOs.TaskDtos
{
    public class TaskRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public enTasksPriority Priority { get; set; } = enTasksPriority.Medium;  // Priority level
        public enTaskStatus Status { get; set; } = enTaskStatus.InProgress; // (To-Do, In Progress, Completed)
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        // Foreign Keys
        public int ProjectId { get; set; }  // Project this task belongs to
        public int CreatedByUserId { get; set; }  // User assigned to this task
        public int ?AssignedToUserId { get; set; }
    }
}
