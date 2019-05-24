using System;
using System.Linq;
using DataCore.Models;
using Microsoft.AspNetCore.Http;

namespace BookingNumbers.Tools
{
    // Authorises the user for access
    public class AuthoriseUser : IServiceProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public AuthoriseUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor; // Tool to access the http context to get the session
        }

        // Code that is run when it gets dependency injected.
        public object GetService(Type serviceType)
        {
            return new AuthoriseUser(_httpContextAccessor);
        }

        // Authorise a user
        public bool Authorise(RolesEnum role, BookingDBContext context)
        {
            // If there isn't a role
            if (_httpContextAccessor.HttpContext.Session.GetString("Role") == null || _httpContextAccessor.HttpContext.Session.GetString("Role") == "")
            {
                return false;
            }

            // Get the current role from the session
            var currentRole = _httpContextAccessor.HttpContext.Session.GetString("Role");

            // Convert the role enum to and int and check if it's higher or equal than the minimum
            if ((int)role >= context.Roles.First(n => n.RoleName == currentRole).RoleId)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
