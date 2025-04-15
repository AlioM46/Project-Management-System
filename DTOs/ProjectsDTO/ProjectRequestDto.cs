using static Project_Management_System.enums;

namespace Project_Management_System.DTOs
{
    public class ProjectRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public enProjectStatus  Status { get; set; } = enProjectStatus.InProgress; // (Active, Completed, Archived)
        public int CreatedByUserId { get; set; } 

    }
}
