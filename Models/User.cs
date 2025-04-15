using Project_Management_System.Models.ManyRelations;
using static Project_Management_System.enums;

namespace Project_Management_System.Models
{
    public class User
    {
        public int Id { get; set; }  // User ID
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public enRoles Role { get; set; } = enRoles.Employee; // (Admin, Manager, Employee)
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastLogin { get; set; } = null;
        public bool IsActive { get; set; } = true;// Whether user is active or not

        public string? RefreshToken { get; set; } = null;
        public DateTime? RefreshTokenExpiryTime { get; set; } = null;

        public string? TwoFactorAuthentication {  get; set; }= null;
        public DateTime? TwoFactorAuthenticationExpiration { get; set; } = null;


        // Navigation Properties
        public ICollection<Project> projects { get; set; }  // Projects the user is part of
        public ICollection<Task> tasks { get; set; }  // Tasks assigned to the user
        public ICollection<Report> reports { get; set; }
        public ICollection<Application> applications { get; set; }


        public ICollection<TaskUser> TaskUsers { get; set; }
        public ICollection<ProjectUser> Projects { get; set; }

    }
}
