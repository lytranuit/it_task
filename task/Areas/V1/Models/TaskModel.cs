using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("task")]
    public class TaskModel
    {
        [Key]
        public int id { get; set; }
        public string? created_by { get; set; }
        public int? statusId { get; set; }

        [ForeignKey("created_by")]
        public virtual UserModel? user_created { get; set; }
        [ForeignKey("statusId")]
        public virtual TaskStatusModel? status { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public int? parentId { get; set; }
        public string? priority { get; set; }
        public int? projectId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }



        public DateTime? created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
