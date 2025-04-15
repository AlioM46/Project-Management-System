using Microsoft.EntityFrameworkCore;
using Project_Management_System.Data;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.DTOs.ReportDtos;
using Project_Management_System.Interfaces;
using Project_Management_System.Models;

namespace Project_Management_System.Services
{
    public class ReportService(AppDbContext dbContext) : IReport
    {
        public async Task<Message> CreateReportAsync(ReportDto report)
        {
            if (string.IsNullOrEmpty(report.Content))
            {
                return new Message { IsSuccess = false, ErrorMessage = "report content should not be empty." };
            }

            bool isUserExist = await dbContext.Users.AnyAsync((e) => e.Id == report.CreatedByUserId);

            if (!isUserExist)
            {
                return new Message { IsSuccess = false, ErrorMessage = "user is not found." };
            }

            bool isTaskExist = await dbContext.Tasks.AnyAsync((e) => e.Id == report.TaskId);
            if (!isTaskExist)
            {
                return new Message { IsSuccess = false, ErrorMessage = "Task is not found." };
            }

            var newReport = new Report { Content = report.Content, UserId = report.CreatedByUserId, TaskId = report.TaskId };

            await dbContext.Reports.AddAsync(newReport);
            await dbContext.SaveChangesAsync();
            return new Message { IsSuccess = true, SuccessMessage = "Report Added Successfully." };


        }

        public async Task<Message> DeleteReportAsync(int reportId)
        {
            var report = await dbContext.Reports.FindAsync(reportId);

            if (report == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "Report not found." };
            }

            dbContext.Reports.Remove(report);
            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "Report deleted successfully." };

        }

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
        {
            var reportsList = await dbContext.Reports.ToListAsync();

            var dtoList = reportsList.Select(r => new ReportDto
            {
                TaskId = r.TaskId,
                Content = r.Content,
                CreatedDate = r.CreatedDate,
                CreatedByUserId = r.UserId
            }).ToList();

            return dtoList;
        }

        public async Task<ReportDto> GetReportByIdAsync(int reportId)
        {
            var report = await dbContext.Reports.FindAsync(reportId);
            if (report == null)
            {
                return null;
            }
            return new ReportDto
            {
                Content = report.Content,
                CreatedByUserId = report.UserId,
                CreatedDate = report.CreatedDate,
                TaskId = report.TaskId
            };
        }

        public async Task<Message> UpdateReportAsync(int reportId, ReportRequestDto report)
        {
            var updatedReport = await dbContext.Reports.FindAsync(reportId);
            if (updatedReport == null)
            {
                return new Message { IsSuccess = false, ErrorMessage = "report is not found." };
            }

            if (!await dbContext.Tasks.AnyAsync((e) => e.Id == report.TaskId))
            {
                return new Message { IsSuccess = false, ErrorMessage = "Task is not found." };
            }
            updatedReport.Content = report.Content;
            updatedReport.TaskId = report.TaskId;

            await dbContext.SaveChangesAsync();

            return new Message { IsSuccess = true, SuccessMessage = "report updated successfully." };
        }
    }
}
