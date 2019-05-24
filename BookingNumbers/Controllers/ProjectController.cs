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
    // This controller deals with the projects
    public class ProjectController : Controller
    {
        private BookingDBContext _context;
        private readonly AuthoriseUser _auth;

        public ProjectController(BookingDBContext context, AuthoriseUser auth)
        {
            _context = context; // The database context object
            _auth = auth; // Authentication service
        }

        public IActionResult Project(int id = -1)
        {
            if (!_auth.Authorise(RolesEnum.Staff, _context)) // Check logged in
                return Redirect("~/Login/Index");

            var userId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;

            var projectUser = _context.ProjectUsers.Where(p => p.ProjectId == id).Where(u => u.UserId == userId).ToList();

            if (projectUser.Count != 1) // Check if user is in project
                return Redirect("~/Project/Dashboard");

            if (id == -1 || !_context.Projects.Any(p => p.ProjectId == id)) // Check project exists and id was given
                return Redirect("~/Project/Dashboard");

            
            var vm = new ViewProjectViewModel(); // Create a new view model
            vm.Project = _context.Projects.First(p => p.ProjectId == id); // Get the project record and add it to the view model.

            var listOfUsersInProject = _context.ProjectUsers.Where(p => p.ProjectId == id).ToList(); // Get users in project from database
            vm.ProjectUsers = new List<TableProjectUser>(); // Instanciate a new list for project users

            // Go through each user
            foreach (var user in listOfUsersInProject)
            {
                // The actual user record
                var accuser = _context.Users.First(u => u.UserId == user.UserId);

                // New table project user and fill in fields
                var temp = new TableProjectUser();
                temp.Username = accuser.UserName;
                temp.Email = accuser.Email ?? "User has not given an email.";
                temp.Role = _context.Roles.First(r => r.RoleId == accuser.RoleId).RoleName;

                var minutesBooked =
                    _context.ProjectMinutesBooked.Where(p => p.ProjectId == id && p.UserId == user.UserId);

                temp.MinutesBooked = 0;

                foreach (var minutes in minutesBooked)
                {
                    temp.MinutesBooked += minutes.AmountOfMinutes;
                }

                // Add new table project user to view model
                vm.ProjectUsers.Add(temp);
            }
            
            // return the Project view
            return View(vm);
        }

        // Creating a project action
        public IActionResult CreateProject()
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in
                return Redirect("~/Project/Dashboard");

            var vm = new CreateProjectViewModel(); // create new view model

            // return the create project view
            return View(vm);
        }

        // The post action for creating a project
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateProject(CreateProjectViewModel vm)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in
                return Redirect("~/Project/Dashboard");

            // Create a new project and fill in fields
            var newProj = new Projects();
            newProj.BookingNumber = vm.ProjectNumber;
            newProj.MaximumMinutes = vm.MaxHours * 60;
            newProj.CurrentUsedMinutes = 0;
            newProj.Locked = false;
            newProj.ProjectDescription = vm.ProjectDesc;
            newProj.ProjectName = vm.ProjectName;

            // Add project to database
            _context.Projects.Add(newProj);
            // Save database
            _context.SaveChanges();

            // Kick out to user management
            return Redirect("~/UserManagement/ManageUsers");
        }

        // Post action for booking hours
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult BookHours(ViewProjectViewModel vm, int id)
        {
            // create a new record and fill out fields
            var rec = new ProjectMinutesBooked();
            rec.UserId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;
            rec.ProjectId = id;
            rec.AmountOfMinutes = (vm.HoursBooked * 60) + vm.MinutesBooked;
            rec.DateOfBooking = DateTime.Now;

            // Add minutes to the project
            _context.Projects.First(p => p.ProjectId == id).CurrentUsedMinutes += rec.AmountOfMinutes;

            // Add the record
            _context.ProjectMinutesBooked.Add(rec);
            
            // Get the project record
            var proj = _context.Projects.First(p => p.ProjectId == id);

            // If over budget
            if (proj.CurrentUsedMinutes > proj.MaximumMinutes)
            {
                // Generate an automatic broadcast
                var broadcastTitle = $"Project: {proj.ProjectName}, has run over-budget.";

                // Check there isn't already a broadcast with the automatic broadcast signature 
                if (!_context.Broadcasts.Any(b => b.Title == broadcastTitle))
                {
                    // Create a new broadcast
                    var broadcast = new Broadcasts();
                    broadcast.Title = broadcastTitle;
                    broadcast.Body = "This project has gone over the maximum amount of hours that was budgeted. Project is being locked.";
                    broadcast.UserId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;

                    // Add broadcast to the database
                    _context.Broadcasts.Add(broadcast);

                    // Lock the project
                    _context.Projects.First(p => p.ProjectId == id).Locked = true;
                }
            }

            // Save the changes to the database
            _context.SaveChanges();

            // Kick back to the project page
            return Redirect($"~/Project/Project/{id}");
        }

        // Function for the dashboard
        public IActionResult Dashboard()
        {
            if (!_auth.Authorise(RolesEnum.Staff, _context)) // Authenticate user
                return Redirect("~/Login/Index");

            var userName = HttpContext.Session.GetString("Username"); // Get the username from the session

            var userId = _context.Users.First(u => u.UserName == userName).UserId; // Get the user id from the username

            var userProjs = _context.ProjectUsers.Where(p => p.UserId == userId).Select(p => p.ProjectId).ToList(); //Gets all project ID's that the user is in.
            
            var vm = new DashboardViewModel(); // Create a new view model
            vm.UsersProjects = new List<Projects>(); // Create a new list for the user projects

            // Add each project to the view model
            foreach (var proj in userProjs)
            {
                vm.UsersProjects.Add(_context.Projects.First(p => p.ProjectId == proj)); //Get projects based on the project Id
            }

            // Return the view
            return View(vm);
        }
    }
}