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
    // This controller deals with admin pages, mainly the mailbox
    public class AdminController : Controller
    {
        private readonly AuthoriseUser _auth;
        private readonly BookingDBContext _context;

        public AdminController(AuthoriseUser auth, BookingDBContext context)
        {
            _auth = auth; // Authorisation service
            _context = context; // Database
        }

        // Mailbox user
        public IActionResult Mailbox()
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authorise the user is an admin
                return Redirect("~/Project/Dashboard");

            // Get a new View model
            var vm = new MailboxViewModel();

            // Instanciate the mail list
            vm.MailList = new List<FormattedRequest>();

            // Go through every request in admin requests
            foreach (var i in _context.AdminRequests.Where(r => r.RequestId > 0))
            {
                // Create a new formatted request and add all the fields
                var temp = new FormattedRequest();
                temp.Name = _context.Users.First(u => u.UserId == i.SentByUserId).UserName;
                temp.Description = i.RequestDescription;
                temp.Responded = i.Responded;
                temp.Response = i.Response;
                temp.DateRequested = i.DateRequested;
                temp.DateResponded = i.DateResponded;
                temp.RequestType = _context.RequestTypes.First(u => u.RequestTypeId == i.RequestTypeId).RequestName;
                temp.RequestId = i.RequestId;

                // Add the new formatted request to the view model
                vm.MailList.Add(temp);
            }

            // Return the view model
            return View(vm);
        }

        // Function to accept a request
        public IActionResult AcceptReq(int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            // Check the request Id exists
            if (!_context.AdminRequests.Any(r => r.RequestId == id))
            {
                return RedirectToAction("MailBox");
            }

            // Get the record and update values
            var rec = _context.AdminRequests.First(r => r.RequestId == id);
            rec.Responded = true;
            rec.RespondedByUserId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;
            rec.DateResponded = DateTime.Now;
            rec.Response = true;

            // Save to the database
            _context.SaveChanges();

            // Redirect back to the mailbox
            return RedirectToAction("Mailbox");
        }

        // Decline a request
        public IActionResult DeclineReq(int id)
        {
            if (!_auth.Authorise(RolesEnum.Admin, _context)) // Authenticate the user
                return Redirect("~/Project/Dashboard");

            // Check the request exists
            if (!_context.AdminRequests.Any(r => r.RequestId == id))
            {
                return RedirectToAction("MailBox");
            }

            // Get the record with the same id and update the values
            var rec = _context.AdminRequests.First(r => r.RequestId == id);
            rec.Responded = true;
            rec.RespondedByUserId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;
            rec.DateResponded = DateTime.Now;
            rec.Response = false;

            // Save the changes to the database
            _context.SaveChanges();

            // Redirect back to the mailbox 
            return RedirectToAction("Mailbox");
        }


    }
}