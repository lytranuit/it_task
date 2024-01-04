

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spire.Xls;
using System.Collections;
using System.Data;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Vue.Data;
using Vue.Models;
using static it_template.Areas.V1.Controllers.UserController;

namespace it_template.Areas.V1.Controllers
{

    public class ApiController : BaseController
    {
        private readonly IConfiguration _configuration;
        private UserManager<UserModel> UserManager;
        public ApiController(ItContext context, IConfiguration configuration, UserManager<UserModel> UserMgr) : base(context)
        {
            _configuration = configuration;
            UserManager = UserMgr;
        }

        [HttpPost]
        public async Task<JsonResult> DeleteProject(int projectId)
        {
            var project = _context.ProjectModel.Where(d => d.id == projectId).FirstOrDefault();
            project.deleted_at = DateTime.Now;
            _context.Update(project);
            _context.SaveChanges();
            return Json(project);
        }
        [HttpPost]
        public async Task<JsonResult> SaveProject(ProjectModel ProjectModel, List<string> list_assignee)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user_id = UserManager.GetUserId(currentUser);
            if (ProjectModel.id == 0)
            {
                ProjectModel.created_at = DateTime.Now;
                ProjectModel.created_by = user_id;
                ProjectModel.assignees = null;
                _context.ProjectModel.Add(ProjectModel);
                _context.SaveChanges();
            }
            var assignees = new List<ProjectAssigneeModel>();
            foreach (var assignee in list_assignee)
            {
                assignees.Add(new ProjectAssigneeModel
                {
                    userId = assignee,
                    projectId = ProjectModel.id
                });
            }
            ProjectModel.assignees = assignees;
            _context.Update(ProjectModel);
            _context.SaveChanges();

            return Json(ProjectModel);
        }
        public async Task<JsonResult> GetListProject()
        {
            var projects = _context.ProjectModel.Where(d => d.deleted_at == null).ToList();
            return Json(projects);
        }
        public async Task<JsonResult> GetProject(int projectId)
        {
            var project = _context.ProjectModel.Where(d => d.id == projectId && d.deleted_at == null).FirstOrDefault();

            return Json(project);
        }

        public async Task<JsonResult> SaveTask()
        {
            return Json(new { });
        }
        public async Task<JsonResult> GetListTask(int projectId)
        {
            return Json(new { });
        }

        public async Task<JsonResult> assignTask()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string user_id = UserManager.GetUserId(currentUser); // Get user id:

            var task = _context.TaskModel.Where(d => d.created_by == user_id && d.deleted_at == null).ToList();


            var count = task.Count();
            var count_truoc_han = task.Where(d => d.finished_at != null && d.startDate > d.finished_at).Count();
            var count_dung_han = task.Where(d => d.finished_at != null && d.startDate <= d.finished_at && d.endDate >= d.finished_at).Count();
            var count_tre_han = task.Where(d => d.finished_at != null && d.endDate < d.finished_at).Count();
            var count_chua_ht = task.Where(d => d.finished_at == null && d.startDate <= DateTime.Now && d.endDate >= DateTime.Now).Count();
            var count_qua_han = task.Where(d => d.finished_at == null && d.endDate < DateTime.Now).Count();

            var data = new
            {
                labels = new List<string>() { "Hoàn thành trước hạn", "Hoàn thành đúng hạn", "Hoàn thành trễ hạn", "Chưa hoàn thành", "Quá hạn" },
                datasets = new List<Chart>() { new Chart { label = "Công việc", data = new List<int>() { count_truoc_han, count_dung_han, count_tre_han, count_chua_ht, count_qua_han } } }
            };

