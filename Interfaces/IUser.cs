using Project_Management_System.DTOs.Messages;
using Project_Management_System.DTOs.UserDTO;
using Project_Management_System.Models;
using Task = System.Threading.Tasks.Task;

namespace Project_Management_System.Interfaces
{
    public interface IUser
    {



        Task<Message> AssignToTask( int ManagerId, int UserId, int TaskId);
        Task<Message> ApplyToTask( int UserId, int TaskId);

        Task<Message> AssignToProject(int ManagerId, int UserId, int ProjectId);
        Task<Message> ApplyToProject( int UserId, int ProjectId);


        Task<UserResponseDto> GetUserByIdAsync(int Id);
        Task<UserResponseDto> GetUserByUsernameAsync(string Username);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();

        Task<UserResponseDto> UpdateUserAsync(UserRegistrationDto User, int Id);





    }
}
