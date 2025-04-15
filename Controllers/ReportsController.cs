using Microsoft.AspNetCore.Mvc;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.DTOs.ReportDtos;
using Project_Management_System.Interfaces;

namespace Project_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController(IReport reportService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportDto report)
        {
            var result = await reportService.CreateReportAsync(report);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var report = await reportService.GetReportByIdAsync(id);
            if (report == null)
                return NotFound(new Message { IsSuccess = false, ErrorMessage = "Report not found." });

            return Ok(report);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] ReportRequestDto report)
        {
            var result = await reportService.UpdateReportAsync(id, report);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var result = await reportService.DeleteReportAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }
    }
}