            return Json(new { count = count, data = data });
        }

        public async Task<JsonResult> myTask()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string user_id = UserManager.GetUserId(currentUser); // Get user id:

            var taskAssign = _context.TaskAssigneeModel.Where(d => d.userId == user_id).Select(d => d.taskId).ToList();
            var task = _context.TaskModel.Where(d => taskAssign.Contains(d.id) && d.deleted_at == null).ToList();


            var count = task.Count();
            var count_truoc_han = task.Where(d => d.finished_at != null && d.startDate > d.finished_at).Count();
            var count_dung_han = task.Where(d => d.finished_at != null && d.startDate <= d.finished_at && d.endDate >= d.finished_at).Count();
            var count_tre_han = task.Where(d => d.finished_at != null && d.endDate < d.finished_at).Count();
            var count_chua_ht = task.Where(d => d.finished_at == null && d.startDate <= DateTime.Now && d.endDate >= DateTime.Now).Count();
            var count_qua_han = task.Where(d => d.finished_at == null && d.endDate < DateTime.Now).Count();

            var data = new
            {
                labels = new List<string>() { "Hoàn thành trước hạn", "Hoàn thành đúng hạn", "Hoàn thành trễ hạn", "Chưa hoàn thành", "Quá hạn" },
                datasets = new List<Chart>() { new Chart { label="Công việc", data = new List<int>() { count_truoc_han, count_dung_han, count_tre_han, count_chua_ht, count_qua_han } }
    }
            };

            return Json(new { count = count, data = data });
        }
        public async Task<JsonResult> myPoint()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string user_id = UserManager.GetUserId(currentUser); // Get user id:

            var taskAssign = _context.TaskAssigneeModel.Where(d => d.userId == user_id).Select(d => d.taskId).ToList();
            var task = _context.TaskModel.Where(d => taskAssign.Contains(d.id) && d.deleted_at == null && d.point >= 0).ToList();


            var count = task.Count();
            var point = task.Sum(d => d.point);

            return Json(new { task = count, point = point });
        }


        [HttpPost]
        public async Task<JsonResult> topdiemnv(DateTime? tungay, DateTime? denngay, List<string>? nhanviens)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string user_id = UserManager.GetUserId(currentUser); // Get user id:
            var tmp = _context.TaskAssigneeModel.Include(d => d.user).Include(d => d.task).Where(d => d.task.deleted_at == null && d.task.finished_at != null);
            if (nhanviens != null && nhanviens.Count > 0)
            {
                tmp = tmp.Where(d => nhanviens.Contains(d.userId));
            }
            else
            {
                nhanviens = _context.UserManagerModel.Where(d => d.userId == user_id).Select(d => d.userManagerId).ToList();
                tmp = tmp.Where(d => nhanviens.Contains(d.userId));
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                tmp = tmp.Where(d => d.task.finished_at >= tungay.Value);
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                tmp = tmp.Where(d => d.task.finished_at <= denngay.Value.Date.AddDays(1).AddTicks(-1));
            }
            var dulieu = tmp.ToList();
            var group = dulieu.GroupBy(d => new { d.userId, d.user.FullName }).Select(d => new
            {
                label = d.Key.FullName,
                id = d.Key.userId,
                point = d.Sum(e => e.task.point).Value,
                list = d.Select(d => d.task.id).ToList(),
            }).OrderByDescending(d => d.point);
            var data = new { labels = group.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Điểm số", data = group.Select(d => d.point).ToList() } } };

            return Json(new
            {
                data = data
            });
        }
        [HttpPost]
        public async Task<JsonResult> congviectg(DateTime tungay, DateTime denngay, List<string>? nhanviens)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string user_id = UserManager.GetUserId(currentUser); // Get user id:
            var tmp = _context.TaskModel.Where(d => d.deleted_at == null);
            if (nhanviens != null && nhanviens.Count > 0)
            {
                var taskAssign = _context.TaskAssigneeModel.Where(d => nhanviens.Contains(d.userId)).Select(d => d.taskId).ToList();
                tmp = tmp.Where(d => taskAssign.Contains(d.id));
            }
            else
            {
                nhanviens = _context.UserManagerModel.Where(d => d.userId == user_id).Select(d => d.userManagerId).ToList();
                var taskAssign = _context.TaskAssigneeModel.Where(d => nhanviens.Contains(d.userId)).Select(d => d.taskId).ToList();
                tmp = tmp.Where(d => taskAssign.Contains(d.id));
            }
            var dulieu = tmp.ToList();
            if (tungay != null)
            {
                if (tungay.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.ToLocalTime();
                }
            }
            if (denngay != null)
            {
                if (denngay.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.ToLocalTime();
                }
            }
            var labels = new List<string>();
            var dataHoanthanh = new List<int>();
            var dataTong = new List<int>();
            for (var day = tungay.Date; day.Date <= denngay.Date; day = day.AddDays(1))
            {
                var task_count = dulieu.Where(d => day >= d.startDate && day <= d.endDate).Count();
                var finish = dulieu.Where(d => d.finished_at != null && d.finished_at.Value.Date == day.Date).Count();
                if (task_count > 0)
                {
                    labels.Add(day.ToString("yyyy-MM-dd"));
                    dataHoanthanh.Add(finish);
                    dataTong.Add(task_count);
                }
            }
            //var dulieu = tmp.ToList();
            var data = new { labels = labels, datasets = new List<Chart>() { new Chart { label = "Hoàn thành", data = dataHoanthanh, fill = true }, new Chart { label = "Tổng công việc", data = dataTong, fill = true } } };

            return Json(new
            {
                data = data
            });
        }
        [HttpPost]
        public async Task<JsonResult> tinhtrangcongviec(DateTime? tungay, DateTime? denngay, List<string>? nhanviens)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string user_id = UserManager.GetUserId(currentUser); // Get user id:
            var tmp = _context.TaskAssigneeModel.Include(d => d.user).Include(d => d.task).Where(d => d.task.deleted_at == null);
            if (nhanviens != null && nhanviens.Count > 0)
            {
                tmp = tmp.Where(d => nhanviens.Contains(d.userId));
            }
            else
            {
                nhanviens = _context.UserManagerModel.Where(d => d.userId == user_id).Select(d => d.userManagerId).ToList();
                tmp = tmp.Where(d => nhanviens.Contains(d.userId));
            }
            if (tungay != null && tungay.HasValue)
            {
                if (tungay.Value.Kind == DateTimeKind.Utc)
                {
                    tungay = tungay.Value.ToLocalTime();
                }
                tmp = tmp.Where(d => d.task.finished_at >= tungay.Value);
            }
            if (denngay != null && denngay.HasValue)
            {
                if (denngay.Value.Kind == DateTimeKind.Utc)
                {
                    denngay = denngay.Value.ToLocalTime();
                }

                tmp = tmp.Where(d => d.task.finished_at <= denngay.Value.Date.AddDays(1).AddTicks(-1));
            }
            //
            //var count_tre_han = task.Where(d => d.finished_at != null && d.endDate < d.finished_at).Count();
            //var count_chua_ht = task.Where(d => d.finished_at == null && d.startDate <= DateTime.Now && d.endDate >= DateTime.Now).Count();
            //var count_qua_han = task.Where(d => d.finished_at == null && d.endDate < DateTime.Now).Count();
            //"Hoàn thành trước hạn", "Hoàn thành đúng hạn", "Hoàn thành trễ hạn", "Chưa hoàn thành", "Quá hạn"
            var dulieu = tmp.ToList();
            var group = dulieu.GroupBy(d => new { d.userId, d.user.FullName }).Select(d => new
            {
                label = d.Key.FullName,
                count = d.Count(),
                truocHan = d.Where(d => d.task.finished_at != null && d.task.startDate > d.task.finished_at).Count(),
                list_truocHan = d.Where(d => d.task.finished_at != null && d.task.startDate > d.task.finished_at).Select(d => d.task.id).ToList(),

                dungHan = d.Where(d => d.task.finished_at != null && d.task.startDate <= d.task.finished_at && d.task.endDate >= d.task.finished_at).Count(),
                list_dungHan = d.Where(d => d.task.finished_at != null && d.task.startDate <= d.task.finished_at && d.task.endDate >= d.task.finished_at).Select(d => d.task.id).ToList(),

                treHan = d.Where(d => d.task.finished_at != null && d.task.endDate < d.task.finished_at).Count(),
                list_treHan = d.Where(d => d.task.finished_at != null && d.task.endDate < d.task.finished_at).Select(d => d.task.id).ToList(),

                chuaHt = d.Where(d => d.task.finished_at == null && d.task.startDate <= DateTime.Now && d.task.endDate >= DateTime.Now).Count(),
                list_chuaHt = d.Where(d => d.task.finished_at == null && d.task.startDate <= DateTime.Now && d.task.endDate >= DateTime.Now).Select(d => d.task.id).ToList(),

                quaHan = d.Where(d => d.task.finished_at == null && d.task.endDate < DateTime.Now).Count(),
                list_quaHan = d.Where(d => d.task.finished_at == null && d.task.endDate < DateTime.Now).Select(d => d.task.id).ToList()
            }).OrderByDescending(d => d.count);
            var data = new
            {
                labels = group.Select(d => d.label).ToList(),
                datasets = new List<Chart>() {
                new Chart { label = "Hoàn thành trước hạn", data = group.Select(d => d.truocHan).ToList() } ,
                new Chart { label = "Hoàn thành đúng hạn", data = group.Select(d => d.dungHan).ToList() } ,
                new Chart { label = "Hoàn thành trễ hạn", data = group.Select(d => d.treHan).ToList() } ,
                new Chart { label = "Chưa hoàn thành", data = group.Select(d => d.chuaHt).ToList() } ,
                new Chart { label = "Quá hạn", data = group.Select(d => d.quaHan).ToList() } ,
            }
            };

            return Json(new
            {
                data = data,
                group = group,
            });
        }
        [HttpPost]
        public async Task<JsonResult> congviecthuchien(List<string>? nhanviens)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string user_id = UserManager.GetUserId(currentUser); // Get user id:
            var tmp = _context.TaskAssigneeModel.Include(d => d.user).Include(d => d.task).Where(d => d.task.deleted_at == null && d.task.finished_at == null);
            if (nhanviens != null && nhanviens.Count > 0)
            {
                tmp = tmp.Where(d => nhanviens.Contains(d.userId));
            }
            else
            {
                nhanviens = _context.UserManagerModel.Where(d => d.userId == user_id).Select(d => d.userManagerId).ToList();
                tmp = tmp.Where(d => nhanviens.Contains(d.userId));
            }
            var dulieu = tmp.ToList();
            var group = dulieu.GroupBy(d => new { d.userId, d.user.FullName }).Select(d => new
            {
                label = d.Key.FullName,
                id = d.Key.userId,
                count = d.Count(),
                list = d.Select(d => d.task.id).ToList(),
            }).ToList();

            var ids = group.Select(d => d.id).ToList();
            //IEnumerable<string> list_delete = UserRoleModel_old.Except(roles);
            IEnumerable<string> list_add = nhanviens.Except(ids);
            if (list_add != null)
            {
                foreach (string key in list_add)
                {
                    var user = _context.UserModel.Where(d => d.Id == key).FirstOrDefault();
                    group.Add(new
                    {
                        label = user.FullName,
                        id = key,
                        count = 0,
                        list = new List<int>()
                    });
                }
            }
            group = group.OrderByDescending(d => d.count).ToList();
            var data = new { labels = group.Select(d => d.label).ToList(), datasets = new List<Chart>() { new Chart { label = "Công việc đang thực hiện", data = group.Select(d => d.count).ToList() } } };

            return Json(new
            {
                data = data,
                group = group,
            });
        }
        public async Task<JsonResult> departments()
        {
            var All = GetChild(0);
            //var jsonData = new { data = ProcessModel };
            return Json(All, new System.Text.Json.JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
        private List<SelectDepartmentResponse> GetChild(int parent)
        {
            var DepartmentModel = _context.DepartmentModel.Where(d => d.deleted_at == null && d.parent == parent).OrderBy(d => d.stt).ToList();
            var list = new List<SelectDepartmentResponse>();
            if (DepartmentModel.Count() > 0)
            {
                foreach (var department in DepartmentModel)
                {
                    //if (users.Count == 0)
                    //    continue;
                    var DepartmentResponse = new SelectDepartmentResponse
                    {

                        id = department.id.ToString(),
                        label = department.name,
                        name = department.name,
                        is_department = true,
                    };
                    //var count_child = _context.DepartmentModel.Where(d => d.deleted_at == null && d.parent == department.id).Count();
                    //if (count_child > 0)
                    //{
                    var child = GetChild(department.id);
                    var users = _context.UserDepartmentModel.Where(d => d.department_id == department.id).Include(d => d.user).ToList();
                    if (users.Count() == 0 && child.Count() == 0)
                        continue;
                    foreach (var item in users)
                    {
                        var user = item.user;
                        child.Add(new SelectDepartmentResponse
                        {

                            id = user.Id.ToString(),
                            label = user.FullName + "<" + user.Email + ">",
                            name = user.FullName,
                        });
                    }
                    if (child.Count() > 0)
                        DepartmentResponse.children = child;
                    //}
                    list.Add(DepartmentResponse);



                }
            }
            return list;
        }
    }

    public class Chart
    {
        public string? label { get; set; }
        public List<int> data { get; set; }

        public bool? fill { get; set; }
    }
}
