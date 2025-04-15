using static Project_Management_System.enums;

namespace Project_Management_System.DTOs
{
    public class ApplicationDto
    {
        public int? ProjectId { get; set; }  // Nullable ProjectId
        public int? TaskId { get; set; }  // Nullable TaskId
        public int UserId;
        public enApplicationStatus Status { get; set; } = enApplicationStatus.Pending;
        public DateTime ?CreatedAt { get; set; } = DateTime.Now;



    }
}
