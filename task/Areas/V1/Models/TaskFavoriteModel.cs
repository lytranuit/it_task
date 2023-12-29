using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Vue.Models
{
	[Table("taskFavorite")]
	public class TaskFavoriteModel
	{
		[Key]
		[Column(Order = 1)]
		public int taskId { get; set; }
		[Key]
		[Column(Order = 2)]
		public string userId { get; set; }


		[ForeignKey("userId")]
		public virtual UserModel user { get; set; }
		[ForeignKey("taskId")]
		public virtual TaskModel task { get; set; }
	}
}
