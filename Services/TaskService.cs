using Microsoft.EntityFrameworkCore;
using Project_Management_System.Data;
using Project_Management_System.DTOs.TaskDtos;
using Project_Management_System.Interfaces;
using Project_Management_System.Models;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.Models.ManyRelations;

namespace Project_Management_System.Services
{
    public class TaskService(AppDbContext dbContext) : ITask
    {
        public async Task<Message> CreateTaskAsync(TaskRequestDto taskDto)
        {
            if (string.IsNullOrEmpty(taskDto.Title))
                return new Message { IsSuccess = false, ErrorMessage = "Task title cannot be empty." };

            // Validate foreign keys (ProjectId and UserId)
            var projectExists = await dbContext.Projects.AnyAsync(p => p.Id == taskDto.ProjectId);

            if (!projectExists)
                return new Message { IsSuccess = false, ErrorMessage = "Invalid Project ID." };

            var userExists = await dbContext.Users.AnyAsync(u => u.Id == taskDto.CreatedByUserId);
            if (!userExists)
                return new Message { IsSuccess = false, ErrorMessage = "Invalid User ID." };

            if (taskDto.AssignedToUserId != null)
            {
                var assignedUser = await dbContext.Users.AnyAsync(u => u.Id == taskDto.AssignedToUserId);
                if (!assignedUser)
                    return new Message { IsSuccess = false, ErrorMessage = "Invalid Assigned User ID." };

            }


            var newTask = new Models.Task
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Priority = taskDto.Priority,
                Status = taskDto.Status,
                CreateDate = DateTime.Now,
                StartDate = taskDto.StartDate,
                EndDate = taskDto.EndDate,
                ProjectId = taskDto.ProjectId,
                CreatedByUserId = taskDto.CreatedByUserId,
            };




            await dbContext.Tasks.AddAsync(newTask);
            await dbContext.SaveChangesAsync();


            if (taskDto.AssignedToUserId.HasValue)
            {
                var taskUser = new Models.ManyRelations.TaskUser
                {
                    TaskId = newTask.Id,  // Link the Task to this User
                    UserId = taskDto.AssignedToUserId.Value  // Use .Value since AssignedToUserId is not null
                };

                await dbContext.TaskUsers.AddAsync(taskUser);
                await dbContext.SaveChangesAsync();
            }

            return new Message { IsSuccess = true, SuccessMessage = "Task created successfully." };
        }


        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            return await dbContext.Tasks
                .Select(taskDto => new TaskDto
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description,
                    Priority = taskDto.Priority,
                    Status = taskDto.Status,
                    CreateDate = DateTime.Now,
                    StartDate = taskDto.StartDate,
                    EndDate = taskDto.EndDate,
                    ProjectId = taskDto.ProjectId,
                    CreatedByUserId = taskDto.CreatedByUserId,
                })
                .ToListAsync();
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int taskId)
        {
            var task = await dbContext.Tasks.FindAsync(taskId);
            if (task == null) return null;

            return new TaskDto
            {
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                CreateDate = DateTime.Now,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                ProjectId = task.ProjectId,
                CreatedByUserId = task.CreatedByUserId,
            };
        }

        public async Task<Message> UpdateTaskAsync(int taskId, TaskRequestDto taskDto)
        {
            var task = await dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                return new Message { IsSuccess = false, ErrorMessage = "Task not found." };

            if (string.IsNullOrEmpty(taskDto.Title))
                return new Message { IsSuccess = false, ErrorMessage = "Task title cannot be empty." };

            // Validate foreign keys (ProjectId and UserId)
            var projectExists = await dbContext.Projects.AnyAsync(p => p.Id == taskDto.ProjectId);
            if (!projectExists)
                return new Message { IsSuccess = false, ErrorMessage = "Invalid Project ID." };

            //if (taskDto.AssignedToUserId != null)
            //{
            //    var assignedUser = await dbContext.Users.AnyAsync(u => u.Id == taskDto.AssignedToUserId);
            //    if (!assignedUser)
            //        return new Message { IsSuccess = false, ErrorMessage = "Invalid Assigned User ID." };
            //}


            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.Priority = taskDto.Priority;
            task.Status = taskDto.Status;
            task.StartDate = taskDto.StartDate;
            task.EndDate = taskDto.EndDate;
            


            await dbContext.SaveChangesAsync();
            return new Message { IsSuccess = true, SuccessMessage = "Task updated successfully." };
        }

        public async Task<Message> DeleteTaskAsync(int taskId)
        {
            var task = await dbContext.Tasks.FindAsync(taskId);
            if (task == null)
                return new Message { IsSuccess = false, ErrorMessage = "Task not found." };

            dbContext.Tasks.Remove(task);
            await dbContext.SaveChangesAsync();
            return new Message { IsSuccess = true, SuccessMessage = "Task deleted successfully." };
        }
    }
}
