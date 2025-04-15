using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Management_System.DTOs;
using Project_Management_System.DTOs.Messages;
using Project_Management_System.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]





    [Authorize(Roles = $"{nameof(enums.enRoles.ProjectManager)}, {nameof(enums.enRoles.Admin)}")]
    public class ProjectController(IProject projectService) : ControllerBase
    {
        // Create a new project
        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectRequestDto projectDto)
        {
            // 

            //var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await projectService.CreateProjectAsync(projectDto);
            if (!result.IsSuccess) return BadRequest(result);
            return Ok(result);
        }

        // Get all projects
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProjectRequestDto>>> GetAllProjects()
        {
            var projects = await projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        // Get a project by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectRequestDto>> GetProjectById(int id)
        {
            var project = await projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound(new Message { IsSuccess = false, ErrorMessage = "Project not found." });
            return Ok(project);
        }

        // Update a project
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectRequestDto projectDto)
        {
            var result = await projectService.UpdateProjectAsync(id, projectDto);


            if (result == null) return NotFound(result);
            return Ok(result);
        }

        // Delete a project
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var result = await projectService.DeleteProjectAsync(id);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }




        //[HttpPost("Assign-User-To-Project/{id}")]
        //public async Task<IActionResult> AssignUserToProject(int id, int EmployeeToAssignId)
        //{
        //    var ProjectManagerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


        //    return Ok();
        //}

        //[HttpPost("Apply-Project/{id}")]
        //public async Task<IActionResult> ApplyToProject(int id) { return Ok(); }














    }
}
