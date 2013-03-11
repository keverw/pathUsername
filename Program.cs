using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;

namespace pathUsername
{
    class Program
    {
        const string usernameVar = "%username%";
        static string pathUsername(string path)
        {
            if (path.IndexOf(usernameVar, StringComparison.CurrentCultureIgnoreCase) == -1) //does not contain username var
            {
                Console.WriteLine("has no username var"); //debug code, remove later
                return path;
            }
            else //contains username var
            {
                Console.WriteLine("has username var"); //debug code, remove later
                string userName = Environment.UserName; //check system for current username
                return Regex.Replace(path, usernameVar, userName, RegexOptions.IgnoreCase); //return path with the system username
            }
        }

        static void Main(string[] args)
        {
            string path = "C:/Users/%username%/Desktop/simBackups";
            //string path = "C:/Users/Kevin/Desktop/simBackups";
            Console.WriteLine(pathUsername(path));
            Console.ReadLine();
        }
    }
}
