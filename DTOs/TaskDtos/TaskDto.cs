using static Project_Management_System.enums;

namespace Project_Management_System.DTOs.TaskDtos
{
    public class TaskDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public enTasksPriority? Priority { get; set; } = null;  // Priority level
        public enTaskStatus? Status { get; set; } = null; // (To-Do, In Progress, Completed)


        public DateTime CreateDate { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
        // Foreign Keys
        public int ProjectId { get; set; }  // Project this task belongs to
        public int CreatedByUserId { get; set; }  // User assigned to this task
        public int? AssignedToUserId { get; set; }

    }
}
