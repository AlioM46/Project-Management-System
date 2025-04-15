using static Project_Management_System.enums;

namespace Project_Management_System.Models
{

    public class Application
    {
        public int Id { get; set; }

        // Foreign key to the User (Employee) applying
        public int UserId { get; set; }
        public User user { get; set; }  // Navigation property to User

        // Either apply for a project or a task, but not both
        public int? ProjectId { get; set; }  // Nullable ProjectId
        public Project project { get; set; } // Navigation property for Project

        public int? TaskId { get; set; }  // Nullable TaskId
        public Task task { get; set; } // Navigation property for Task

        // Status of the application (Pending, Accepted, Rejected)
        public enApplicationStatus Status { get; set; } = enApplicationStatus.Pending;

        // Timestamp for when the application was created
        public DateTime CreatedAt { get; set; }

        // Optional comments from the project manager or task manager regarding the application
    }

}
