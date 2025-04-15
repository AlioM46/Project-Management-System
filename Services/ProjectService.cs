using Microsoft.EntityFrameworkCore;
using Project_Management_System.Data;
using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.Interfaces;
using Project_Management_System.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Management_System.Services
{
    public class ProjectService(AppDbContext dbContext) : IProject
    {
        // Create a new project
        public async Task<Message> CreateProjectAsync(ProjectRequestDto project)
        {
            if (string.IsNullOrEmpty(project.Title))
                return new Message { IsSuccess = false, ErrorMessage = "Fields cannot be empty." };

            if (!await dbContext.Users.AnyAsync((e) => e.Id == project.CreatedByUserId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "Invalid User ID" };

            }


            var newProject = new Project
            {
                Title = project.Title,
                Description = project.Description,
                CreatedDate = DateTime.UtcNow, 
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                UserId = project.CreatedByUserId,
                Status = project.Status
            };

            await dbContext.Projects.AddAsync(newProject);
            int rowsAffected = await dbContext.SaveChangesAsync();


            if (rowsAffected > 0)
            {

                return new Message { IsSuccess = true, SuccessMessage = "New project created successfully." };
            }
            else
            {
                return new Message { IsSuccess = false, ErrorMessage = "Failed To Create The Project." };

            }
        }

        // Get all projects
        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var projects = await dbContext.Projects.ToListAsync();
            return projects.Select(p => new ProjectResponseDto
            {
                Title = p.Title,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                CreatedDate = p.CreatedDate,
                CreatedByUserId = p.UserId,
                Status = p.Status
            });
        }

        // Get project by ID
        public async Task<ProjectResponseDto?> GetProjectByIdAsync(int projectId)
        {
            var project = await dbContext.Projects.FindAsync(projectId);
            if (project == null) return null;

            return new ProjectResponseDto
            {
                Title = project.Title,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreatedDate = project.CreatedDate,
                CreatedByUserId = project.UserId,
                Status = project.Status
            };
        }

        // Update a project
        public async Task<Message> UpdateProjectAsync(int projectId, ProjectRequestDto projectDto)
        {
            var project = await dbContext.Projects.FindAsync(projectId);
            if (project == null)
                return new Message { IsSuccess = false, ErrorMessage = "Project not found." };

            project.Title = projectDto.Title;
            project.Description = projectDto.Description;
            project.StartDate = projectDto.StartDate;
            project.EndDate = projectDto.EndDate;
            project.Status = projectDto.Status;


            await dbContext.SaveChangesAsync();
            return new Message { IsSuccess = true, SuccessMessage = "Project updated successfully." };
        }

        // Delete a project
        public async Task<Message> DeleteProjectAsync(int projectId)
        {
            var project = await dbContext.Projects.FindAsync(projectId);
            if (project == null)
                return new Message { IsSuccess = false, ErrorMessage = "Project not found." };

            dbContext.Projects.Remove(project);
            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "Project deleted successfully." };
        }
    }
}
