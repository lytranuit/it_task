using NuGet.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("taskCommentFile")]
    public class TaskCommentFileModel
    {
        public int id { get; set; }

        [StringLength(255)]
        public string name { get; set; }
        [StringLength(255)]
        public string url { get; set; }
        [StringLength(50)]
        public string ext { get; set; }
        [StringLength(255)]
        public string mimeType { get; set; }

        public int taskCommentId { get; set; }

        [ForeignKey("taskCommentId")]
        public virtual TaskCommentModel? comment { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? deleted_at { get; set; }

    }

}
