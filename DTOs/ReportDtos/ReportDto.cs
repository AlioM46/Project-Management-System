namespace Project_Management_System.DTOs.ReportDtos
{
    public class ReportDto
    {
        public int CreatedByUserId { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string Content { get; set; }

        public int TaskId { get; set; }

    }
}
