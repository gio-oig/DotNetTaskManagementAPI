using TaskManagement.Models;

namespace TaskManagement.Repository.IRepository
{
    public interface ITaskRepository : IRepository<TaskModel>
    {
        Task<TaskModel> UpdateAsync(TaskModel entity);
    }
}
