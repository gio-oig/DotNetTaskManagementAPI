using TaskManagement.Models;
using TaskManagement.Models.API;
using TaskManagement.Models.DTO.Task;

namespace TaskManagement.Services.Task
{
    public interface ITaskService
    {
        Task<ServiceResponse<List<TaskModel>>> GetAll();
        Task<ServiceResponse<TaskModel>> GetById(int taskId);
        Task<ServiceResponse<List<TaskModel>>> GetTasksForUser(string userId);
        Task<ServiceResponse<string>> Create(CreateTaskRequestDTO dto);
        Task<ServiceResponse<TaskModel>> Update(UpdateTaskRequestDTO dto);
        Task<ServiceResponse<string>> Remove(int TaskId);
    }
}
