

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

    public class ProjectController : BaseController
    {
        private readonly IConfiguration _configuration;
        private UserManager<UserModel> UserManager;
        public ProjectController(ItContext context, IConfiguration configuration, UserManager<UserModel> UserMgr) : base(context)
        {
            _configuration = configuration;
            UserManager = UserMgr;
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var project = _context.ProjectModel.Where(d => d.id == id).FirstOrDefault();
            project.deleted_at = DateTime.Now;
            _context.Update(project);
            _context.SaveChanges();
            return Json(project);
        }
        [HttpPost]
        public async Task<JsonResult> Save(ProjectModel ProjectModel, List<string> list_assignee)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user_id = UserManager.GetUserId(currentUser);
            ProjectModel? ProjectModel_old;
            if (ProjectModel.id == 0)
            {
                ProjectModel.created_at = DateTime.Now;
                ProjectModel.created_by = user_id;
                ProjectModel.assignees = null;
                _context.ProjectModel.Add(ProjectModel);
                _context.SaveChanges();
                List<ProjectStatusModel> list = new List<ProjectStatusModel>()
                {
                    new ProjectStatusModel()
                    {
                        created_at = DateTime.Now,
                        name= "To do",
                        color="#000000",
                        rank="i",
                        projectId = ProjectModel.id
                    },
                    new ProjectStatusModel()
                    {
                        created_at = DateTime.Now,
                        name= "Done",
                        color="#000000",
                        rank="j",
                        projectId = ProjectModel.id
                    }
                };
                _context.AddRange(list);
                _context.SaveChanges();

                ProjectModel_old = ProjectModel;
            }
            else
            {
                ProjectModel_old = _context.ProjectModel.Where(d => d.id == ProjectModel.id).FirstOrDefault();
                ProjectModel_old.updated_at = DateTime.Now;
                ////Field Allow edit
                ProjectModel_old.name = ProjectModel.name;
                ProjectModel_old.description = ProjectModel.description;
                _context.Update(ProjectModel_old);
                _context.SaveChanges();
            }
            if (list_assignee != null)
            {
                var list_old = _context.ProjectAssigneeModel.Where(d => d.projectId == ProjectModel_old.id).ToList();
                _context.RemoveRange(list_old);

                var assignees = new List<ProjectAssigneeModel>();
                foreach (var assignee in list_assignee)
                {
                    assignees.Add(new ProjectAssigneeModel
                    {
                        userId = assignee,
                        projectId = ProjectModel_old.id
                    });
                }
                _context.AddRange(assignees);
                _context.SaveChanges();

            }
            return Json(ProjectModel);
        }
        public async Task<JsonResult> GetList()
        {
            var projects = _context.ProjectModel.Where(d => d.deleted_at == null).ToList();
            return Json(projects);
        }
        public async Task<JsonResult> Get(int id)
        {
            var project = _context.ProjectModel.Where(d => d.id == id && d.deleted_at == null).Include(d => d.assignees).FirstOrDefault();

            return Json(project);
        }

    }
}
