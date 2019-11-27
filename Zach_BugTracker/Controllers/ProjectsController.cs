using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Zach_BugTracker.Helpers;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]

    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectsHelper projectsHelper = new ProjectsHelper();

        public ActionResult ManageUsers(int id)
        {
            ViewBag.ProjectId = id;

            #region PM section
            var pmId = projectsHelper.ListUsersOnProjectInRole(id, "Project_Manager").FirstOrDefault().Id;
            ViewBag.ProjectManagerID = new SelectList(roleHelper.UsersInRole("Project_Manager"), "Id", "Email", pmId);
            #endregion

            #region Dev section
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "Email", projectsHelper.ListUsersOnProjectInRole(id, "Developer"));
            #endregion

            #region Sub section
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "Email", projectsHelper.ListUsersOnProjectInRole(id, "Submitter"));
            #endregion

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUsers(int projectId, string projectManagerId, List<string> developers, List<string> submitters)
        {
            foreach(var user in projectsHelper.UsersOnProject(projectId).ToList())
            {
                projectsHelper.RemoveUserFromProject(user.Id, projectId);
            }

            //In order to ensure that I always have the correct and current
            //set of assigned users, I will first remove all users from the project
            //and then I will add back the selected users

            if (!string.IsNullOrEmpty(projectManagerId))
            {
                projectsHelper.AddUserToProject(projectManagerId, projectId);
            }

            if(developers != null)
            {
                foreach (var developerId in developers)
                {
                    projectsHelper.AddUserToProject(developerId, projectId);
                }
            }

            if (submitters != null)
            {
                foreach (var submitterId in submitters)
                {
                    projectsHelper.AddUserToProject(submitterId, projectId);
                }
            }
            return RedirectToAction("ManageUsers", new { id = projectId });
        }


        // GET: Projects

        [Authorize]
    
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();    
            var allProjects = db.Projects;

            if (User.IsInRole("Admin"))
            {
                return View(allProjects);
            }
            else
            {
                return View(projectsHelper.ListUserProjects(user));

            }

        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            Project project = new Project
            {
                Users = projectsHelper.UsersOnProject((int)id).ToList(),
                Tickets = db.Projects.Find((int)id).Tickets.ToList(),
                Description = db.Projects.Find((int)id).Description,
                ProjectName = db.Projects.Find((int)id).ProjectName,
                Created = db.Projects.Find((int)id).Created,
                Updated = db.Projects.Find((int)id).Updated,
                Id = (int)id
            };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create

        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                //project.Submitter = User.Identity.GetUserId();
                project.Created = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                project.Updated = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
