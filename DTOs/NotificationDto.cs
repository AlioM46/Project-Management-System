using static Project_Management_System.enums;

namespace Project_Management_System.DTOs
{
    public class NotificationDto
    {
        public string Subject { get; set; }
        public string? Message { get; set; }
        public string NotificationType { get; set; }  // (TaskAssigned, TaskUpdated, DeadlineReminder, etc.)
        public DateTime CreatedDate { get; set; }

    }
}
