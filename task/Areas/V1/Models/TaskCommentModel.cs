using NuGet.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("taskComment")]
    public class TaskCommentModel
    {
        public int id { get; set; }
        public int taskId { get; set; }
        public string? comment { get; set; }


        [ForeignKey("taskId")]
        public virtual TaskModel? task { get; set; }


        public string user_id { get; set; }

        [ForeignKey("user_id")]
        public virtual UserModel? user { get; set; }
        public virtual List<TaskCommentFileModel>? files { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }
        [NotMapped]

        public bool is_read { get; set; } = false;
    }

}
