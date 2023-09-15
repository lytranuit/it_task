

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Spire.Xls;
using System.Collections;
using Vue.Data;
using Vue.Models;

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
            var project = _context.ProjectModel.Where(d => d.id == projectId).FirstOrDefault();
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

    }
}
