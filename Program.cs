using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace pathUsername
{
    class Program
    {
        static string pathUsername(string path)
        {
            string userName = Environment.UserName;
            string value = Regex.Replace(path, "%username%", userName, RegexOptions.IgnoreCase);

            return value;
        }

        static void Main(string[] args)
        {
            string path = "C:/Users/%username%/Desktop/simBackups";
            Console.WriteLine(pathUsername(path));
            Console.ReadLine();
        }
    }
}
