using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
    [Table("projectManager")]
    public class ProjectManagerModel
    {
        [Key]
        [Column(Order = 1)]
        public int projectId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string userId { get; set; }

        [ForeignKey("userId")]
        public virtual UserModel user { get; set; }
        [ForeignKey("projectId")]
        public virtual ProjectModel project { get; set; }
    }
}
