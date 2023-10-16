using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("projectStatus")]
    public class ProjectStatusModel
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }

        public int? projectId { get; set; }
        public string? rank { get; set; }
        public string? created_by { get; set; }
        public string? color { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
