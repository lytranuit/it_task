
using Vue.Models;
using Microsoft.AspNetCore.Mvc;
using Vue.Data;
using System.Net.Mail;
using Vue.Services;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Fernandezja.ColorHashSharp.Entities;

namespace Vue.Controllers
{

    public class HomeController : Controller
    {
        protected readonly ItContext _context;
        private readonly ViewRender _view;


        public HomeController(ItContext context, ViewRender view)
        {
            _context = context;
            _view = view;
            var listener = _context.GetService<DiagnosticSource>();
            (listener as DiagnosticListener).SubscribeWithAdapter(new CommandInterceptor());
        }

        public JsonResult Index()
        {
            return Json(new { test = 1, message = DateTime.Now });

        }

        public async Task<JsonResult> cronjob()
        {
            var timecheck = DateTime.Now;
            var timecheck1 = DateTime.Now.AddDays(1);

            //var tasks = _context.TaskModel.Where(d => d.deleted_at == null && (d.finished_at == null || d.progress != 100) && d.endDate != null && d.endDate < DateTime.Now.AddDays(1)).Include(d => d.assignees).ToList();
            var tasks = _context.TaskAssigneeModel.Include(d => d.task).Where(d => d.task.deleted_at == null && (d.task.finished_at == null || d.task.progress != 100) && d.task.endDate != null && d.task.endDate >= timecheck && d.task.endDate <= timecheck1).ToList();
            var all = tasks.GroupBy(d => d.userId, (x, y) => new
            {
                num_sign = y.Count(),
                data = y.Select(d => d.task).ToList(),
                userId = x
            }).ToList();
            foreach (var item in all)
            {

                var user = _context.UserModel.Where(d => d.Id == item.userId).FirstOrDefault();
                if (user == null)
                    continue;

                if (user.deleted_at != null || (user.LockoutEnd != null && user.LockoutEnd >= DateTime.Now))
                    continue;
                ///Xóa user nếu user 1 tháng chưa đăng nhập
                //var last_login = user.last_login != null ? user.last_login : user.created_at;
                //if (last_login < DateTime.Now.AddMonths(-1))
                //{
                //    user.LockoutEnd = DateTime.Now.AddDays(360);
                //    _context.Update(user);
                //    _context.SaveChanges();
                //    continue;
                //}
                //foreach (var da in item.data)
                //{
                //    var type = _context.DocumentTypeModel.Where(d => d.id == da.type_id).FirstOrDefault();
                //    da.type = type;
                //}
                var mail_string = user.Email;
                string Domain = (HttpContext.Request.IsHttps ? "https://" : "http://") + HttpContext.Request.Host.Value;
                var body = _view.Render("Emails/DueTask", new { link_logo = Domain + "/images/clientlogo_astahealthcare.com_f1800.png", link = Domain, count = item.num_sign, data = item.data });
                var email = new EmailModel
                {
                    email_to = mail_string,
                    subject = "[Nhắc nhở] Các công việc gần đến hạn hoàn thành.",
                    body = body,
                    email_type = "DueTask",
                    status = 1
                };
                _context.Add(email);

            }

            ////Quá hạn
            var query = _context.TaskAssigneeModel.Include(d => d.task).Where(d => d.task.deleted_at == null && (d.task.finished_at == null || d.task.progress != 100) && d.task.endDate != null && d.task.endDate < timecheck);
            var query_string = query.ToQueryString();
            var tasks_overdue = query.ToList();
            var all_overdue = tasks_overdue.GroupBy(d => d.userId, (x, y) => new
            {
                num_sign = y.Count(),
                data = y.Select(d => d.task).ToList(),
                userId = x
            }).ToList();
            foreach (var item in all_overdue)
            {

                var user = _context.UserModel.Where(d => d.Id == item.userId).FirstOrDefault();
                if (user == null)
                    continue;

                if (user.deleted_at != null || (user.LockoutEnd != null && user.LockoutEnd >= DateTime.Now))
                    continue;
                ///Xóa user nếu user 1 tháng chưa đăng nhập
                //var last_login = user.last_login != null ? user.last_login : user.created_at;
                //if (last_login < DateTime.Now.AddMonths(-1))
                //{
                //    user.LockoutEnd = DateTime.Now.AddDays(360);
                //    _context.Update(user);
                //    _context.SaveChanges();
                //    continue;
                //}
                //foreach (var da in item.data)
                //{
                //    var type = _context.DocumentTypeModel.Where(d => d.id == da.type_id).FirstOrDefault();
                //    da.type = type;
                //}
                var mail_string = user.Email;
                string Domain = (HttpContext.Request.IsHttps ? "https://" : "http://") + HttpContext.Request.Host.Value;
                var body = _view.Render("Emails/OverDueTask", new { link_logo = Domain + "/images/clientlogo_astahealthcare.com_f1800.png", link = Domain, count = item.num_sign, data = item.data });
                var email = new EmailModel
                {
                    email_to = mail_string,
                    subject = "[Quá hạn] Các công việc đã quá hạn.",
                    body = body,
                    email_type = "OverDueTask",
                    status = 1
                };
                _context.Add(email);

            }
            _context.Save();
            return Json(new { success = true, data = tasks });
        }


    }
    class SuccesMail
    {
        public int success { get; set; }
        public Exception ex { get; set; }
    }
}
