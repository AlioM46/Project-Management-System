using static Project_Management_System.enums;

namespace Project_Management_System.DTOs
{
    public class ProjectResponseDto
    {
        public string Title { get; set; }
        public string? Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public enProjectStatus Status { get; set; } // (Active, Completed, Archived)
        public int CreatedByUserId { get; set; }

    }
}
