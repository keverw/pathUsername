using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;


namespace PathUsername
{
    class Program
    {
        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        const string usernameVar = "%username%";
        static string PathUsername(string Path) //supports using %username% in place of username
        {
            if (Path.IndexOf(usernameVar, StringComparison.CurrentCultureIgnoreCase) == -1) //does not contain username var
            {
                Console.WriteLine("has no username var"); //debug code, remove later
                return Path;
            }
            else //contains username var
            {
                Console.WriteLine("has username var"); //debug code, remove later
                string userName = Environment.UserName; //check system for current username
                return Regex.Replace(Path, usernameVar, userName, RegexOptions.IgnoreCase); //return Path with the system username
            }
        }

        const string HomedriveVar = "%homedrive%";
        static string PathHomeDrive(string Path) //supports for %homedrive%, gives the drive letter on Windows
        {
            if (Path.IndexOf(HomedriveVar, StringComparison.CurrentCultureIgnoreCase) == -1) //does not contain username var
            {
                Console.WriteLine("has no homedrive var"); //debug code, remove later
                return Path;
            }
            else
            {
                Console.WriteLine("has homedrive var"); //debug code, remove later
				
                if (IsLinux) //is Linux
                {
                    return Path;
                }
                else
                {
                    string DriveLetter = Environment.GetEnvironmentVariable("HOMEDRIVE");

                    return Regex.Replace(Path, HomedriveVar, DriveLetter, RegexOptions.IgnoreCase);
                }
            }
        }

        static string PathTilde(string Path) //supports ~ for home dir at beginning of Path
        {
            if (Path.IndexOf ("~", StringComparison.CurrentCultureIgnoreCase) == -1) //does not contain ~
            {
                Console.WriteLine("has no ~"); //debug code, remove later
                return Path;
            }
            else
            {
                Console.WriteLine("has ~"); //debug code, remove later

                if (Path[0].ToString() == "~")
                {
                    string homePath = "";
					
                    if (IsLinux) //is Linux
                    {
                        Console.WriteLine("Running on Unix"); //debug code, remove later
                        homePath = Environment.GetEnvironmentVariable("HOME");
                    }
                    else //is Windows
                    {
                        Console.WriteLine("NOT running on Unix"); //debug code, remove later
                        homePath = Environment.GetEnvironmentVariable("userprofile");
                    }

                    Path = Path.Substring(1);
                    return homePath + Path;
                }
                else
                {
                    return Path;
                }
            }
        }

        static string CorrectSlash(string Path) //become someone is obsessed with perfection....
        {
            string slash;
			
            if (IsLinux) //is Linux
            {
                slash = "/"; //forward slash
            }
            else
            {
                slash = "\\"; //back slash
            }
            
            Path = Path.Replace("/", slash);
            Path = Path.Replace("\\", slash);

            return Path;
        }

        static string ComputeFullPath(string Path) //single function that calls the functions that help compute a full url Path
        {
            return CorrectSlash(
                PathHomeDrive(
                    PathUsername(
                        PathTilde(Path)
                    )
                )
            );
        }

        static void Main(string[] args)
        {
            //string Path = "C:/Users/%username%/Desktop/simBackups";
            //string Path = "C:/Users/Kevin/Desktop/simBackups";
            string Path = "~/desktop";
            //string Path = "%homedrive%/simBackups";

            Console.WriteLine(ComputeFullPath(Path));
            Console.ReadLine();
        }
    }
}