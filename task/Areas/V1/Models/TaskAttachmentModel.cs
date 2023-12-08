using NuGet.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("taskAttachment")]
    public class TaskAttachmentModel
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string url { get; set; }
        [StringLength(50)]
        public string ext { get; set; }
        [StringLength(255)]
        public string mimeType { get; set; }
        public long size { get; set; }

        public int taskId { get; set; }

        [ForeignKey("taskId")]
        public virtual TaskModel? task { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }
    }

}
