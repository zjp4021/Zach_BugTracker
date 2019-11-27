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

    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private ProjectsHelper projHelper = new ProjectsHelper();

        // GET: Admin

        [Authorize(Roles = "Admin")]

        public ActionResult ManageRoles()
        {
            ViewBag.UserIds = new MultiSelectList(db.Users,"Id","FullName");
            ViewBag.Role = new SelectList(db.Roles, "Name","Name");

            var users = new List<ManageRolesViewModel>();
            foreach(var user in db.Users.ToList())
            {
                users.Add(new ManageRolesViewModel
                {
                    UserName = $"{user.LastName}, {user.FirstName}",
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault()
                });
            }
            
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string role)
        {
            //Step 1: Unenroll all the selected Users from ANY roles
            //they may currently occupy
            foreach(var userId in userIds)
            {
                //What is the role of this person?
                var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
                if(userRole != null)
                {
                    roleHelper.RemoveUserFromRole(userId, userRole);
                }
            }

            //Step 2: Add them back to the selected Role
            if (!string.IsNullOrEmpty(role))
            {
                foreach (var userId in userIds)
                {
                    roleHelper.AddUserToRole(userId, role);
                }
            }
            return RedirectToAction("ManageRoles", "Admin");
        }
    
    
        [Authorize(Roles = "Admin, Project_Manager, DemoAdmin, DemoPM")]
        public ActionResult ManageProjectUsers()
        {
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "ProjectName");
            ViewBag.Developers = new MultiSelectList(roleHelper.UsersInRole("Developer").Union(roleHelper.UsersInRole("DemoDeveloper")), "Id", "FullName");
            ViewBag.Submitters = new MultiSelectList(roleHelper.UsersInRole("Submitter").Union(roleHelper.UsersInRole("DemoSubmitter")), "Id", "FullName");

            if (User.IsInRole("Admin"))
            {
                ViewBag.ProjectManagerId = new SelectList(roleHelper.UsersInRole("Project_Manager").Union(roleHelper.UsersInRole("DemoPM")), "Id", "FullName");
            }

            //Let's create a View Model for purposes of displaying User's and their associated Projects
            var myData = new List<UserProjectListViewModel>();
            UserProjectListViewModel userVm = null;
            foreach(var user in db.Users.ToList())
            {
                userVm = new UserProjectListViewModel
                {
                    Name = $"{user.LastName}, {user.FirstName}",
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault(),
                    ProjectNames = projHelper.ListUserProjects(user.Id).Select(p => p.ProjectName).ToList()
                };
                if (userVm.ProjectNames.Count() == 0)
                    userVm.ProjectNames.Add("N/A");

                myData.Add(userVm);
            }

            return View(myData);


        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectUsers(List<int> projects, string projectManagerId, List<string> developers, List<string> submitters)
        {
            //Remove users from every project I have selected
            if(projects != null)
            {
                foreach(var projectId in projects)
                {
                    //Remove everyone from THIS project
                    foreach(var user in projHelper.UsersOnProject(projectId).ToList())
                    {
                        projHelper.RemoveUserFromProject(user.Id, projectId);
                    }

                    //Add back a PM if I can
                    if (!string.IsNullOrEmpty(projectManagerId))
                    {
                        projHelper.AddUserToProject(projectManagerId, projectId);
                    }

                    if(developers != null)
                    {
                        foreach(var developerId in developers)
                        {
                            projHelper.AddUserToProject(developerId, projectId);
                        }
                    }

                    if(submitters != null)
                    {
                        foreach(var submitterId in submitters)
                        {
                            projHelper.AddUserToProject(submitterId, projectId);
                        }
                    }          
                }         
            }
            return RedirectToAction("ManageProjectUsers");
        }
    }
}