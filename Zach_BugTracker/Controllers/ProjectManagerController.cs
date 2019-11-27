using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zach_BugTracker.Helpers;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class ProjectManagerController : Controller
    {
            private ApplicationDbContext db = new ApplicationDbContext();
            private ProjectsHelper projectsHelper = new ProjectsHelper();


        // GET: ProjectManager

        //[Authorize(Roles = "Admin")]

        public ActionResult ManageProjects()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");
            ViewBag.Project = new MultiSelectList(db.Projects, "Id", "Name");

            var users = new List<ManageProjectsViewModel>();
            foreach (var user in db.Users.ToList())
            {
                var newProjectVM = new ManageProjectsViewModel
                {
                    UserName = $"{user.LastName}, {user.FirstName}",
                    ProjectNames = projectsHelper.ListUserProjects(user.Id).Select(p => p.ProjectName).ToList()
                };

                if (newProjectVM.ProjectNames.Count() == 0)
                    newProjectVM.ProjectNames.Add("N/A");

                users.Add(newProjectVM);               
            }

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjects(List<string> userIds, List<int> project )
        {
            //Step 1: Unenroll all the selected Users from ANY projects
            //they may currently occupy
            foreach (var userId in userIds)
            {
                //What is the role of this person?
                foreach (var proj in projectsHelper.ListUserProjects(userId).ToList())               
                {
                    projectsHelper.RemoveUserFromProject(userId, proj.Id);
                }
            }

            //Step 2: Add them back to the selected Projects
            
            if (project.Count() > 0)
            {
                foreach(var pro in project)
                {
                    foreach (var userId in userIds)
                    {
                        projectsHelper.AddUserToProject(userId, pro);
                    }
                }
            }
            return RedirectToAction("ManageProjects", "ProjectManager");
        }
    }
}