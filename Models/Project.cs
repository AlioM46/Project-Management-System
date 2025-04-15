using Project_Management_System.Models.ManyRelations;
using System.ComponentModel.DataAnnotations.Schema;
using static Project_Management_System.enums;

namespace Project_Management_System.Models
{
    public class Project
    {
        public int Id { get; set; }  // Project ID

        [Column(TypeName = "nvarchar(50)")]

        public string Title { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Description { get; set; } = string.Empty;


        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public enProjectStatus Status { get; set; } = enProjectStatus.InProgress; // (Active, Completed, Archived)


        [ForeignKey("UserId")]
        public int UserId { get; set; }  // Created by User

        // Navigation Properties
        public User User { get; set; }
        public ICollection<Task> tasks { get; set; }
        public ICollection<ProjectUser> users { get; set; }

    }
}
