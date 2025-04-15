using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.Models;

namespace Project_Management_System.Interfaces
{
    public interface IProject
    {
        Task<Message> CreateProjectAsync(ProjectRequestDto project);

        // Get all projects
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync();

        // Get a project by ID
        Task<ProjectResponseDto> GetProjectByIdAsync(int projectId);

        // Update an existing project
        Task<Message> UpdateProjectAsync(int projectId, ProjectRequestDto project);

        // Delete a project
        Task<Message> DeleteProjectAsync(int projectId);
    }
}
