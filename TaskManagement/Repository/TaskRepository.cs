using System.Linq.Expressions;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Repository.IRepository;

namespace TaskManagement.Repository
{
    public class TaskRepository : Repository<TaskModel>, ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<TaskModel> UpdateAsync(TaskModel entity)
        {
            _db.Tasks.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
