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
        static string pathUsername(string path) //supports using %username% in place of username
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

        const string homedriveVar = "%homedrive%";
        static string pathHomeDrive(string path) //supports for %homedrive%, gives the drive letter on Windows
        {
            if (path.IndexOf(homedriveVar, StringComparison.CurrentCultureIgnoreCase) == -1) //does not contain username var
            {
                Console.WriteLine("has no homedrive var"); //debug code, remove later
                return path;
            }
            else
            {
                Console.WriteLine("has homedrive var"); //debug code, remove later

                int p = (int)Environment.OSVersion.Platform;
                if ((p == 4) || (p == 6) || (p == 128)) //is Linux
                {
                    return path;
                }
                else
                {
                    string driveLetter = Environment.GetEnvironmentVariable("HOMEDRIVE");

                    return Regex.Replace(path, homedriveVar, driveLetter, RegexOptions.IgnoreCase);
                }
            }
        }

        static string pathTilde(string path) //supports ~ for home dir at beginning of path
        {
            if (path.IndexOf ("~", StringComparison.CurrentCultureIgnoreCase) == -1) //does not contain ~
            {
                Console.WriteLine("has no ~"); //debug code, remove later
                return path;
            }
            else
            {
                Console.WriteLine("has ~"); //debug code, remove later

                if (path[0].ToString() == "~")
                {
                    string homePath = "";

                    int p = (int)Environment.OSVersion.Platform;
                    if ((p == 4) || (p == 6) || (p == 128)) //is Linux
                    {
                        Console.WriteLine("Running on Unix"); //debug code, remove later
                        homePath = Environment.GetEnvironmentVariable("HOME");
                    }
                    else //is Windows
                    {
                        Console.WriteLine("NOT running on Unix"); //debug code, remove later
                        homePath = Environment.GetEnvironmentVariable("userprofile");
                    }

                    path = path.Substring(1);
                    return homePath + path;
                }
                else
                {
                    return path;
                }
            }
        }

        static string correctSlash(string path) //become someone is obsessed with perfection....
        {
            string slash;

            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128)) //is Linux
            {
                slash = "/"; //forward slash
            }
            else
            {
                slash = "\\"; //back slash
            }
            
            path = path.Replace("/", slash);
            path = path.Replace("\\", slash);

            return path;
        }

        static string computeFullPath(string path) //single function that calls the functions that help compute a full url path
        {
            return correctSlash(
                pathHomeDrive(
                    pathUsername(
                        pathTilde(path)
                    )
                )
            );
        }

        static void Main(string[] args)
        {
            //string path = "C:/Users/%username%/Desktop/simBackups";
            //string path = "C:/Users/Kevin/Desktop/simBackups";
            //string path = "~/desktop";
            string path = "%homedrive%/simBackups";

            Console.WriteLine(computeFullPath(path));
            Console.ReadLine();
        }
    }
}