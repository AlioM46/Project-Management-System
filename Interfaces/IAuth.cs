using Project_Management_System.DTOs;
using Project_Management_System.DTOs.UserDTO;
using Project_Management_System.Models;

namespace Project_Management_System.Interfaces
{
    public interface IAuth
    {
        Task<TokenResponseDto> Login(UserLoginDto userInfo);
        Task<object> Register(UserRegistrationDto userInfo);
        System.Threading.Tasks.Task Logout();

        Task<TokenResponseDto> RefreshToken(RefreshTokenRequestDto request);

        bool Send2FACode(User user);
        Task<TokenResponseDto> Validate2FACode(string username, string Token);

        Task<object> ChangePassword(string username, string currentPassword, string newPassword);
        Task<object> ResetPassword(string username, string email, string newPassword);

    }
}
