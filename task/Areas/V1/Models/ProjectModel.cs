using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("project")]
    public class ProjectModel
    {
        [Key]
        public int id { get; set; }
        public string? created_by { get; set; }

        [ForeignKey("created_by")]
        public virtual UserModel? user_created { get; set; }

        public virtual List<ProjectAssigneeModel>? assignees { get; set; }
        public virtual List<ProjectManagerModel>? manager { get; set; }
        public string name { get; set; }
        public string? description { get; set; }



        public DateTime? created_at { get; set; }
        public DateTime? deleted_at { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
