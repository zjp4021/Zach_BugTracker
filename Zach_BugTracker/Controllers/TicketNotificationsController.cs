using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Zach_BugTracker.Helpers;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Controllers
{

    [Authorize]
    [RequireHttps]
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Dismiss(int id)
        {
            var notification = db.TicketNotifications.Find(id);
            notification.IsRead = true;
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Home");
        }


        // GET: TicketNotifications
        public ActionResult Index()
        {
            var ticketNotifications = db.TicketNotifications.Include(t => t.Recipient).Include(t => t.Ticket);
            return View(ticketNotifications.ToList());
        }

        // GET: TicketNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Create
        public ActionResult Create()
        {
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId");
            return View();
        }

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,RecipientId,NotificationBody,IsRead")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                ticketNotifications.CreatedDate = DateTime.Now;
                db.TicketNotifications.Add(ticketNotifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.RecipientId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketNotifications.TicketId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.RecipientId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketNotifications.TicketId);
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,NotificationBody,CreatedDate,RecipientId,IsRead")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketNotifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RecipientId = new SelectList(db.Users, "Id", "FirstName", ticketNotifications.RecipientId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "SubmitterId", ticketNotifications.TicketId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketNotifications ticketNotifications = db.TicketNotifications.Find(id);
            db.TicketNotifications.Remove(ticketNotifications);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Get Assign Ticket
        //public ActionResult AssignTicket(int? id)
        //{
        //    RoleHelper helper = new RoleHelper();

        //    var ticket = db.Tickets.Find(id);
        //    var users = helper.UsersInRole("DEVELOPER").ToList();
        //    ViewBag.AssignedToUserId = new SelectList(users, "Id", "FullName", ticket.AssignedToUserId);

        //    return View(ticket);
        //}

        ////Post Assign Ticket
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AssignTicket(Ticket model)
        //{
        //    var ticket = db.Tickets.Find(model.Id);
        //    ticket.AssignedToUserId = model.AssignedToUserId;
        //    db.SaveChanges();

        //    var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

        //    try
        //    {
        //        EmailService ems = new EmailService();
        //        IdentityMessage msg = new IdentityMessage();
        //        ApplicationUser user = new db.Users.Find(model.AssignedToUserId);

        //        msg.Body = "You have been assigned a new Ticket." + Environment.NewLine +
        //                    "Please click the following link to view the details" +
        //                    "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";

        //        msg.Destination = user.Email;
        //        msg.Subject = "Invite to Household";

        //        await ems.SendMailAsync(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        await Task.FromResult(0);
        //    }

        //    return RedirectToAction("Index");
        //}



    }
}
