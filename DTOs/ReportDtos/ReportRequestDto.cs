namespace Project_Management_System.DTOs.ReportDtos
{
    public class ReportRequestDto
    {
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string Content { get; set; }
        public int TaskId {  get; set; }
    }
}
