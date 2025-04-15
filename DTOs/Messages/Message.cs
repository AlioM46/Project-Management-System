namespace Project_Management_System.DTOs.Messages
{
    public class Message
    {
        public bool IsSuccess { get; set; } = true;
        public  string SuccessMessage {  get; set; }
        public  string ErrorMessage {  get; set; }
    }
}
