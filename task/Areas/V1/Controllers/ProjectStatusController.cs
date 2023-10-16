

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

namespace it_template.Areas.V1.Controllers
{

    public class ProjectStatusController : BaseController
    {
        private readonly IConfiguration _configuration;
        private UserManager<UserModel> UserManager;
        public ProjectStatusController(ItContext context, IConfiguration configuration, UserManager<UserModel> UserMgr) : base(context)
        {
            _configuration = configuration;
            UserManager = UserMgr;
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id, int moveId)
        {
            var ProjectStatus = _context.ProjectStatusModel.Where(d => d.id == id).FirstOrDefault();
            ProjectStatus.deleted_at = DateTime.Now;
            _context.Update(ProjectStatus);
            var listTask = _context.TaskModel.Where(d => d.statusId == id).ToList();
            foreach (var task in listTask)
            {
                task.statusId = moveId;
            }
            _context.UpdateRange(listTask);
            _context.SaveChanges();
            return Json(ProjectStatus);
        }
        [HttpPost]
        public async Task<JsonResult> Save(ProjectStatusModel ProjectStatusModel, List<string>? list_assignee)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var user_id = UserManager.GetUserId(currentUser);
            ProjectStatusModel? ProjectStatusModel_old;
            if (ProjectStatusModel.id == 0)
            {
                ProjectStatusModel.created_at = DateTime.Now;
                ProjectStatusModel.created_by = user_id;

                ProjectStatusModel.rank = getRank(ProjectStatusModel);
                //var number = convertInt(ProjectStatusModel.rank, ProjectStatusModel.rank.Length);
                //var string1 = convertString(number, ProjectStatusModel.rank.Length);
                _context.ProjectStatusModel.Add(ProjectStatusModel);

                _context.SaveChanges();

                ProjectStatusModel_old = ProjectStatusModel;

            }
            else
            {
                ProjectStatusModel_old = _context.ProjectStatusModel.Where(d => d.id == ProjectStatusModel.id).FirstOrDefault();
                ProjectStatusModel_old.updated_at = DateTime.Now;
                ////Field Allow edit
                ProjectStatusModel_old.color = ProjectStatusModel.color;
                ProjectStatusModel_old.name = ProjectStatusModel.name;
                _context.Update(ProjectStatusModel_old);
                _context.SaveChanges();
            }

            return Json(ProjectStatusModel_old);
        }
        public async Task<JsonResult> GetList(int projectId)
        {
            var ProjectStatuss = _context.ProjectStatusModel.Where(d => d.projectId == projectId && d.deleted_at == null).OrderBy(d => d.rank).ToList();
            return Json(ProjectStatuss, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });
        }

        public async Task<JsonResult> Get(int id)
        {
            var ProjectStatus = _context.ProjectStatusModel.Where(d => d.id == id && d.deleted_at == null).FirstOrDefault();

            return Json(ProjectStatus);
        }

        public async Task<JsonResult> Rank(int id, int issueId, string type)
        {
            var ProjectStatus = _context.ProjectStatusModel.Where(d => d.id == id && d.deleted_at == null).FirstOrDefault();
            var issueProjectStatus = _context.ProjectStatusModel.Where(d => d.id == issueId && d.deleted_at == null).FirstOrDefault();

            ProjectStatus.rank = getRank(ProjectStatus, issueProjectStatus, type);
            _context.Update(ProjectStatus);
            _context.SaveChanges();
            return Json(ProjectStatus);
        }
        private string getRank(ProjectStatusModel ProjectStatusModel, ProjectStatusModel? issueProjectStatus = null, string type = "middleSegment")
        {
            var rank_return = ProjectStatusModel.rank;
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
                issueProjectStatus = _context.ProjectStatusModel.Where(d => d.id != ProjectStatusModel.id).OrderBy(d => d.rank).LastOrDefault();
                if (issueProjectStatus != null && issueProjectStatus.rank != null)
                {
                    var issue_rank = issueProjectStatus.rank;
                    var number_issue_rank = convertInt(issue_rank, issue_rank.Length);
                    var nextIndex = number_issue_rank + 1;
                    rank_return = convertString(nextIndex, issue_rank.Length);
                }
                else
                {
                    rank_return = "i";
                }

            }
            else if (type == "bottomSegment")
            {
                var issue_rank = issueProjectStatus.rank;
                var issue_next = _context.ProjectStatusModel.Where(d => d.rank.CompareTo(issue_rank) > 0).OrderBy(d => d.rank).FirstOrDefault();
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
                var issue_rank = issueProjectStatus.rank;
                var issue_prev = _context.ProjectStatusModel.Where(d => d.rank.CompareTo(issue_rank) < 0).OrderBy(d => d.rank).LastOrDefault();
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
    }
}
