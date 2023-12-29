using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Vue.Models;

namespace workflow.Areas.V1.Models
{
    [Table("taskEvent")]
    public class EventModel
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string event_content { get; set; }


        public int taskId { get; set; }

        [ForeignKey("taskId")]
        public virtual TaskModel? task { get; set; }

        public int? type { get; set; }
        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public DateTime? deleted_at { get; set; }


    }
}
