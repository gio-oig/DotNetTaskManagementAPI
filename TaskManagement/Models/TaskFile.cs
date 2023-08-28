using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Models
{
    public class TaskFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string URL { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }
        public TaskModel Task { get; set; }
    }
}
