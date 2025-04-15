using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.DTOs.UserDTO;
using Project_Management_System.Interfaces;
using Project_Management_System.Models;
using Project_Management_System.Services;
using System.Security.Claims;

namespace Project_Management_System.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUser _user, IAuth _auth) : ControllerBase
    {

        /*        
        Task<UserResponseDto> GetUserByIdAsync(int Id);
        Task<UserResponseDto> GetUserByUsernameAsync(string Username);
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto> UpdateUserAsync(UserRegistrationDto User, int Id);
        Task<TokenResponseDto> Login(UserLoginDto userInfo);
        Task<TokenResponseDto> Register(UserRegistrationDto userInfo);
        Task Logout();
        Task<TokenResponseDto> RefreshToken(RefreshTokenRequestDto request);
        
        Task<Message> AssignToTask(Message message, int ManagerId, int UserId, int TaskId);
        Task<Message> ApplyToTask(Message message, int UserId, int TaskId);

        Task<Message> AssignToProject(Message message, int ManagerId, int UserId, int ProjectId);
        Task<Message> ApplyToProject(Message message, int UserId, int ProjectId);
*/


        [Authorize(Policy = "AdminOrManager")]
        [HttpPost("assign-to-task/{TaskId}/{UserId}")]
        public async Task<ActionResult<Message>> AssignToTask(int UserId, int TaskId)
        {
            var managerIdClaim = HttpContext.User.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

            int managerId = int.TryParse(managerIdClaim, out var id) ? id : 0; // or handle error if 0 is invalid

            var message = await _user.AssignToTask(managerId, UserId, TaskId);

            return message.IsSuccess ? Ok(message) : BadRequest(message);
        }

        [Authorize(Policy = "AdminOrManager")]
        [HttpPost("assign-to-project/{ProjectId}/{UserId}")]
        public async Task<ActionResult<Message>> AssignToProject(int UserId, int ProjectId)
        {
            var managerIdClaim = HttpContext.User.FindFirst(e => e.Type == ClaimTypes.NameIdentifier)?.Value;

            int managerId = int.TryParse(managerIdClaim, out var id) ? id : 0; // or handle error if 0 is invalid

            var message = await _user.AssignToProject(managerId, UserId, ProjectId);

            return message.IsSuccess ? Ok(message) : BadRequest(message);
        }

        [Authorize(Policy = "AdminOrManager")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid Id");
                }

                var user = await _user.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound("NOT FOUND USER");
                }


                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "AdminOrManager")]
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserResponseDto>> GetUserByUsernameAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest("Invalid username");
                }

                var user = await _user.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return NotFound("NOT FOUND USER");
                }


                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Authorize(Policy = "AdminOrManager")]
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            try
            {
                var UsersList = await _user.GetAllUsersAsync();
                if (UsersList.Count() <= 0)
                {
                    return NoContent();
                }
                return Ok(UsersList);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }







        [Authorize(Policy = "EmployeeAndAbove")]
        [HttpPost("apply-to-project/{ProjectId}")]
        public async Task<ActionResult<Message>> ApplyToProject(int ProjectId)
        {
            // ? = Null-conditional Operator -> Safely access -> if it's null, it will not crash your app, cuz you caught it.
            var userIdClaim = HttpContext.User.FindFirst((e) => e.Type == ClaimTypes.NameIdentifier)?.Value;

            int userId = int.TryParse(userIdClaim, out int id) ? id : 0;

            var message = await _user.ApplyToProject(userId, ProjectId);

            return message.IsSuccess ? Ok(message) : BadRequest(message);


        }

        [Authorize(Policy = "EmployeeAndAbove")]
        [HttpPost("apply-to-task/{TaskId}")]
        public async Task<ActionResult<Message>> ApplyToTask(int TaskId)
        {
            // ? = Null-conditional Operator -> Safely access -> if it's null, it will not crash your app, cuz you caught it.
            var userIdClaim = HttpContext.User.FindFirst((e) => e.Type == ClaimTypes.NameIdentifier)?.Value;

            int userId = int.TryParse(userIdClaim, out int id) ? id : 0;

            var message = await _user.ApplyToTask(userId, TaskId);

            return message.IsSuccess ? Ok(message) : BadRequest(message);
        }


        [Authorize(Policy = "EmployeeAndAbove")]
        [HttpPut("")]
        public async Task<ActionResult<UserResponseDto>> UpdateUserAsync(UserRegistrationDto userInfo, int Id)
        {
            try
            {

                if (Id <= 0)
                {
                    return BadRequest();
                }

                var user = await _user.GetUserByIdAsync(Id);

                if (user == null)
                {
                    return NotFound();
                }
                var userResponse = await _user.UpdateUserAsync(userInfo, Id);

                return Ok(userResponse);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserLoginDto userInfo)
        {
            try
            {
                var ResponseToken = await _auth.Login(userInfo);

                return ResponseToken == null ? NoContent() : Ok(ResponseToken);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("register")]
        public async Task<ActionResult<object>> Register(UserRegistrationDto userInfo)
        {
            try
            {
                var ResponseObject = await _auth.Register(userInfo);

                return ResponseObject == null ? NoContent() : Ok(ResponseObject);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Policy = "EmployeeAndAbove")]
        [HttpPost("change-password")]
        public async Task<ActionResult<object>> ChangePassword([FromQuery] string username, [FromQuery] string currentPassword, [FromQuery] string newPassword)
        {
            try
            {
                var ResponseObject = await _auth.ChangePassword(username, currentPassword, newPassword);

                return ResponseObject == null ? NoContent() : Ok(ResponseObject);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        
        [Authorize(Policy = "EmployeeAndAbove")]
        [HttpPost("reset-password")]
        public async Task<ActionResult<object>> ResetPassword([FromQuery] string username, [FromQuery] string email, [FromQuery] string newPassword)
        {
            try
            {
                var ResponseObject = await _auth.ResetPassword(username, email, newPassword);

                return ResponseObject == null ? NoContent() : Ok(ResponseObject);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("validate-2fa")]
        public async Task<ActionResult<TokenResponseDto>> Validate2FA([FromQuery] string username, [FromQuery] string Token)
        {
            try
            {
                var ResponseObject = await _auth.Validate2FACode(username, Token);

                return ResponseObject == null ? NoContent() : Ok(ResponseObject);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                Thread.Sleep(2000);
                return Ok("Loggin out");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
      
        
        [HttpPost("Refresh-Token")]
        [Authorize(Policy = "EmployeeAndAbove")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            try
            {
                var ResponseToken = await _auth.RefreshToken(request);
                return ResponseToken == null ? NoContent() : Ok(ResponseToken);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


    }
}
