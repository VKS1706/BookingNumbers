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
    // Controller that handles everything to do with displaying user details, mostly on the user page page
    public class DetailsController : Controller
    {
        private BookingDBContext _context;
        private readonly AuthoriseUser _auth;

        public DetailsController(BookingDBContext context, AuthoriseUser auth)
        {
            _context = context; // Database 
            _auth = auth; // Authentication service
        }

        // Page shows all the users details
        public IActionResult UserPage()
        {
            if (!_auth.Authorise(RolesEnum.Staff, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            // Create a new view model
            var vm = new ProfileViewModel();
            
            // Instanciate the projects list
             vm.Projects = new List<FormattedProject>();

            // Get the userId
            var userId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;

            // Get the users email.
            vm.Email = _context.Users.First(u => u.UserId == userId).Email ?? "User has not given an email.";


            // Get all the project Ids the user is in
            foreach (var projId in _context.ProjectUsers.Where(p => p.UserId == userId).Select(p => p.ProjectId))
            {
                // Get the project
                var proj = _context.Projects.First(p => p.ProjectId == projId);

                // Create a new formatted project
                var temp = new FormattedProject();
                temp.ProjectId = projId;
                temp.Description = proj.ProjectDescription;
                temp.PercentageComplete = (int)(((double) proj.CurrentUsedMinutes / (double) proj.MaximumMinutes) * 100);
                temp.ProjectName = proj.ProjectName;
                temp.LastActivity = _context.ProjectMinutesBooked.Any(p => p.ProjectId == projId) ? _context.ProjectMinutesBooked.Last(p => p.ProjectId == projId).DateOfBooking
                                        .ToLongDateString() : "This project has not had any bookings.";

                // Add the new project to the view model
                vm.Projects.Add(temp);
            }

            // Create a new list for bookings
            vm.Bookings = new List<FormattedBooking>();

            // Get all of the users bookings
            var listOfAllBookings = _context.ProjectMinutesBooked.Where(u => u.UserId == userId).ToList();
            int amountOfBookings; // However many bookings that're gonna be displayed

            // If there's less than 4
            if (listOfAllBookings.Count() < 4)
            {
                // List out all of the bookings the user has
                amountOfBookings = listOfAllBookings.Count();
            }
            else
            {
                // Only display 3 bookings
                amountOfBookings = 3;
            }

            // For however many bookings are to be displayed
            for (var i = 0; i < amountOfBookings; i++)
            {
                // Create a new formatted booking and fill out the fields
                var temp = new FormattedBooking();
                temp.DateBooked = listOfAllBookings[listOfAllBookings.Count - (i + 1)].DateOfBooking;
                var totalMinutes = listOfAllBookings[listOfAllBookings.Count - (i + 1)].AmountOfMinutes;

                var minutesBooked = totalMinutes % 60;
                var HoursBooked = (totalMinutes - minutesBooked) / 60;

                temp.MinutesBooked = minutesBooked;
                temp.HoursBooked = HoursBooked;
                temp.ProjectBookedTo = _context.Projects
                    .First(p => p.ProjectId == listOfAllBookings[listOfAllBookings.Count - (i + 1)].ProjectId).ProjectName;

                // Add the booking to the view model
                vm.Bookings.Add(temp);
            }


            // Return the user page view
            return View(vm);
        }

    }
}