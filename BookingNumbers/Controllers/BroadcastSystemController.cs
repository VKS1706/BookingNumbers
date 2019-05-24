using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BookingNumbers.Models;
using BookingNumbers.Tools;
using DataCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingNumbers.Controllers
{
    // This controller deals with the broadcasts
    public class BroadcastSystemController : Controller
    {
        private BookingDBContext _context;
        private readonly AuthoriseUser _auth;

        public BroadcastSystemController(BookingDBContext context, AuthoriseUser auth)
        {
            _context = context; // Database context object
            _auth = auth; // Authentication service
        }

        // List the broadcasts to the user
        public IActionResult List()
        {

            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                Redirect("~/Project/Dashboard");

            var vm = new BroadcastListViewModel(); // Get a new view model

            var authoredList = new List<AuthoredBroadcast>(); // Instanciate the authored list
            // Go through each broadcast
            foreach (var broadcast in _context.Broadcasts)
            {
                // Create a new authored broadcast
                var temp = new AuthoredBroadcast()
                {
                    Broadcast = broadcast
                };

                // Update username field
                temp.Username = _context.Users.Any(u => u.UserId == broadcast.UserId)
                    ? _context.Users.First(u => u.UserId == broadcast.UserId).UserName
                    : "Unknown User";
                
                // Add the new broadcast to the authored list
                authoredList.Add(temp);
            }

            // Add the authored list to the view model
            vm.BroadcastsList = authoredList;

            // Return the List view
            return View(vm);
        }

        // Publish a new broadcast or modify a broadcast
        public IActionResult Publish()
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                Redirect("~/Project/Dashboard");

            var vm = new PublishBroadcastViewModel(); // New view model

            // Return the publish view
            return View(vm);
        }
        
        // Action to delete a broadcast, gets the id from the url
        public IActionResult DeleteBroadcast(int id)
        {

            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                Redirect("~/Project/Dashboard");

            _context.Broadcasts.Remove(_context.Broadcasts.First(i => i.BroadcastId == id)); // Remove the broadcast with the correct id

            _context.SaveChanges(); // Save the changes to the database

            // Redirect back to the list view
            return RedirectToAction("List");
        }

        // Post action, Action to add a broadcast
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddBroadcast(PublishBroadcastViewModel vm)
        {

            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                Redirect("~/Project/Dashboard");

            if (!_context.Broadcasts.Any(b => b.BroadcastId == vm.UpdateId)) // If there isn't a broadcast with that id
                return RedirectToAction("Publish"); // Kick back to publish page

            // If The update id is null
            if (vm.UpdateId == null)
            {
                // Create new broadcast
                var userId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;

                // create a new broadcast
                var newBroadcast = new Broadcasts()
                {
                    Body = vm.Body,
                    Title = vm.Title,
                    UserId = userId
                };

                // add the new broadcast to the database
                _context.Broadcasts.Add(newBroadcast);
            }
            else
            {
                // Update existing broadcast

                if (vm.Title != null)
                    _context.Broadcasts.First(i => i.BroadcastId == vm.UpdateId).Title = vm.Title;

                if (vm.Body != null)
                    _context.Broadcasts.First(i => i.BroadcastId == vm.UpdateId).Body = vm.Body;

                _context.Broadcasts.First(i => i.BroadcastId == vm.UpdateId).UserId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;
            }
                
            // Save changes to the database
            _context.SaveChanges();

            // Return to the list view
            return RedirectToAction("List");
        }

    }
}