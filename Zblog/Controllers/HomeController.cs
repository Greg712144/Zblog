using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zblog.Models;
using PagedList;
using PagedList.Mvc;

namespace Zblog.Controllers
{    
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);

            var pageSize = 3;
            var pageNumber = (page ?? 1);
            return View(blogList.ToPagedList(pageNumber, pageSize));
        }

        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            IQueryable<BlogPost> result = null;
            if (searchStr != null)
            {
                result = db.BlogPosts.AsQueryable();
                result = result.Where(p => p.Title.Contains(searchStr) ||
                                            (p.Body.Contains(searchStr) ||
                                             p.Comments.Any(c => c.Body.Contains(searchStr) ||
                                                                 c.Author.FirstName.Contains(searchStr) ||
                                                                 c.Author.LastName.Contains(searchStr) ||
                                                                 c.Author.DisplayName.Contains(searchStr) ||
                                                                 c.Author.Email.Contains(searchStr))));

            }
            else
            {
                result = db.BlogPosts.AsQueryable();
            }

            return result.OrderByDescending(p => p.Created);
        }


        public ActionResult BlogNow()
        {
            ViewBag.Message = "My Blogs";

            return View();
        }

        public ActionResult Posts()
        {
            ViewBag.Message = "Posts";

            return View(db.BlogPosts.ToList());
        }


        //HTTP : GET
        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From:<bold>{0}</bold><p>({1})</p><p>Mesaage:</p><p>{2}</p>";
                    var emailto = ConfigurationManager.AppSettings["emailto"];
                    var from = string.Format("Zblog<{0}>", emailto);

                    model.Body = "This is a message from your blog site. The name and the email of the contacting person is above." + model.Body;

                    var email = new MailMessage(from, emailto)       
                    {
                        Subject = model.Subject,
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }

                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
       
            }

            return View(model);

        }
    }
}