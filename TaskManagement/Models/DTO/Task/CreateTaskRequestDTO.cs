using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models.DTO.Task
{
    public class CreateTaskRequestDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string AssignedUserId { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
