using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.DTO.Task
{
    public class UpdateTaskRequestDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedUserId { get; set; }

        public List<FileData>? Files { get; set; }
    }

    public class FileData
    {
        public int? Id { get; set; } = null;
        public IFormFile? File { get; set; }
    }
}
