

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using Spire.Xls;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Text.Json;
using Vue.Data;
using Vue.Models;
using workflow.Areas.V1.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace it_template.Areas.V1.Controllers
{

	public class TaskController : BaseController
	{
		private readonly IConfiguration _configuration;
		private UserManager<UserModel> UserManager;
		public TaskController(ItContext context, IConfiguration configuration, UserManager<UserModel> UserMgr) : base(context)
		{
			_configuration = configuration;
			UserManager = UserMgr;
		}

		[HttpPost]
		public async Task<JsonResult> Delete(int id)
		{
			var Task = _context.TaskModel.Where(d => d.id == id).FirstOrDefault();
			Task.deleted_at = DateTime.Now;
			_context.Update(Task);
			_context.SaveChanges();
			return Json(Task);
		}
		[HttpPost]
		public async Task<JsonResult> Save(TaskModel TaskModel, List<string>? list_assignee)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			var user_id = UserManager.GetUserId(currentUser);
			var user = await UserManager.GetUserAsync(currentUser);
			TaskModel? TaskModel_old;
			if (TaskModel.id == 0)
			{
				TaskModel.created_at = DateTime.Now;
				TaskModel.created_by = user_id;

				TaskModel.startDate = TaskModel.startDate != null && TaskModel.startDate.Value.Kind == DateTimeKind.Utc ? TaskModel.startDate.Value.ToLocalTime() : TaskModel.startDate;
				TaskModel.endDate = TaskModel.endDate != null && TaskModel.endDate.Value.Kind == DateTimeKind.Utc ? TaskModel.endDate.Value.ToLocalTime() : TaskModel.endDate;
				TaskModel.baselineStartDate = TaskModel.baselineStartDate != null && TaskModel.baselineStartDate.Value.Kind == DateTimeKind.Utc ? TaskModel.baselineStartDate.Value.ToLocalTime() : TaskModel.baselineStartDate;
				TaskModel.baselineEndDate = TaskModel.baselineEndDate != null && TaskModel.baselineEndDate.Value.Kind == DateTimeKind.Utc ? TaskModel.baselineEndDate.Value.ToLocalTime() : TaskModel.baselineEndDate;

				TaskModel.startDate = Truncate(TaskModel.startDate, DateTimeResolution.Hour);

				TaskModel.endDate = Truncate(TaskModel.endDate, DateTimeResolution.Hour);

				TaskModel.baselineStartDate = Truncate(TaskModel.baselineStartDate, DateTimeResolution.Hour);

				TaskModel.baselineEndDate = Truncate(TaskModel.baselineEndDate, DateTimeResolution.Hour);

				TaskModel.rank = getRank(TaskModel);
				_context.TaskModel.Add(TaskModel);

				_context.SaveChanges();

				TaskModel_old = TaskModel;

				EventModel EventModel = new EventModel
				{
					taskId = TaskModel_old.id,
					event_content = "<b>" + user.FullName + "</b> tạo mới",
					created_at = DateTime.Now,
				};
				_context.Add(EventModel);
				_context.SaveChanges();

			}
			else
			{
				var event_content = "<div><b>" + user.FullName + "</b> đã chỉnh sửa.<div>";
				event_content += "<ul>";
				TaskModel_old = _context.TaskModel.Where(d => d.id == TaskModel.id).FirstOrDefault();
				TaskModel_old.updated_at = DateTime.Now;
				////Field Allow edit
				if (TaskModel_old.name != TaskModel.name)
				{
					event_content += $"<li>Tên:<b>{TaskModel_old.name}</b> -> <b>{TaskModel.name}</b></li>";
					TaskModel_old.name = TaskModel.name;
				}

				if (TaskModel_old.statusId != TaskModel.statusId)
				{
					event_content += $"<li>Trạng thái:<b>{TaskModel_old.statusId}</b> -> <b>{TaskModel.statusId}</b></li>";
					TaskModel_old.statusId = TaskModel.statusId;
				}

				if (TaskModel_old.description != TaskModel.description)
				{
					event_content += $"<li>Mô tả:<b>{TaskModel_old.description}</b> -> <b>{TaskModel.description}</b></li>";
					TaskModel_old.description = TaskModel.description;
				}

				if (TaskModel_old.priority != TaskModel.priority)
				{
					event_content += $"<li>Độ ưu tiên:<b>{TaskModel_old.priority}</b> -> <b>{TaskModel.priority}</b></li>";
					TaskModel_old.priority = TaskModel.priority;
				}

				if (TaskModel_old.point != TaskModel.point)
				{
					event_content += $"<li>Chấm điểm:<b>{TaskModel_old.point}</b> -> <b>{TaskModel.point}</b></li>";
					TaskModel_old.point = TaskModel.point;
				}
				if (TaskModel_old.progress != TaskModel.progress && TaskModel.progress == 100)
				{
					TaskModel_old.finished_at = DateTime.Now;
				}

				if (TaskModel_old.progress != TaskModel.progress)
				{
					event_content += $"<li>Tình trạng:<b>{TaskModel_old.progress}</b> -> <b>{TaskModel.progress}</b></li>";
					TaskModel_old.progress = TaskModel.progress;
				}

				TaskModel.startDate = TaskModel.startDate != null && TaskModel.startDate.Value.Kind == DateTimeKind.Utc ? TaskModel.startDate.Value.ToLocalTime() : TaskModel.startDate;
				TaskModel.endDate = TaskModel.endDate != null && TaskModel.endDate.Value.Kind == DateTimeKind.Utc ? TaskModel.endDate.Value.ToLocalTime() : TaskModel.endDate;
				TaskModel.baselineStartDate = TaskModel.baselineStartDate != null && TaskModel.baselineStartDate.Value.Kind == DateTimeKind.Utc ? TaskModel.baselineStartDate.Value.ToLocalTime() : TaskModel.baselineStartDate;
				TaskModel.baselineEndDate = TaskModel.baselineEndDate != null && TaskModel.baselineEndDate.Value.Kind == DateTimeKind.Utc ? TaskModel.baselineEndDate.Value.ToLocalTime() : TaskModel.baselineEndDate;


				TaskModel.startDate = Truncate(TaskModel.startDate, DateTimeResolution.Hour);

				TaskModel.endDate = Truncate(TaskModel.endDate, DateTimeResolution.Hour);

				TaskModel.baselineStartDate = Truncate(TaskModel.baselineStartDate, DateTimeResolution.Hour);

				TaskModel.baselineEndDate = Truncate(TaskModel.baselineEndDate, DateTimeResolution.Hour);
				if (TaskModel_old.startDate != TaskModel.startDate)
				{
					event_content += $"<li>Ngày bắt đầu:<b>{TaskModel_old.startDate}</b> -> <b>{TaskModel.startDate}</b></li>";
					TaskModel_old.startDate = TaskModel.startDate;
				}
				if (TaskModel_old.endDate != TaskModel.endDate)
				{
					event_content += $"<li>Ngày kết thúc:<b>{TaskModel_old.endDate}</b> -> <b>{TaskModel.endDate}</b></li>";
					TaskModel_old.endDate = TaskModel.endDate;
				}
				if (TaskModel_old.baselineStartDate != TaskModel.baselineStartDate)
				{
					event_content += $"<li>Ngày kế hoạch bắt đầu:<b>{TaskModel_old.baselineStartDate}</b> -> <b>{TaskModel.baselineStartDate}</b></li>";
					TaskModel_old.baselineStartDate = TaskModel.baselineStartDate;
				}
				if (TaskModel_old.baselineEndDate != TaskModel.baselineEndDate)
				{
					event_content += $"<li>Ngày kế hoạch kết thúc:<b>{TaskModel_old.baselineEndDate}</b> -> <b>{TaskModel.baselineEndDate}</b></li>";
					TaskModel_old.baselineEndDate = TaskModel.baselineEndDate;
				}
				event_content += "</ul>";
				_context.Update(TaskModel_old);

				EventModel EventModel = new EventModel
				{
					taskId = TaskModel_old.id,
					event_content = event_content,
					created_at = DateTime.Now,
				};
				_context.Add(EventModel);
				_context.SaveChanges();



			}
			if (list_assignee != null)
			{
				var list_old = _context.TaskAssigneeModel.Where(d => d.taskId == TaskModel_old.id).ToList();
				_context.RemoveRange(list_old);

				var assignees = new List<TaskAssigneeModel>();
				foreach (var assignee in list_assignee)
				{
					assignees.Add(new TaskAssigneeModel
					{
						userId = assignee,
						taskId = TaskModel_old.id
					});
				}
				_context.AddRange(assignees);
				_context.SaveChanges();

			}
			TaskModel_old = _context.TaskModel.Where(d => d.id == TaskModel_old.id)
				.Include(d => d.status)
				.Include(d => d.project)
				.Include(d => d.assignees)
				.ThenInclude(d => d.user)
				.Include(d => d.favorites.Where(d => d.userId == user_id)).FirstOrDefault();
			return Json(TaskModel_old);
		}
		public async Task<JsonResult> GetList(int projectId)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			var user_id = UserManager.GetUserId(currentUser);
			var Tasks = _context.TaskModel.Where(d => d.projectId == projectId && d.deleted_at == null)
				.Include(d => d.attachments)
				.Include(d => d.status)
				.Include(d => d.project)
				.Include(d => d.assignees)
				.ThenInclude(d => d.user)
				.Include(d => d.favorites.Where(d => d.userId == user_id))
				.OrderBy(d => d.rank).ToList();
			return Json(Tasks);
		}

		[HttpPost]
		public async Task<JsonResult> GetListbyFilter(string type, string? type_2)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			var user_id = UserManager.GetUserId(currentUser);
			var tmp = _context.TaskModel.Where(d => d.deleted_at == null);
			if (type == "mytask")
			{
				var taskAssign = _context.TaskAssigneeModel.Where(d => d.userId == user_id).Select(d => d.taskId).ToList();
				tmp = tmp.Where(d => taskAssign.Contains(d.id));
			}
			else if (type == "myassign")
			{
				tmp = tmp.Where(d => d.created_by == user_id);
			}
			else
			{
				return Json(new { });
			}
			////
			if (type_2 == "chuahoanthanh")
			{
				tmp = tmp.Where(d => d.finished_at == null && d.startDate <= DateTime.Now && d.endDate >= DateTime.Now);
			}
			else if (type_2 == "quahan")
			{
				tmp = tmp.Where(d => d.finished_at == null && d.endDate < DateTime.Now);
			}
			var Tasks = tmp
				.Include(d => d.attachments)
				.Include(d => d.status)
				.Include(d => d.project)
				.Include(d => d.assignees)
				.ThenInclude(d => d.user)
				.Include(d => d.favorites.Where(d => d.userId == user_id))
				.OrderByDescending(d => d.startDate).ToList();
			return Json(Tasks);
		}

		public async Task<JsonResult> Get(int id)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			var user_id = UserManager.GetUserId(currentUser);
			var Task = _context.TaskModel.Where(d => d.id == id && d.deleted_at == null)
				.Include(d => d.status)
				.Include(d => d.project)
				.Include(d => d.assignees)
				.ThenInclude(d => d.user)
				.Include(d => d.attachments)
				.Include(d => d.favorites.Where(d => d.userId == user_id)).FirstOrDefault();

			return Json(Task);
		}

		// GET: Admin/Document/addComment
		[HttpPost]
		public async Task<IActionResult> AddComment(TaskCommentModel TaskCommentModel)
		{

			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			string user_id = UserManager.GetUserId(currentUser); // Get user id:
			var user = await UserManager.GetUserAsync(currentUser); // Get user id:
			TaskCommentModel.user_id = user_id;
			TaskCommentModel.created_at = DateTime.Now;
			_context.Add(TaskCommentModel);
			_context.SaveChanges();
			var files = Request.Form.Files;

			var items_comment = new List<TaskCommentFileModel>();
			if (files != null && files.Count > 0)
			{

				foreach (var file in files)
				{
					var timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
					string name = file.FileName;
					string ext = Path.GetExtension(name);
					string mimeType = file.ContentType;

					//var fileName = Path.GetFileName(name);
					var newName = timeStamp + " - " + name;

					newName = newName.Replace("+", "_");
					newName = newName.Replace("%", "_");
					var dir = _configuration["Source:Path_Private"] + "\\task\\" + TaskCommentModel.taskId;
					bool exists = Directory.Exists(dir);

					if (!exists)
						Directory.CreateDirectory(dir);


					var filePath = dir + "\\" + newName;

					string url = "/private/task/" + TaskCommentModel.taskId + "/" + newName;
					items_comment.Add(new TaskCommentFileModel
					{
						ext = ext,
						url = url,
						name = name,
						mimeType = mimeType,
						taskCommentId = TaskCommentModel.id,
						created_at = DateTime.Now
					});

					using (var fileSrteam = new FileStream(filePath, FileMode.Create))
					{
						await file.CopyToAsync(fileSrteam);
					}
				}
				_context.AddRange(items_comment);
				_context.SaveChanges();
			}
			TaskCommentModel = _context.TaskCommentModel.Where(d => d.id == TaskCommentModel.id).Include(d => d.files).Include(d => d.user).FirstOrDefault();
			////
			///
			//DocumentUserReadModel user_read = _context.DocumentUserReadModel.Where(d => d.document_id == DocumentCommentModel.document_id && d.user_id == DocumentCommentModel.user_id).FirstOrDefault();
			//if (user_read == null)
			//{
			//    user_read = new DocumentUserReadModel
			//    {
			//        document_id = DocumentCommentModel.document_id,
			//        user_id = DocumentCommentModel.user_id,
			//        time_read = DateTime.Now,
			//    };
			//    _context.Add(user_read);
			//}
			//else
			//{
			//    user_read.time_read = DateTime.Now;
			//    _context.Update(user_read);
			//}

			///create unread

			//var DocumentModel = _context.DocumentModel
			//            .Where(d => d.id == DocumentCommentModel.document_id)
			//            .Include(d => d.users_follow)
			//            .Include(d => d.users_signature)
			//            .Include(d => d.users_receive)
			//            .FirstOrDefault();

			//var users_follow = DocumentModel.users_follow.Select(a => a.user_id).ToList();
			//var users_signature = DocumentModel.users_signature.Select(a => a.user_id).ToList();
			//var users_representative = DocumentModel.users_signature.Where(a => a.representative_id != null).Select(a => a.representative_id).ToList();
			//var users_receive = DocumentModel.users_receive.Select(a => a.user_id).ToList();
			//List<string> users_related = new List<string>();
			//users_related.AddRange(users_follow);
			//users_related.AddRange(users_signature);
			//users_related.AddRange(users_representative);
			//users_related.AddRange(users_receive);
			//users_related = users_related.Distinct().ToList();
			//var itemToRemove = users_related.SingleOrDefault(r => r == user_id);
			//users_related.Remove(itemToRemove);
			//var items = new List<DocumentUserUnreadModel>();
			//foreach (string u in users_related)
			//{
			//    items.Add(new DocumentUserUnreadModel
			//    {
			//        user_id = u,
			//        document_id = DocumentModel.id,
			//        time = DateTime.Now,
			//    });
			//}
			//_context.AddRange(items);
			////SEND MAIL
			//if (users_related != null)
			//{
			//    var users_related_obj = _context.UserModel.Where(d => users_related.Contains(d.Id)).Select(d => d.Email).ToList();
			//    var mail_string = string.Join(",", users_related_obj.ToArray());
			//    string Domain = (HttpContext.Request.IsHttps ? "https://" : "http://") + HttpContext.Request.Host.Value;
			//    var attach = items_comment.Select(d => d.url).ToList();
			//    var text = DocumentCommentModel.comment;
			//    if (attach.Count() > 0 && DocumentCommentModel.comment == null)
			//    {
			//        text = $"{user.FullName} gửi đính kèm";
			//    }
			//    var body = _view.Render("Emails/NewComment",
			//        new
			//        {
			//            link_logo = Domain + "/images/clientlogo_astahealthcare.com_f1800.png",
			//            link = Domain + "/admin/document/details/" + DocumentModel.id,
			//            text = text,
			//            name = user.FullName
			//        });


			//    var email = new EmailModel
			//    {
			//        email_to = mail_string,
			//        subject = "[Tin nhắn mới] " + DocumentModel.name_vi,
			//        body = body,
			//        email_type = "new_comment_document",
			//        status = 1,
			//        data_attachments = attach

			//    };
			//    _context.Add(email);
			//}
			////await _context.SaveChangesAsync();

			///// Audittrail
			//var audit = new AuditTrailsModel();
			//audit.UserId = user.Id;
			//audit.Type = AuditType.Update.ToString();
			//audit.DateTime = DateTime.Now;
			//audit.description = $"Tài khoản {user.FullName} đã thêm bình luận.";
			//_context.Add(audit);
			//await _context.SaveChangesAsync();

			//DocumentCommentModel.user = await UserManager.GetUserAsync(currentUser);
			//DocumentCommentModel.is_read = true;

			return Json(new
			{
				success = 1,
				comment = TaskCommentModel
			});
		}
		[HttpPost]
		public async Task<IActionResult> Favorite(string type, int taskId)
		{

			System.Security.Claims.ClaimsPrincipal currentUser = this.User;
			string user_id = UserManager.GetUserId(currentUser); // Get user id:
																 //var user = await UserManager.GetUserAsync(currentUser); // Get user id:
			if (type == "add")
			{
				var taskFavorite = new TaskFavoriteModel()
				{
					userId = user_id,
					taskId = taskId
				};
				_context.Add(taskFavorite);
				_context.SaveChanges();
			}
			else
			{
				var taskFavorite = _context.TaskFavoriteModel.Where(d => d.taskId == taskId).FirstOrDefault();
				if (taskFavorite != null)
				{
					_context.Remove(taskFavorite);
					_context.SaveChanges();
				}
			}
			var Task = _context.TaskModel.Where(d => d.id == taskId && d.deleted_at == null)
				.Include(d => d.status)
				.Include(d => d.project)
				.Include(d => d.assignees).ThenInclude(d => d.user)
				.Include(d => d.favorites.Where(d => d.userId == user_id)).FirstOrDefault();

			return Json(new
			{
				success = 1,
				data = Task
			});
		}

		public async Task<IActionResult> MoreComment(int taskId, int? from_id)
		{
			int limit = 10;
			var comments_ctx = _context.TaskCommentModel
				.Where(d => d.taskId == taskId);
			if (from_id != null)
			{
				comments_ctx = comments_ctx.Where(d => d.id < from_id);
			}
			List<TaskCommentModel> comments = comments_ctx.OrderByDescending(d => d.id)
				.Take(limit).Include(d => d.files).Include(d => d.user).ToList();
			//System.Security.Claims.ClaimsPrincipal currentUser = User;
			//string current_user_id = UserManager.GetUserId(currentUser); // Get user id:
			//var user_read = _context.UserReadModel.Where(d => d.user_id == current_user_id && d.execution_id == execution_id).FirstOrDefatult();
			//DateTime? time_read = null;
			//if (user_read != null)
			//    time_read = user_read.time_read;

			//foreach (var comment in comments)
			//{
			//    if (comment.user_id == current_user_id)
			//    {
			//        comment.is_read = true;
			//        continue;
			//    }
			//    if (time_read != null && comment.created_at <= time_read)
			//        comment.is_read = true;
			//}
			return Json(new { success = 1, comments });
		}

		public async Task<IActionResult> getEvents(int taskId)
		{
			var events = _context.EventModel.Where(d => d.taskId == taskId).ToList();

			return Json(new { success = 1, events = events });
		}
		public async Task<JsonResult> Rank(int id, int issueId, string type)
		{
			var Task = _context.TaskModel.Where(d => d.id == id && d.deleted_at == null).FirstOrDefault();
			var issueTask = _context.TaskModel.Where(d => d.id == issueId && d.deleted_at == null).FirstOrDefault();
			if (type == "middleSegment")
			{
				Task.parentId = issueId;
				Task.rank = getRank(Task, null, type);
				_context.Update(Task);
				_context.SaveChanges();
			}
			else
			{
				Task.parentId = issueTask.parentId;
				Task.rank = getRank(Task, issueTask, type);
				_context.Update(Task);
				_context.SaveChanges();

			}
			return Json(Task);
		}
		private string getRank(TaskModel taskModel, TaskModel? issueTask = null, string type = "middleSegment")
		{
			var rank_return = taskModel.rank;
			var arr = new List<string>()
			{
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","t","u","v","w","x","y","z"
			};

			if (type == "middleSegment")
			{

				var parentId = taskModel.parentId;
				issueTask = _context.TaskModel.Where(d => d.parentId == parentId && d.id != taskModel.id).OrderBy(d => d.rank).LastOrDefault();
				if (issueTask != null && issueTask.rank != null)
				{
					var issue_rank = issueTask.rank;
					var number_issue_rank = convertInt(issue_rank, issue_rank.Length);
					var nextIndex = number_issue_rank + 1;
					rank_return = convertString(nextIndex, issue_rank.Length);
				}

			}
			else if (type == "bottomSegment")
			{
				var issue_rank = issueTask.rank;
				var issue_next = _context.TaskModel.Where(d => d.rank.CompareTo(issue_rank) > 0 && d.parentId == issueTask.parentId).OrderBy(d => d.rank).FirstOrDefault();
				if (issue_next == null || issue_next.rank == null) /// Không có next
				{
					string lastCharacter = issue_rank[issue_rank.Length - 1].ToString();
					string founderMinus1 = issue_rank.Remove(issue_rank.Length - 1, 1);
					int keyIndex = arr.FindIndex(w => w == lastCharacter); // 17,33
					if (lastCharacter == "z")
					{
						rank_return = founderMinus1 + "zi";
					}
					else
					{
						var amount_arr = arr.Count();
						var nextIndex = keyIndex + ((amount_arr - keyIndex) / 2 + 1);
						rank_return = founderMinus1 + arr[nextIndex];
					}
				}
				else
				{
					var issue_next_rank = issue_next.rank;

					if (issue_rank.Length > issue_next_rank.Length)
					{
						var len1 = issue_rank.Length;
						var len2 = issue_next_rank.Length;
						for (var i = 0; i < len1 - len2; i++)
						{
							issue_next_rank += "0";
						}
					}
					else if (issue_rank.Length < issue_next_rank.Length)
					{
						var len1 = issue_rank.Length;
						var len2 = issue_next_rank.Length;
						for (var i = 0; i < len2 - len1; i++)
						{
							issue_rank += "0";
						}
					}
					var number_issue_rank = convertInt(issue_rank, issue_rank.Length);
					var number_issue_next_rank = convertInt(issue_next_rank, issue_next_rank.Length);
					if (number_issue_next_rank - number_issue_rank <= 1)
					{
						issue_rank += "0";
						issue_next_rank += "0";
						number_issue_rank = convertInt(issue_rank, issue_rank.Length);
						number_issue_next_rank = convertInt(issue_next_rank, issue_next_rank.Length);
					}
					var nextIndex = number_issue_rank + ((number_issue_next_rank - number_issue_rank) / 2 + 1);
					rank_return = convertString(nextIndex, issue_rank.Length);
				}
				// x  y0i
			}
			else if (type == "topSegment")
			{
				var issue_rank = issueTask.rank;
				var issue_prev = _context.TaskModel.Where(d => d.rank.CompareTo(issue_rank) < 0 && d.parentId == issueTask.parentId).OrderBy(d => d.rank).LastOrDefault();
				if (issue_prev == null || issue_prev.rank == null) /// Không có next
				{
					var number_issue_rank = convertInt(issue_rank, issue_rank.Length);
					var prevIndex = number_issue_rank - 1;
					rank_return = convertString(prevIndex, issue_rank.Length);
				}
				else
				{
					var issue_prev_rank = issue_prev.rank;

					if (issue_rank.Length > issue_prev_rank.Length)
					{
						var len1 = issue_rank.Length;
						var len2 = issue_prev_rank.Length;
						for (var i = 0; i < len1 - len2; i++)
						{
							issue_prev_rank += "0";
						}
					}
					else if (issue_rank.Length < issue_prev_rank.Length)
					{
						var len1 = issue_rank.Length;
						var len2 = issue_prev_rank.Length;
						for (var i = 0; i < len2 - len1; i++)
						{
							issue_rank += "0";
						}
					}
					var number_issue_rank = convertInt(issue_rank, issue_rank.Length);
					var number_issue_prev_rank = convertInt(issue_prev_rank, issue_prev_rank.Length);
					if (number_issue_rank - number_issue_prev_rank <= 2)
					{
						issue_rank += "0";
						issue_prev_rank += "0";
						number_issue_rank = convertInt(issue_rank, issue_rank.Length);
						number_issue_prev_rank = convertInt(issue_prev_rank, issue_prev_rank.Length);
					}
					var prevIndex = number_issue_prev_rank + ((number_issue_rank - number_issue_prev_rank) / 2 + 1);
					rank_return = convertString(prevIndex, issue_rank.Length);
				}
				// x  y0i
			}
			return rank_return;
		}
		private int convertInt(string rank, int num)
		{
			var number = 0;
			var arr = new List<string>()
			{
				"0",
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","t","u","v","w","x","y","z"
			};
			for (var i = num; i > 0; i--)
			{
				//if (rank.Length - 1 > i)
				//    continue;
				int keyIndex = arr.FindIndex(w => w == rank[i - 1].ToString()); // 17,33
				number += keyIndex * (int)Math.Pow(arr.Count(), (num - i));
			}
			//foreach (char c in rank)
			//{
			//    int keyIndex = arr.FindIndex(w => w == c.ToString()); // 17,33

			//}

			return number;

		}
		private string convertString(int number, int num)
		{
			var rank = "";
			var arr = new List<string>()
			{
				"0",
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","t","u","v","w","x","y","z"
			};



			for (var i = num; i > 0; i--)
			{
				var keyIndex = number % arr.Count();
				string character = arr[keyIndex].ToString();
				rank = character + rank;
				number = number / arr.Count();
			}
			return rank;
		}
		public enum DateTimeResolution
		{
			Year, Month, Day, Hour, Minute, Second, Millisecond, Tick
		}

		private DateTime? Truncate(DateTime? d, DateTimeResolution resolution = DateTimeResolution.Second)
		{
			if (d == null)
				return null;
			var self = d.Value;
			switch (resolution)
			{
				case DateTimeResolution.Year:
					return new DateTime(self.Year, 1, 1, 0, 0, 0, 0, self.Kind);
				case DateTimeResolution.Month:
					return new DateTime(self.Year, self.Month, 1, 0, 0, 0, self.Kind);
				case DateTimeResolution.Day:
					return new DateTime(self.Year, self.Month, self.Day, 0, 0, 0, self.Kind);
				case DateTimeResolution.Hour:
					return self.AddTicks(-(self.Ticks % TimeSpan.TicksPerHour));
				case DateTimeResolution.Minute:
					return self.AddTicks(-(self.Ticks % TimeSpan.TicksPerMinute));
				case DateTimeResolution.Second:
					return self.AddTicks(-(self.Ticks % TimeSpan.TicksPerSecond));
				case DateTimeResolution.Millisecond:
					return self.AddTicks(-(self.Ticks % TimeSpan.TicksPerMillisecond));
				case DateTimeResolution.Tick:
					return self.AddTicks(0);
				default:
					throw new ArgumentException("unrecognized resolution", "resolution");
			}
		}
	}
}
