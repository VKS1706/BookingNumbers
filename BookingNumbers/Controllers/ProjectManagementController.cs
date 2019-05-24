using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingNumbers.Models;
using BookingNumbers.Tools;
using DataCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingNumbers.Controllers
{
    public class ProjectManagementController : Controller
    {
        private readonly BookingDBContext _context;
        private readonly AuthoriseUser _auth;

        public ProjectManagementController(BookingDBContext context, AuthoriseUser auth)
        {
            _context = context; // The database object
            _auth = auth; // The authentication service
        }


        public IActionResult ManageProjects()
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");


            var vm = new ManageProjectViewModel(); // Create a new view model
            vm.AllProjects = _context.Projects.Where(p => p.ProjectId > 0).ToList(); // Get all the projects from the database
            
            // Return the Manage Projects view
            return View(vm);
        }

        // Delete project action
        public IActionResult DeleteProject(int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            if (!_context.Projects.Any(p => p.ProjectId == id)) // Make sure the project exists
                return RedirectToAction("ManageProjects");

            var projectToRemove = _context.Projects.First(p => p.ProjectId == id); // Get the record

            _context.Projects.Remove(projectToRemove); // Remove the project

            var projectUsersToRemove = _context.ProjectUsers.Where(p => p.ProjectId == id).ToList(); // Get the projectUsers to remove
            var projectMinutesToRemove = _context.ProjectMinutesBooked.Where(p => p.ProjectId == id).ToList(); // Get the minutes to remove

            _context.ProjectUsers.RemoveRange(projectUsersToRemove); // Remove the projectusers
            _context.ProjectMinutesBooked.RemoveRange(projectMinutesToRemove); // remove the minutes

            _context.SaveChanges(); // Save the database

            // Redirect back to manage projects
            return RedirectToAction("ManageProjects");

        }
        
        // Action to modify a project
        public IActionResult ModifyProject(int id = -1)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            if (!_context.Projects.Any(p => p.ProjectId == id)) // Check the project exists
                return RedirectToAction("ManageProjects");

            var vm = new ModifyProjectViewModel(); // Create a new view model

            var proj = _context.Projects.First(p => p.ProjectId == id); // Get the project record

            // Update the fields
            vm.Body = proj.ProjectDescription;
            vm.Title = proj.ProjectName;
            vm.BookingNumber = proj.BookingNumber;

            // Return the view
            return View(vm);
        }

        // Post function for modify project
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult ModifyProject(ModifyProjectViewModel vm)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate user
                return Redirect("~/Project/Dashboard");

            if (!_context.Projects.Any(p => p.BookingNumber == vm.BookingNumber)) // Check user exists
                return RedirectToAction("ManageProjects");

            // Get project from the database
            var project = _context.Projects.First(p => p.BookingNumber == vm.BookingNumber);

            // Fill out the fields
            project.ProjectName = vm.Title;
            project.ProjectDescription = vm.Body;

            // Save the databases
            _context.SaveChanges();

            // Return the view
            return RedirectToAction("ManageProjects");
        }

        // This is the view to delete a user from a project.
        public IActionResult DeleteUsersFromProject(int id = -1)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            // Create new view model
            var vm = new DeleteUserFromProjectViewModel();

            // Add all project names to view model
            vm.AllProjectNames = _context.Projects.Where(p => p.ProjectId > 0).Select(p => p.ProjectName).ToList();

            if (id > 0)
            {
                // Get selected project
                vm.SelectedProject = _context.Projects.First(p => p.ProjectId == id).ProjectName;

                

                // Get all the user IDs in the project
                var userIds = _context.ProjectUsers.Where(p => p.ProjectId == id).Select(u => u.UserId).ToList();

                // Get all the user objects
                vm.UsersInProject = _context.Users.Where(u => userIds.Contains(u.UserId)).ToList();

                // Add all project names to view model
                vm.AllProjectNames = _context.Projects.Where(p => p.ProjectId > 0).Select(p => p.ProjectName).ToList();

            }

            // Return the view
            return View(vm);
        }

        // Post method to select project
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult DeleteUsersFromProject(DeleteUserFromProjectViewModel vm)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            // Get the project ID
            var projId = _context.Projects.First(p => p.ProjectName == vm.SelectedProject).ProjectId;

            // Set the session project
            HttpContext.Session.SetString("SelectedProject", projId.ToString());

            // return the view.
            return Redirect($"~/ProjectManagement/DeleteUsersFromProject/{projId}");

        }

        public IActionResult DeleteUser(int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            // Get the project Id from the session
            var projId = int.Parse(HttpContext.Session.GetString("SelectedProject"));

            // Get the record of the link
            var user = _context.ProjectUsers.First(u => u.UserId == id && u.ProjectId == projId);

            // Remove the user from the project
            _context.Remove(user);

            // Save the changes.
            _context.SaveChanges();

            // Kick back to the page
            return Redirect($"~/ProjectManagement/DeleteUsersFromProject/{projId}");
        }
    }
}