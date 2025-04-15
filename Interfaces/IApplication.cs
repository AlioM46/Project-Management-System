using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.Models;

namespace Project_Management_System.Interfaces
{
    public interface IApplication
    {
        // Create a new application
        Task<Message> CreateApplicationAsync(ApplicationDto application, int userId);

        // Get all applications
        Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync();

        // Get an application by ID
        Task<ApplicationDto> GetApplicationByIdAsync(int applicationId);

        // Update an existing application
        Task<Message> UpdateApplicationAsync(int applicationId, ApplicationDto application);

        // Delete an application
        Task<Message> DeleteApplicationAsync(int applicationId);
    }
}
