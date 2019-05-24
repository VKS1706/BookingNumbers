using System.Collections.Generic;
using System.Linq;
using BookingNumbers.Models;
using BookingNumbers.Tools;
using DataCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingNumbers.Controllers
{
    public class UserManagementController : Controller
    {
        BookingDBContext _context;
        private readonly AuthoriseUser _auth;

        public UserManagementController(BookingDBContext context, AuthoriseUser auth)
        {
            _context = context; // Database context
            _auth = auth; // Authentication service
        }

        // Add user view
        public IActionResult AddUser()
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in as admin
                return Redirect("~/Project/Dashboard");

            List<Roles> roles = _context.Roles.ToList(); // Get roles from database

            // Create new view model and fill in fields
            var vm = new AddUserViewModel();
            vm.AllRoles = roles;
            vm.ErrorMessage = "";

            // Return the add user view
            return View(vm);
        }

        // Add user pose function
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddUser(AddUserViewModel vm)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in as admin
                return Redirect("~/Project/Dashboard");

            // Reset error message
            vm.ErrorMessage = "";
            // Get roles from database and fill in field
            List<Roles> roles = _context.Roles.ToList();
            vm.AllRoles = roles;

            // Create new user
            var newUser = new Users();

            // If username exists
            var userTemp = _context.Users.Any(r => r.UserName == vm.UserName);
            if (userTemp)
            {
                vm.ErrorMessage += "Username already exists\n";
            }
            //Validate Password
            if (vm.Password != vm.ConfirmPassword)
            {
                vm.ErrorMessage += "Passwords must be equal.\n";
            }

            // If there's an error message
            if (vm.ErrorMessage != "")
            {
                return View(vm);
            }


            // Encrypt Password
            // Generate Salt
            var salt = Salter.Shake();

            // Hash Password
            var hashedPass = Hasher.Hash(vm.Password + salt);

            // Fill in fields
            newUser.UserName = vm.UserName;
            newUser.HashedPassword = hashedPass;
            newUser.Salt = salt;

            newUser.RoleId = _context.Roles.First(r => r.RoleName == vm.RoleName).RoleId;

            // check if email is null

            if (vm.Email != null)
            {
                newUser.Email = vm.Email; // Only add email if one exists
            }
            
            // Add users to database
            _context.Users.Add(newUser);

            // Save the database
            _context.SaveChanges();

            // Redirect to the login page
            return Redirect("/Login/Index");
        }

        // Manage users view
        public IActionResult ManageUsers(int id = -1)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in as admin
                return Redirect("~/Project/Dashboard");

            var vm = new ManageUsersViewModel(); // new view model

            vm.AllUsers = new List<TableProjectUser>(); // instanciate all users

            // Foreach user in the database
            foreach (var user in _context.Users)
            {
                // Create a temp user
                var temp = new TableProjectUser();
                temp.Email = user.Email ?? "This user has not given an email.";
                temp.Username = user.UserName;
                temp.Role = _context.Roles.First(r => r.RoleId == user.RoleId).RoleName;
                temp.MinutesBooked = user.UserId;

                // Add temp user to view model
                vm.AllUsers.Add(temp);
            }

            // If there's an id given and it's a valid id
            if (id != -1 && _context.Users.Any(u => u.UserId == id))
            {
                // Create new temp selected user and fill out fields
                var temp = new SelectUser();
                temp.User = _context.Users.First(u => u.UserId == id);
                temp.Role = _context.Roles.First(r => r.RoleId == temp.User.RoleId).RoleName;

                var projectIds = _context.ProjectUsers.Where(p => p.UserId == temp.User.UserId).Select(c => c.ProjectId).ToList();

                temp.MemberOfProjects = _context.Projects.Where(p => projectIds.Contains(p.ProjectId))
                    .Select(p => p.ProjectName).ToList();

                // All user to view model
                vm.SelectedUser = temp;
            }

            // Get project names from the database
            vm.ProjectNames = _context.Projects.Select(p => p.ProjectName).ToList();
            // Get roles from the database
            vm.AllRoles = _context.Roles.Select(r => r.RoleName).ToList();

            // Return the view
            return View(vm);
        }

        // Add to project function 
        public IActionResult AddToProj(ManageUsersViewModel vm, int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in as admin
                return Redirect("~/Project/Dashboard");

            var projId = _context.Projects.First(p => p.ProjectName == vm.SelectedProjectName).ProjectId; // Get project from database

            // If the user is already in the project
            if (_context.ProjectUsers.Any(p => p.ProjectId == projId && p.UserId == id))
            {
                return Redirect($"~/UserManagement/ManageUsers/{id}");
            }

            // Create new user project link
            var temp = new ProjectUsers();
            temp.UserId = id;
            temp.ProjectId = _context.Projects.First(p => p.ProjectName == vm.SelectedProjectName).ProjectId;

            // Add the link to the database
            _context.ProjectUsers.Add(temp);

            // Save the database
            _context.SaveChanges();

            // Kick back to the management page
            return Redirect($"~/UserManagement/ManageUsers/{id}");
        }

        public IActionResult ChangePassword(ManageUsersViewModel vm, int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in as admin
                return Redirect("~/Project/Dashboard");

            var salt = Salter.Shake(); // Get a new salt
            var hashedPassword = Hasher.Hash(vm.NewPassword + salt); // Hash the salted password

            var rec = _context.Users.First(u => u.UserId == id); // Get the record and update the values
            rec.Salt = salt;
            rec.HashedPassword = hashedPassword;

            // Save the changes to the database
            _context.SaveChanges();

            // Kick the users back to the user management view
            return Redirect($"~/UserManagement/ManageUsers/{id}");
        }

        public IActionResult ChangeRole(ManageUsersViewModel vm, int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in as admin
                return Redirect("~/Project/Dashboard");

            var roleId = _context.Roles.First(r => r.RoleName == vm.NewRole).RoleId;

            // If changing role of current logged in user, update session to reflect new role.
            if (_context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId == id)
            {
                HttpContext.Session.SetString("Role", vm.NewRole);
            }

            // Change the role id
            _context.Users.First(u => u.UserId == id).RoleId = roleId;
            // Save the changes to the database
            _context.SaveChanges();

            // Redirect the user to the manage users view
            return Redirect($"~/UserManagement/ManageUsers/{id}");
        }

        public IActionResult DeleteUser(int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Check logged in as admin
                return Redirect("~/Project/Dashboard");

            // Get the record of the selected user
            var user = _context.Users.First(u => u.UserId == id);

            // Remove that record
            _context.Users.Remove(user);
            // Save the database
            _context.SaveChanges();

            // Kick back to the user management view
            return Redirect($"~/UserManagement/ManageUsers");
        }
    }
}