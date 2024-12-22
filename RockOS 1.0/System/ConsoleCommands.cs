using System;
using System.IO;
using System.Threading;

namespace system1._0RockOS
{
    public static class ConsoleCommands
    {
        private static string accountFilePath;
        public static void RunCommand(string command)
        {
            string[] words = command.Split(' ');
            if (words.Length > 0)
            {
                if (words[0] == "info")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteMessage.WriteLogo();
                    Console.WriteLine("RockOS " + Kernel.Version);
                    Console.WriteLine("Build " + Kernel.DataOS);
                    Console.WriteLine("Architektura " + Kernel.Architektura);
                    Console.WriteLine("Created by Maksymilian wedrowski ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (words[0] == "format")
                {
                    if (Kernel.fs.Disks[0].Partitions.Count > 0)
                    {
                        Kernel.fs.Disks[0].DeletePartition(0);
                    }
                    Kernel.fs.Disks[0].Clear();
                    Kernel.fs.Disks[0].CreatePartition(Kernel.fs.Disks[0].Size / (1024 * 124));
                    Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
                    WriteMessage.WriteOK("Sucess!");
                    WriteMessage.WriteWarn("RockOS will reboot in 3 seconds!");
                    Thread.Sleep(3000);
                    Cosmos.System.Power.Reboot();
                }
                else if (words[0] == "space")
                {
                    long free = Kernel.fs.GetAvailableFreeSpace(Kernel.Path);
                    Console.WriteLine("Free space: " + free / 1024 + "KB");
                }
                else if (words[0] == "dir")
                {
                    var Directories = Directory.GetDirectories(Kernel.Path);
                    var Files = Directory.GetFiles(Kernel.Path);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Directories (" + Directories.Length + ")");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    for (int i = 0; i < Directories.Length; i++)
                    {
                        Console.WriteLine(Directories[i]);
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Files (" + Files.Length + ")");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    for (int i = 0; i < Files.Length; i++)
                    {
                        Console.WriteLine(Files[i]);
                    }
                }
                else if (words[0] == "echo")
                {
                    if (words.Length > 1)
                    {
                        string wholwSreing = "";
                        for (int i = 1; i < words.Length; i++)
                        {
                            wholwSreing += words[i] + " ";
                        }
                        int pathIndex = wholwSreing.LastIndexOf('>');
                        string text = wholwSreing.Substring(0, pathIndex);
                        string path = wholwSreing.Substring(pathIndex + 1);
                        if (!path.Contains(@"\"))
                            path = Kernel.Path + path;
                        if (path.EndsWith(' '))
                        {
                            path = path.Substring(0, path.Length - 1);
                        }
                        var file_stream = File.Create(path);
                        file_stream.Close();
                        File.WriteAllText(path, text);
                    }
                    else
                        WriteMessage.WriteError("Invalid syntax!");
                }
                else if (words[0] == "cat")
                {
                    if (words.Length > 1)
                    {
                        string path = words[1];
                        if (!path.Contains(@"\"))
                            path = Kernel.Path + path;
                        if (path.EndsWith(' '))
                        {
                            path = path.Substring(0, path.Length - 1);
                        }
                        if (File.Exists(path))
                        {
                            string text = File.ReadAllText(path);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine(text);
                        }
                        else
                            WriteMessage.WriteError("Fils " + path + " not found!");
                    }
                    else
                        WriteMessage.WriteError("Invalid syntax!");
                }
                else if (words[0] == "del")
                {
                    if (words.Length > 1)
                    {
                        string path = words[1];
                        if (!path.Contains(@"\"))
                            path = Kernel.Path + path;
                        if (path.EndsWith(' '))
                        {
                            path = path.Substring(0, path.Length - 1);
                        }
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            WriteMessage.WriteOK("Daleted " + path + "!");
                        }
                        else
                            WriteMessage.WriteError("Fils " + path + "not found!");
                    }
                    else
                        WriteMessage.WriteError("Invalid syntax!");
                }
                else if (words[0] == "mkdir")
                {
                    if (words.Length > 1)
                    {
                        string path = words[1];
                        if (!path.Contains(@"\"))
                            path = Kernel.Path + path;
                        if (path.EndsWith(' '))
                        {
                            path = path.Substring(0, path.Length - 1);
                        }
                        Directory.CreateDirectory(path);
                    }
                    else
                        WriteMessage.WriteError("Invalid syntax!");
                }
                else if (words[0] == "cd")
                {
                    if (words.Length > 1)
                    {
                        if (words[1] == "..")
                        {
                            if (Kernel.Path != @"0:\")
                            {
                                string tempPath = Kernel.Path.Substring(0, Kernel.Path.Length - 1);
                                Kernel.Path = tempPath.Substring(0, tempPath.LastIndexOf(@"\") + 1);
                                return;
                            }
                            else
                                return;
                        }
                        string path = words[1];
                        if (!path.Contains(@"\"))
                            path = Kernel.Path + path + @"\";
                        if (path.EndsWith(' '))
                        {
                            path = path.Substring(0, path.Length - 1);
                        }
                        if (!path.EndsWith(@"\"))
                            path += @"\";
                        if (Directory.Exists(path))
                            Kernel.Path = path;
                        else
                            WriteMessage.WriteError("Directory " + path + " not found!");
                    }
                    else
                        Kernel.Path = @"0:\";
                }
                else if (words[0] == "shutdown")
                {
                    WriteMessage.WriteWarn("RockOS will Shutdown in 2 seconds!");
                    Thread.Sleep(2000);
                    Cosmos.System.Power.Shutdown();
                }
                else if (words[0] == "reboot")
                {
                    WriteMessage.WriteWarn("RockOS will restart in 2 seconds!");
                    Thread.Sleep(2000);
                    Cosmos.System.Power.Reboot();
                }
                else if (words[0] == "clear")
                {
                    WriteMessage.WriteWarn("The screen will clear in 2 seconds!");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Booting RockOS " + Kernel.Verson);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (words[0] == "account")
                {
                    try
                    {
                        if (Kernel.loginOnOff)
                        {
                            if (words.Length > 1)
                            {
                                string option = words[1].ToLower();

                                if (option == "info")
                                {
                                    Console.WriteLine("Account Information:");
                                    Console.WriteLine($"Username: {Kernel.username}");
                                    Console.WriteLine("Password: ********");
                                }
                                else if (option == "passwd")
                                {
                                    Console.Write("Enter current password: ");
                                    string currentPassword = Console.ReadLine();

                                    if (currentPassword == Kernel.password)
                                    {
                                        Console.Write("Enter new password: ");
                                        string newPassword = Console.ReadLine();

                                        Console.Write("Confirm new password: ");
                                        string confirmPassword = Console.ReadLine();

                                        if (newPassword == confirmPassword)
                                        {
                                            Kernel.password = newPassword;

                                            // Zaktualizuj plik
                                            string accountData = $"{Kernel.username},{Kernel.password}";
                                            File.WriteAllText(@"0:\account.txt", accountData);

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Password changed successfully.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Passwords do not match.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Incorrect current password.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                else if (option == "delete") // Usuń konto
                                {
                                    Console.Write("Are you sure you want to delete your account? (yes/no): ");
                                    string confirmation = Console.ReadLine();

                                    if (confirmation.ToLower() == "yes")
                                    {
                                        if (File.Exists(@"0:\account.txt"))
                                        {
                                            File.Delete(@"0:\account.txt");

                                            Kernel.username = null;
                                            Kernel.password = null;
                                            Kernel.loginOnOff = false;

                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Account deleted successfully.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                        else
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("Account file not found.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Account deletion cancelled.");
                                    }
                                }
                                else
                                {
                                    WriteMessage.WriteError("Invalid option. Available options: info, changepassword, delete");
                                }
                            }
                            else
                            {
                                WriteMessage.WriteError("Invalid syntax! Usage: account [info|changepassword|delete]");
                            }
                        }
                        else
                        {
                            WriteMessage.WriteError("You must be logged in to manage your account.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error in 'account' command: {ex.Message}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if (words[0] == "login")
                {
                    string accountFilePath = @"0:\account.txt";

                    if (!File.Exists(accountFilePath))
                    {
                        Console.Write("Choose a username: ");
                        var username = Console.ReadLine();

                        Console.Write("Choose a password: ");
                        var password = Console.ReadLine();

                        var accountData = username + "," + password;
                        File.WriteAllText(accountFilePath, accountData);

                        Kernel.username = username;
                        Kernel.password = password;

                        Kernel.loginOnOff = true;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Account created successfully. Please log in with your username and password.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write("Username: ");
                        var username = Console.ReadLine();

                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        var accountData = File.ReadAllText(accountFilePath).Split(',');

                        if (username == accountData[0] && password == accountData[1])
                        {
                            Kernel.username = username;
                            Kernel.password = password;
                            Kernel.loginOnOff = true;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Login successful!");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Kernel.loginOnOff = false;

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Incorrect username or password. Please try again.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                else if (words[0] == "accountInfo")
                {
                    Console.WriteLine("Account information:");
                    Console.WriteLine($"Username: {Kernel.username}");
                    Console.WriteLine("Password: ********");
                }
                else if (words[0] == "passwd")
                {
                    // Change password
                    Console.Write("Enter the new password: ");
                    string newPassword = Console.ReadLine();

                    Console.Write("Confirm the new password: ");
                    string confirmPassword = Console.ReadLine();

                    if (newPassword == confirmPassword)
                    {
                        Kernel.password = newPassword;

                        // Update the data in the file
                        string accountData = $"{Kernel.username},{Kernel.password}";
                        File.WriteAllText(@"0:\account.txt", accountData);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("The password has been successfully changed.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The passwords do not match.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if (words[0] == "name") // Add new option to change username
                {
                    Console.Write("Enter the new username: ");
                    string newName = Console.ReadLine();

                    if (!string.IsNullOrEmpty(newName))
                    {
                        Kernel.username = newName;

                        // Update the data in the file
                        string accountData = $"{Kernel.username},{Kernel.password}";
                        File.WriteAllText(@"0:\account.txt", accountData);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("The username has been successfully changed.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("The username cannot be empty.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                else if (words[0] == "accountDelete") // Delete account
                {
                    Console.Write("Are you sure you want to delete your account? (yes/no): ");
                    string confirmation = Console.ReadLine();

                    if (confirmation.ToLower() == "yes")
                    {
                        if (File.Exists(@"0:\account.txt"))
                        {
                            File.Delete(@"0:\account.txt");

                            Kernel.username = null;
                            Kernel.password = null;
                            Kernel.loginOnOff = false;

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("The account has been successfully deleted.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Account file not found.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Account deletion has been canceled.");
                    }
                }
                
                else if (words[0] == "help")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Dostepne komendy:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("info      - Wyswietla informacje o systemie (wersja OS i autor)");
                    Console.WriteLine("format    - Formatuje pierwszy dysk na partycje FAT32");
                    Console.WriteLine("space     - Wyswietla dostepna przestrzen na dysku");
                    Console.WriteLine("dir       - Wyswietla katalogi i pliki w biezacej sciezce");
                    Console.WriteLine("echo      - Wypisuje tekst do pliku. Skladnia: echo [tekst] > [sciezka_pliku]");
                    Console.WriteLine("cat       - Wyswietla zawartosc pliku");
                    Console.WriteLine("del       - Usuwa plik");
                    Console.WriteLine("reboot    - restartuje system");
                    Console.WriteLine("shutdown  - wychodzisz z systemu");
                    Console.WriteLine("clear     - wyczyszcza ekran");
                    Console.WriteLine("mkdir     - dodawanie folderuw");
                    Console.WriteLine("cd        - wchodzis do foldera lub wychodzienia folderuw");
                    Console.WriteLine("login     - stwozy się konto");
                    Console.WriteLine("account   - ustawienia konta");
                    Console.WriteLine("passwd    - zmiana hasla");
                    Console.WriteLine("kontoInfo - informacje o koncie");
                    Console.WriteLine("name      - zmiana imienia");
                    Console.WriteLine("kontoDelete - usuwanie konta");
                }
                else if (words[0] == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("czemó");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    WriteMessage.WriteError("Unknown command.");
                }
            }
            else
            {
            }
        }
    }
}