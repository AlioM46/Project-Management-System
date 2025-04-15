using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_System.Models
{
    public class Report
    {
        public int Id { get; set; }  // Comment ID
        public int UserId { get; set; }  // User who made the comment

        [Column(TypeName = "nvarchar(Max)")]
        public string Content { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("TaskId")]
        public int TaskId { get; set; } 

        // Navigation Properties
        public Task task { get; set; }
        public User user { get; set; }

    }
}
