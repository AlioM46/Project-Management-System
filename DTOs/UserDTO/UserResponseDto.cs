namespace Project_Management_System.DTOs.UserDTO
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
    }
}
