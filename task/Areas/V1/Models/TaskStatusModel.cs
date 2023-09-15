using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("taskStatus")]
    public class TaskStatusModel
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
