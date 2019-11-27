using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]


    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DemoUser()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

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
                    var emailTo = ConfigurationManager.AppSettings["emailfrom"];

                    var from = $"{model.FromEmail}<{emailTo}>";

                    var email = new MailMessage(from, emailTo)
                    {
                        Subject = model.Subject,
                        Body = $"You have received an email from your Bug Tracker <br /> {model.Body}",
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
            }
            return View(model);
        }
    };
}