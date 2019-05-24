using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace BookingNumbers.Tools
{
    // Generates a salt
    public class Salter
    {
        public static string Shake()
        {
            var SaltByteLength = 15; // How many bytes you want the salt to be
            RNGCryptoServiceProvider rncCsp = new RNGCryptoServiceProvider(); // Random salt generator
            byte[] salt = new byte[SaltByteLength]; // Instanciate salt
            rncCsp.GetBytes(salt); // Generate salt

            StringBuilder builder = new StringBuilder(); // Create a new string builder
            for (int i = 0; i < salt.Length; i++)
            {
                builder.Append(salt[i].ToString("x2")); // Convert the bytes from binary to ASCII
            }

            return builder.ToString(); // Return the salt as a string
        }
    }
}
