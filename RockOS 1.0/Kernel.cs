using System;
using System.IO;
using Cosmos.System.FileSystem;
using system1._0RockOS;
using Sys = Cosmos.System;

namespace system1._0RockOS
{
    public class Kernel : Sys.Kernel
    {
        public static string DataOS = "21.12.2024";
        public static string Version = "1.0.0";
        public static string Architecture = "x86_64";
        public static string Path = @"0:\";
        public static CosmosVFS fs;
        public static string username, password;
        public static bool loginOnOff = false;
        public static int WindowSizeY = 90, WindowSizeX = 30;

        public static string Verson { get; internal set; }
        public static string Architektura { get; internal set; }

        protected override void BeforeRun()
        {
            Console.SetWindowSize(90, 30);
            Console.OutputEncoding = Cosmos.System.ExtendedASCII.CosmosEncodingProvider.Instance.GetEncoding(437);
            fs = new Cosmos.System.FileSystem.CosmosVFS();
            Cosmos.System.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Booting RockOS " + Version);
            Console.ForegroundColor = ConsoleColor.White;

            string accountFilePath = @"0:\account.txt";
            if (File.Exists(accountFilePath))
            {
                if (loginOnOff == true)
                {
                    login();
                }
            }
            else
            {
                Console.WriteLine("No account found. Please create an account first using the 'login' command.");
            }
        }

        protected override void Run()
        {
            Console.Write(Path + ">");
            var command = Console.ReadLine();
            ConsoleCommands.RunCommand(command);
            Console.ForegroundColor = ConsoleColor.White;
        }

        void login()
        {
            string accountFilePath = @"0:\account.txt";

            while (true)
            {
                var accountData = File.ReadAllText(accountFilePath).Split(',');

                Console.Write("Username: ");
                var inputUsername = Console.ReadLine();

                Console.Write("Password: ");
                var inputPassword = Console.ReadLine();

                if (inputUsername == accountData[0] && inputPassword == accountData[1])
                {
                    username = inputUsername;
                    password = inputPassword;
                    loginOnOff = true;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully logged in!");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                }
                else
                {
                    loginOnOff = false;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect username or password. Please try again.");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}