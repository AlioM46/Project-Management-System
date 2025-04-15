using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.Interfaces;

namespace Project_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ApplicationsController(IApplication applicationService) : ControllerBase
    {
        // GET: api/Applications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetAll()
        {
            var result = await applicationService.GetAllApplicationsAsync();
            return Ok(result);
        }

        // GET: api/Applications/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationDto>> GetById(int id)
        {
            var result = await applicationService.GetApplicationByIdAsync(id);
            if (result == null)
                return NotFound("Application not found.");
            return Ok(result);
        }

        // POST: api/Applications
        [HttpPost]
        public async Task<ActionResult<Message>> Create([FromBody] ApplicationDto applicationDto)
        {
            // Assuming userId is passed somehow (e.g., via token or manually)
            int userId = 1; // Replace this with your actual logic to get the current user ID

            var result = await applicationService.CreateApplicationAsync(applicationDto, userId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }

        // PUT: api/Applications/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Message>> Update(int id, [FromBody] ApplicationDto applicationDto)
        {
            var result = await applicationService.UpdateApplicationAsync(id, applicationDto);
            if (result == null)
                return NotFound(new Message { IsSuccess = false, ErrorMessage = "Application not found." });

            return Ok(result);
        }

        // DELETE: api/Applications/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> Delete(int id)
        {
            var result = await applicationService.DeleteApplicationAsync(id);
            if (!result.IsSuccess)
                return NotFound(result);
            return Ok(result);
        }
    }
}
