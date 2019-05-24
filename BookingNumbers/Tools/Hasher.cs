using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookingNumbers.Tools
{
    // This hashes strings and then returns it
    public static class Hasher
    {
        public static string Hash(string value)
        {
            using (SHA256 sha256hash = SHA256.Create()) // Generate a hashing object
            {
                byte[] bytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(value)); // Get the hash in an array of bytes

                StringBuilder stringBuilder = new StringBuilder(); // Create a string builder

                for (int i = 0; i < bytes.Length; i++) // Go through each item in the array and put it in the string
                {
                    stringBuilder.Append(bytes[i].ToString("x2")); // Convert the binary byte to a string
                }

                return stringBuilder.ToString(); // Return the string from the string builder
            }
        }
    }
}
