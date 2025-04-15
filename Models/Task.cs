using Project_Management_System.Models.ManyRelations;
using System.ComponentModel.DataAnnotations.Schema;
using static Project_Management_System.enums;

namespace Project_Management_System.Models
{
    public class Task
    {


        public int Id { get; set; }  // Task ID


        [Column(TypeName = "nvarchar(40)")]
        public string Title { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string? Description { get; set; }
        public enTasksPriority Priority { get; set; } = enTasksPriority.Medium;  // Priority level
        public enTaskStatus Status { get; set; } = enTaskStatus.InProgress; // (To-Do, In Progress, Completed)


        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }



        // Foreign Keys
        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }  // Project this task belongs to

        [ForeignKey("UserId")]
        public int CreatedByUserId { get; set; }

        // Navigation Properties
        public Project Project { get; set; }

        public User CreatedByUser { get; set; }

        public ICollection<Report> Reports { get; set; }

        public ICollection<TaskUser> TaskUsers { get; set; }

    }
}
