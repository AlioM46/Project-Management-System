using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_Management_System.Models.ManyRelations
{
    public class TaskUser
    {
        public int TaskId { get; set; }
        public Task Task { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        // Optional fields
        public DateTime AssignedDate { get; set; } = DateTime.Now;
    }
}
