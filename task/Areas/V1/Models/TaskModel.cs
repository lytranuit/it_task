using NuGet.Configuration;
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
		public virtual ProjectStatusModel? status { get; set; }


		public string name { get; set; }
		public string? description { get; set; }
		public int? parentId { get; set; }
		public string? priority { get; set; }
		public int? projectId { get; set; }

		[ForeignKey("projectId")]
		public virtual ProjectModel? project { get; set; }

		public int? point { get; set; }
		public string? rank { get; set; }
		public int? progress { get; set; } = 0;
		public DateTime? startDate { get; set; }
		public DateTime? endDate { get; set; }

		public DateTime? baselineStartDate { get; set; }
		public DateTime? baselineEndDate { get; set; }

		public DateTime? finished_at { get; set; }
		public virtual List<TaskAttachmentModel>? attachments { get; set; }
		public virtual List<TaskAssigneeModel>? assignees { get; set; }
		public virtual List<TaskFavoriteModel>? favorites { get; set; }

		public List<string>? list_assignee
		{
			get
			{
				//Console.WriteLine(settings);
				return assignees != null ? assignees.Select(d => d.userId).ToList() : new List<string>();
			}
		}
		public bool is_overdue
		{
			get
			{
				//Console.WriteLine(settings);
				return endDate != null ? endDate.Value < DateTime.Now : false;
			}
		}
		public DateTime? created_at { get; set; }
		public DateTime? deleted_at { get; set; }
		public DateTime? updated_at { get; set; }
	}

}
