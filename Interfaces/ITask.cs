using Project_Management_System.DTOs.Messages;
using Project_Management_System.DTOs.TaskDtos;
using Project_Management_System.Models;

namespace Project_Management_System.Interfaces
{
    public interface ITask
    {
        // Create a new task
        Task<Message> CreateTaskAsync(TaskRequestDto task);

        // Get all tasks
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();

        // Get a task by ID
        Task<TaskDto> GetTaskByIdAsync(int taskId);

        // Update an existing task
        Task<Message> UpdateTaskAsync(int taskId, TaskRequestDto task);

        // Delete a task
        Task<Message> DeleteTaskAsync(int taskId);
    }
}
