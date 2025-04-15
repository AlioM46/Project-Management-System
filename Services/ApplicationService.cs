using Microsoft.EntityFrameworkCore;
using Project_Management_System.Data;
using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.Interfaces;
using Project_Management_System.Models;

namespace Project_Management_System.Services
{
    public class ApplicationService(AppDbContext dbContext) : IApplication
    {
        public async Task<Message> CreateApplicationAsync(ApplicationDto application, int userId)
        {
            if ((!application.ProjectId.HasValue && !application.TaskId.HasValue))
            {
                return new DTOs.Messages.Message
                {
                    IsSuccess = false,
                    ErrorMessage = "Both Task and Project Id's Is Null"
                };
            }





            var app = new Application
            {
                Status = application.Status,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            if (application.ProjectId.HasValue)
            {
                if (await dbContext.Projects.AnyAsync((e) => e.Id == application.ProjectId))
                {
                    app.ProjectId = application.ProjectId.Value;
                }
                else
                {
                    return new Message { IsSuccess = false, ErrorMessage = "Project Id Is Not Found" };
                }
            }
            if (application.TaskId.HasValue)
            {
                if (await dbContext.Tasks.AnyAsync((e) => e.Id == application.TaskId))
                {
                    app.TaskId = application.TaskId.Value;
                }
                else
                {
                    return new Message { IsSuccess = false, ErrorMessage = "Task Id Is Not Found" };
                }
            }


            await dbContext.AddAsync(app);
            await dbContext.SaveChangesAsync();





            return new Message
            {
                IsSuccess = true,
                SuccessMessage = "Application created successfully."
            };


        }


        public async Task<Message> DeleteApplicationAsync(int applicationId)
        {

            bool isExist = await dbContext.Applications.AnyAsync((e) => e.Id == applicationId);
            if (isExist)
            {
                var app = await dbContext.Applications.FirstOrDefaultAsync(e => e.Id == applicationId);

                dbContext.Applications.Remove(app);
                return new Message { IsSuccess = true, SuccessMessage = "Application Deleted Successfully." };
            }
            return new Message { IsSuccess = false, ErrorMessage = "Failed To Delete Application." };


        }

        public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync()
        {

            var AppList = await dbContext.Applications.ToListAsync();

            var DtosList = AppList.Select((e) => new ApplicationDto { UserId = e.UserId, TaskId = e.TaskId, ProjectId = e.ProjectId, CreatedAt = e.CreatedAt, Status = e.Status }).ToList();

            return DtosList;

        }

        public async Task<ApplicationDto> GetApplicationByIdAsync(int applicationId)
        {
            var app = await dbContext.Applications.FirstOrDefaultAsync(e => e.Id == applicationId);

            if (app == null)
                return null;

            return new ApplicationDto
            {
                UserId = app.UserId,
                CreatedAt = app.CreatedAt,
                Status = app.Status,
                TaskId = app.TaskId,
                ProjectId = app.ProjectId
            };


        }

        public async Task<Message> UpdateApplicationAsync(int applicationId, ApplicationDto application)
        {
            var app = await dbContext.Applications.FindAsync(applicationId);

            if (app == null)
                return null;


            if ((!application.TaskId.HasValue && !application.ProjectId.HasValue))
            {
                return new Message { IsSuccess = false, ErrorMessage = "Both Task and Project Id's is Null" };

            }

            if (await dbContext.Projects.AnyAsync((e) => e.Id == application.ProjectId))
            {
                    app.ProjectId = application.ProjectId;
            }
            else
            {
                return new Message { IsSuccess = false, ErrorMessage = "Project Id Is Not Found" };
            }

            if (await dbContext.Tasks.AnyAsync((e) => e.Id == application.TaskId))
            {
                    app.TaskId = application.TaskId;
            }
            else
            {
                return new Message { IsSuccess = false, ErrorMessage = "Task Id Is Not Found" };
            }

         
            app.Status = application.Status;

            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "Application Updated Successfully." };

        }

    }
}
