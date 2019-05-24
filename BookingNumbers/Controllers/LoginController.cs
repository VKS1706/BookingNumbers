using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingNumbers.Models;
using DataCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System.Web;
using BookingNumbers.Tools;
using Microsoft.AspNetCore.Http;

namespace BookingNumbers.Controllers
{   
    // Controller for the login page
    public class LoginController : Controller
    {
        private BookingDBContext _context;

        public LoginController(BookingDBContext context)
        {
            _context = context; // The database context class
        }

        // The login page action
        public IActionResult Index()
        {
            // Create a new view model
            var vm = new LoginViewModel();

            // Get the broadcasts from the database
            vm.LoginBroadcasts = _context.Broadcasts.ToList();

            // Return the view
            return View(vm);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel vm)
        {
            // Check if they entered a valid username / password combination
            var valid = PassValidator.ValidatePassword(_context, vm.UserName, vm.Password);

            // If not valid
            if (!valid)
            {
                // Set password back to null
                vm.Password = "";
                // Return back to the login page
                return RedirectToAction("Index");
            }

            // Set session username
            HttpContext.Session.SetString("Username", vm.UserName);
            var roleId = _context.Users.First(u => u.UserName == vm.UserName).RoleId;
            // Set session role
            HttpContext.Session.SetString("Role", _context.Roles.First(r => r.RoleId == roleId).RoleName);


            // Redirect to the dashboard - SUCCESSFUL LOGIN
            return Redirect("Project/Dashboard");

        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Username", ""); // Set the username to null
            HttpContext.Session.SetString("Role", ""); // Set the role to null

            return RedirectToAction("Index"); // Redirect to the login page
        }
    }
}