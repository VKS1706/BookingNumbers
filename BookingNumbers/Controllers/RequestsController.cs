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
    // This controller handles the requests
    public class RequestsController : Controller
    {
        private BookingDBContext _context;
        private readonly AuthoriseUser _auth;

        public RequestsController(BookingDBContext context, AuthoriseUser auth)
        {
            _context = context; // The database context object
            _auth = auth; // Authentication service
        }

        // Create a new request view
        public IActionResult CreateRequest()
        {
            var vm = new CreateRequestViewModel(); // Create new view model
            vm.RequestType = _context.RequestTypes.Where(r => r.RequestTypeId > -1).Select(r => r.RequestName).ToList(); // Get the request type from the database

            // Return the view
            return View(vm);
        }

        // Create request post function
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CreateRequest(CreateRequestViewModel vm)
        {
            // Check it's valid
            if (vm.SelectedType == "Choose Request Type" || string.IsNullOrEmpty(vm.Reason))
            {
                return RedirectToAction("CreateRequest");
            }

            // Create a new admin request
            var newReq = new AdminRequests();
            newReq.RequestDescription = vm.Reason;
            newReq.SentByUserId = _context.Users.First(u => u.UserName == HttpContext.Session.GetString("Username")).UserId;
            newReq.Responded = false;
            newReq.DateRequested = DateTime.Now;
            newReq.RequestTypeId = _context.RequestTypes.First(r => r.RequestName == vm.SelectedType).RequestTypeId;

            // Add it to database
            _context.AdminRequests.Add(newReq);

            // Save database
            _context.SaveChanges();

            // Redirect to dashboard
            return Redirect("~/Project/Dashboard");
        }
    }
}