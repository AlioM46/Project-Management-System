using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_System.Models.ManyRelations
{
    public class ProjectUser
    {

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("ProjectId")]
        public int ProjectId { get; set; }
        public DateTime ?AssignedDate { get; set; } = DateTime.Now;


        public User user { get; set; }
        public Project project { get; set; }

    }
}
