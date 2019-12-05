using Zach_BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Zach_BugTracker.Helpers
{
    public class ProjectsHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
       
      

        public List<ApplicationUser> ListUsersOnProjectInRole(int projectId, string roleName)
        {
            var userIdList = new List<ApplicationUser>();
            foreach (var user in UsersOnProject(projectId))
            {
                if (roleHelper.IsUserInRole(user.Id, roleName))
                    userIdList.Add(user);
            }
            return userIdList;
        }


        public bool IsUserOnProject(string userId, int projectId) 
        { 
            var project = db.Projects.Find(projectId); 
            var flag = project.Users.Any(u => u.Id == userId); 
            return (flag); 
        }
        public ICollection<Project> ListUserProjectsOn(string userId)
        {
            var projects = db.Projects.ToList();
            var projList = new List<Project>();

            foreach(var proj in projects)
            {

                if (IsUserOnProject(userId, proj.Id))
                { 
                    
                    projList.Add(proj);
                    db.SaveChanges();
                }

                
            }


           
            return (projList);
        }

        public ICollection<Project> ListUserProjects(string userId) 
        { 
            ApplicationUser user = db.Users.Find(userId); 
            
            var projects = user.Projects.ToList(); 
            return (projects); 
        }
        
        public void AddUserToProject(string userId, int projectId) 
        { 
            if (!IsUserOnProject(userId, projectId)) 
            { 
                Project proj = db.Projects.Find(projectId); 
                var newUser = db.Users.Find(userId); 
                
                proj.Users.Add(newUser); 
                db.SaveChanges(); 
            } 
        }
        
        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId); 
                var delUser = db.Users.Find(userId); 
                
                proj.Users.Remove(delUser); 
                db.Entry(proj).State = EntityState.Modified; // just saves this obj instance.
                db.SaveChanges();
            }
        }
        
        public ICollection<ApplicationUser> UsersOnProject(int projectId)
        { 
            return db.Projects.Find(projectId).Users;
        }
        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();}
        }
    }




