using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Models;

namespace BookingNumbers.Tools
{
    // This class validates the passwords to the username
    public class PassValidator
    {
        public static bool ValidatePassword(BookingDBContext context, string username, string password)
        {
            // Get the user id from the database
            var userId = GetUserId(context, username);

            // Check there's a valid user id
            if (userId == -1)
                return false;

            // Get the hashed password from the database
            var hashedPass = GetHashedPass(context, userId);

            // Get the salt from the database
            var salt = GetSalt(context, userId);

            // Hash what the user input
            var newHashedPass = Hasher.Hash(password + salt);

            // If the passwords match
            if (newHashedPass == hashedPass)
            {
                return true;
            }

            return false;
        }

        // Gets the userid from the database from the username
        private static int GetUserId(BookingDBContext context, string username)
        {
            // Get all the users from the database
            var allUsers = context.Users.ToList();

            // If there aren't any users with that user id, return -1
            if (!context.Users.Any(u => u.UserName == username))
                return -1;

            // Get the user record
            var user = allUsers.First(u => u.UserName == username);

            // Return the user id
            return user.UserId;
        }

        // Gets the salt from the database for the user
        private static string GetSalt(BookingDBContext context, int userId)
        {
            // Get all the users
            var allUsers = context.Users.ToList();

            // Get the user record
            var user = allUsers.First(u => u.UserId == userId);

            // Return the salt
            return user.Salt;
        }

        // This gets the hashed password for a user from the database
        private static string GetHashedPass(BookingDBContext context, int userId)
        {
            // Get all the users
            var allUsers = context.Users.ToList();

            // Get the user record
            var user = allUsers.First(u => u.UserId == userId);

            // Return the hashed password for that user
            return user.HashedPassword;
        }
    }
}
