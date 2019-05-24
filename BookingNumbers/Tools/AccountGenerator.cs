using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Models;

namespace BookingNumbers.Tools
{
    // This generates accounts and stores them in the database
    public class AccountGenerator
    {
        public static void GenerateAccount(BookingDBContext context, Account newAccount)
        {
            // Create a new user record
            var newUser = new Users();

            // Generate a new salt
            var salt = Salter.Shake();

            // Hash the password and the salt
            var hashedPass = Hasher.Hash(newAccount.Password + salt);

            // Get the role id from the database
            var roleId = context.Roles.First(r => r.RoleName == newAccount.Role).RoleId;

            // Fill in the fields
            newUser.UserName = newAccount.Username;
            newUser.HashedPassword = hashedPass;
            newUser.Salt = salt;
            newUser.RoleId = roleId;

            if (newAccount.Email != null)
            {
                newUser.Email = newAccount.Email;
            }

            // Add the user to the database
            context.Users.Add(newUser);

            // Save to the database
            context.SaveChanges();

        }
    }
}
