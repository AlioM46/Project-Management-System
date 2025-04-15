using Project_Management_System.DTOs.Messages;
using Project_Management_System.DTOs.ReportDtos;
using Project_Management_System.Models;

namespace Project_Management_System.Interfaces
{
    public interface IReport
    {
        // Create a new report
        Task<Message> CreateReportAsync(ReportDto report);

        // Get all reports
        Task<IEnumerable<ReportDto>> GetAllReportsAsync();

        // Get a report by ID
        Task<ReportDto> GetReportByIdAsync(int reportId);

        // Update an existing report
        Task<Message> UpdateReportAsync(int reportId, ReportRequestDto report);

        // Delete a report
        Task<Message> DeleteReportAsync(int reportId);
    }
}
