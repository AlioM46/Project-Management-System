using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Project_Management_System.Data;
using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.DTOs.UserDTO;
using Project_Management_System.Interfaces;
using Project_Management_System.Models;
using Project_Management_System.Models.ManyRelations;
using Project_Management_System.Options;

namespace Project_Management_System.Services
{
    public class UserService(AppDbContext dbContext) : IUser
    {
        public async Task<Message> ApplyToProject(int UserId, int ProjectId)
        {
            var user = await dbContext.Users.FindAsync(UserId);

            if (user == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not found." };
            }

            if (!user.IsActive)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not active, contact admin." };
            }

            if (!await dbContext.Projects.AnyAsync((e) => e.Id == ProjectId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "project is not found." };
            }

            if (await dbContext.ProjectUsers.AnyAsync((e) => e.ProjectId == ProjectId && e.UserId == UserId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "user already assigned to this project." };
            }

            var projectUser = new ProjectUser { UserId = UserId, ProjectId = ProjectId };

            await dbContext.ProjectUsers.AddAsync(projectUser);
            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "User Added to This Project Successfully." };

        }
        public async Task<Message> AssignToProject(int ManagerId, int UserId, int ProjectId)
        {
            var user = await dbContext.Users.FindAsync(UserId);
            var manager = await dbContext.Users.FindAsync(ManagerId);

            if (UserId == ManagerId)
            {
                return new Message { IsSuccess = false, ErrorMessage = "You Can't Assign your self." };
            }

            if (user == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not found." };
            }

            if (!user.IsActive)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not active, contact admin." };
            }

            if (manager == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "manager is not found." };
            }

            if (!manager.IsActive)
            {
                return new Message { IsSuccess = false, ErrorMessage = "manager is not active, contact admin." };
            }




            if (!await dbContext.Projects.AnyAsync((e) => e.Id == ProjectId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "project is not found." };
            }

            if (await dbContext.ProjectUsers.AnyAsync((e) => e.ProjectId == ProjectId && e.UserId == UserId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "user already assigned to this project." };
            }

            var projectUser = new ProjectUser { UserId = UserId, ProjectId = ProjectId };

            await dbContext.ProjectUsers.AddAsync(projectUser);
            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "User Added to This Project Successfully." };
        }
        public async Task<Message> ApplyToTask(int UserId, int TaskId)
        {

            var user = await dbContext.Users.FindAsync(UserId);

            if (user == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not found." };
            }

            if (!user.IsActive)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not active, contact admin." };
            }
            // user should first apply to the project of this task.


            // COMPLETE HERE.
            // FIND THE TASK THEN TAKE THE PROJECT ID
            var task = await dbContext.Tasks.FindAsync(TaskId);

            if (task == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "task is not found." };
            }

            int projectId = task.ProjectId;

            if (!await dbContext.ProjectUsers.AnyAsync((e) => e.ProjectId == projectId && e.UserId == UserId))
            {
                return new Message { IsSuccess = false, ErrorMessage = $"user is not in the project #{projectId} of this task" };
            }

            if (await dbContext.TaskUsers.AnyAsync((e) => e.TaskId == TaskId && e.UserId == UserId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "user already assigned to this task." };
            }

            var taskUser = new TaskUser { UserId = UserId, TaskId = TaskId };

            await dbContext.TaskUsers.AddAsync(taskUser);
            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "User Added to This Task Successfully." };

        }
        public async Task<Message> AssignToTask(int ManagerId, int UserId, int TaskId)
        {
            var user = await dbContext.Users.FindAsync(UserId);
            var manager = await dbContext.Users.FindAsync(ManagerId);

            if (UserId == ManagerId )
            {
                return new Message { IsSuccess = false, ErrorMessage = "You Can't Assign your self." };
            }

            if (user == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not found." };
            }

            if (!user.IsActive)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not active, contact admin." };
            }

            if (manager == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "manager is not found." };
            }

            if (!manager.IsActive)
            {
                return new Message { IsSuccess = false, ErrorMessage = "manager is not active, contact admin." };
            }



            if (!await dbContext.Tasks.AnyAsync((e) => e.Id == TaskId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "task is not found." };
            }

            if (await dbContext.TaskUsers.AnyAsync((e) => e.TaskId == TaskId && e.UserId == UserId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "user already assigned to this task." };
            }

            var TaskUsers = new TaskUser { UserId = UserId, TaskId = TaskId };

            await dbContext.TaskUsers.AddAsync(TaskUsers);
            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "User Added to This Task Successfully." };

        }
        
        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {

            var usersList = await dbContext.Users.ToListAsync();

            List<UserResponseDto> users = new List<UserResponseDto>();

            users = usersList.Select((e) => new UserResponseDto()
            {
                Id = e.Id,
                CreatedDate = e.CreatedDate,
                Email = e.Email,
                IsActive = e.IsActive,
                Username = e.Username
            }).ToList();

            return users;

        }
        public async Task<UserResponseDto> GetUserByIdAsync(int Id)
        {
            if (!await dbContext.Users.AnyAsync((e) => e.Id == Id))
            {
                return null;
            }

            var user = await dbContext.Users.FindAsync(Id);


            return new UserResponseDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            };
        }
        public async Task<UserResponseDto> GetUserByUsernameAsync(string Username)
        {
            if (!await dbContext.Users.AnyAsync(e => e.Username.ToLower() == Username.ToLower()))
            {
                return null;
            }

            var user = await dbContext.Users.FirstOrDefaultAsync((e) => e.Username.ToLower() == Username.ToLower());
            return new UserResponseDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            };
        }
        public async Task<UserResponseDto> UpdateUserAsync(UserRegistrationDto userDto, int id)
        {
            // Check if user with the given Id exists in the database
            var user = await dbContext.Users.FindAsync(id);

            if (user == null)
            {
                throw new Exception($"Can't find the user by id={id}");
            }

            // Check if the username is unique, it shouldn't belong to any other user
            var isUsedUsername = await dbContext.Users.AnyAsync(e => e.Username.ToLower() == userDto.Username.ToLower() && e.Id != id);

            if (isUsedUsername)
            {
                throw new Exception("Username is already used by another person.");
            }

            // Update user information
            user.Username = userDto.Username;
            user.Email = userDto.Email;

            // Hash the password if it's being updated
            if (!string.IsNullOrEmpty(userDto.Password)) // Ensure that password is provided for update
            {
                var passwordHasher = new PasswordHasher<Models.User>();
                user.PasswordHash = passwordHasher.HashPassword(user, userDto.Password);
            }

            // Save the changes to the database
            await dbContext.SaveChangesAsync();

            // Return the updated user details as a response DTO
            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                IsActive = user.IsActive
            };
        }
    }
}
