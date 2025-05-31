using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gp_unisis.Helpers
{
    class MD5Helper
    {
        public static string CreateMD5(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes).ToLower(); // .NET 5 +
            }
        }
    }
}
