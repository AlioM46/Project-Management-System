namespace Project_Management_System.DTOs
{
    public class RefreshTokenRequestDto
    {
        public required int Id { get; set; }
        public required string RefreshToken { get; set; } = string.Empty;
    }
}
