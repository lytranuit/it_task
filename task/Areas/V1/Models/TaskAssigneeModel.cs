using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("taskAssignee")]
    public class TaskAssigneeModel
    {
        [Key]
        [Column(Order = 1)]
        public int taskId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string userId { get; set; }
    }
}
