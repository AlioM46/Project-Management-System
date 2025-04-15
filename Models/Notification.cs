
using System.ComponentModel.DataAnnotations.Schema;
using static Project_Management_System.enums;

namespace Project_Management_System.Models

{
    public class Notification
    {
        public int Id { get; set; }  // Notification ID
        public int UserId { get; set; }  // User the notification is for


        [Column(TypeName ="nvarchar(50)")]
        public string Subject { get; set; }


        [Column(TypeName ="nvarchar(100)")]
        public string? Message { get; set; }


        public enNotificationsTypes NotificationType { get; set; }  // (TaskAssigned, TaskUpdated, DeadlineReminder, etc.)
        public DateTime CreatedDate { get; set; }

        // Navigation Properties
        public User user { get; set; }

    }
}
