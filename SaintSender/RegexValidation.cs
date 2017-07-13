using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SaintSender
{
    class RegexValidation
    {
        public static bool IsItEmail(string emailAddress)
        {
            return Regex.IsMatch(emailAddress, @"[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]+");
        }
    }
}
