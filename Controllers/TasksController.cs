using Microsoft.AspNetCore.Mvc;
using Project_Management_System.DTOs.TaskDtos;
using Project_Management_System.Interfaces;
using Project_Management_System.DTOs.Messages;

namespace Project_Management_System.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController(ITask taskService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Message>> CreateTask( TaskRequestDto taskDto)
        {
            var response = await taskService.CreateTaskAsync(taskDto);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            var tasks = await taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            var task = await taskService.GetTaskByIdAsync(id);
            return task is not null ? Ok(task) : NotFound(new Message { IsSuccess = false, ErrorMessage = "Task not found." });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Message>> UpdateTask(int id,  TaskRequestDto taskDto)
        {
            var response = await taskService.UpdateTaskAsync(id, taskDto);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Message>> DeleteTask(int id)
        {
            var response = await taskService.DeleteTaskAsync(id);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
