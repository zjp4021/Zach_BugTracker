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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper ticketHelper = new TicketHelper();
        private RoleHelper roleHelper = new RoleHelper();
        private TicketHistoryHelper ticketHistoryHelper = new TicketHistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();


        //Get Assign Ticket
        [Authorize(Roles = "Admin, DemoAdmin, Project_Manager, DemoPM")]
        public ActionResult AssignTicket(int? id)
        {
            RoleHelper helper = new RoleHelper();
            ProjectsHelper projHelper = new ProjectsHelper();

            var ticket = db.Tickets.Find(id);
            var userList = projHelper.ListUsersOnProjectInRole(ticket.ProjectId,"Developer").ToList();
            //var users = helper.UsersInRole("Developer").ToList();
            
            ViewBag.AssignedToUserId = new SelectList(userList, "Id", "FullName", ticket.AssignedToUserId);

            return View(ticket);
        }

        ////Post Assign Ticket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignTicket(Ticket model)
        {
            var userDemo = db.Users.Find(User.Identity.GetUserId());
            if (userDemo.IsUserDemo())
            {
                return RedirectToAction("Index");
            }
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == model.Id);

            var ticket = db.Tickets.Find(model.Id);
            ticket.DeveloperId = model.AssignedToUserId;
            ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.StatusName == "Assigned").Id;
            db.SaveChanges();

            var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == model.Id);

            notificationHelper.ManageNotifications(oldTicket, newTicket);

            var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);


            try
            {
                EmailService ems = new EmailService();
                IdentityMessage msg = new IdentityMessage();
                ApplicationUser user = db.Users.Find(model.AssignedToUserId);

                msg.Body = "You have been assigned a new Ticket." + Environment.NewLine +
                            "Please click the following link to view the details" +
                            "<a href=\"" + callbackUrl + "\">NEW TICKET</a>";

                msg.Destination = user.Email;
                msg.Subject = "Invite to Household";

                await ems.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                await Task.FromResult(0);
            }

            return RedirectToAction("Index");
        }




        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            //All tickets
            var allTickets = db.Tickets;
            var highPriorityTickets = db.TicketPriorities.FirstOrDefault(tp => tp.PriorityName == "High").Tickets;
            var medPriorityTickets = db.Tickets.Where(t => t.TicketPriority.PriorityName == "Medium");

            //What role do I occupy 
            return View(ticketHelper.ListMyTickets());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
  
        [Authorize(Roles = "Submitter, DemoSubmitter")]
        public ActionResult Create(int? projectId)
        {
            

            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.ProjectId = projectId;
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,SubmitterId,Title,Description")] Ticket ticket)
        {
            var userDemo = db.Users.Find(User.Identity.GetUserId());
            if (userDemo.IsUserDemo())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                ticket.SubmitterId = User.Identity.GetUserId(); 
                ticket.Created = DateTime.Now;
                ticket.TicketStatusId = ticketHelper.SetDefaultTicketStatus();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FullName", ticket.DeveloperId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FullName", ticket.SubmitterId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles =("Submitter, Developer, DemoSubmitter, DemoDeveloper"))]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }


            var userId = User.Identity.GetUserId();
            if (roleHelper.ListUserRoles(userId).FirstOrDefault().Contains("Developer"))
            {
                if (db.Tickets.Find(id).DeveloperId != userId)
                {
                    //sweetAlert2 so only users assigned to specific ticket can Edit
                    TempData["Message"] = "You can not edit tickets you are not assigned to.";
                    return RedirectToAction("Index");
                }
            }

            if (roleHelper.ListUserRoles(userId).FirstOrDefault().Contains("Submitter"))
            {
                if (db.Tickets.Find(id).SubmitterId != userId)
                {
                    //sweetAlert2 so only users assigned to specific ticket can Edit
                    TempData["Message"] = "You can not edit tickets you did not create.";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FullName", ticket.DeveloperId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FullName", ticket.SubmitterId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,SubmitterId,DeveloperId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //Record old ticket before it gets updated
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                var statuses = db.TicketStatus.ToList();
                foreach (var status in statuses)
                {
                    if (ticket.TicketStatusId == status.Id)
                    {
                        oldTicket.TicketStatusId = statuses.FirstOrDefault(t => t.Id == ticket.TicketStatusId).Id;
                    }
                } 

                oldTicket.Updated = DateTime.Now;
                db.Entry(oldTicket).State = EntityState.Modified;
                db.SaveChanges();

                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                ticketHistoryHelper.RecordHistoricalChanges(oldTicket, newTicket);
                notificationHelper.ManageNotifications(oldTicket, newTicket);
                
                    
                return RedirectToAction("Index");
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FullName", ticket.DeveloperId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FullName", ticket.SubmitterId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ProjectName", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "PriorityName", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "StatusName", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "TypeName", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
    }
}
