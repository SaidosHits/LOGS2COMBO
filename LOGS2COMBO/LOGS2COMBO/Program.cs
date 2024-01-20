using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        banner();
        Console.Write(" [>] Enter Path root Folder: ");
        string input_path = Console.ReadLine();
        if (!Directory.Exists(input_path))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine(" Sorry the root Path is not found !");
            Console.ReadKey();
            return;
        }
        var search = Directory.GetFiles(input_path, "Passwords.txt", SearchOption.AllDirectories);
        if (search.Length <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($@" Sorry the Files Passwords.txt is not found in this Directory {input_path}");
            Console.ReadKey();
            Environment.Exit(0);
        }
        HashSet<string> Urls = new HashSet<string>();
        HashSet<string> usernames = new HashSet<string>();
        HashSet<string> passwords = new HashSet<string>();
        HashSet<string> combo_email = new HashSet<string>();
        HashSet<string> combo_user = new HashSet<string>();

        foreach (string file in search)
        {
            using (StreamReader read_file = File.OpenText(file))
            {
                string line;
                string currentUrl = "";
                string currentUsername = "";
                while ((line = read_file.ReadLine()) != null)
                {
                    if (line.Contains("URL:"))
                    {
                        currentUrl = line.Substring(5);
                    }
                    if (line.Contains("Username: "))
                    {
                        currentUsername = line.Substring(10);
                    }
                    if (line.Contains("Password: "))
                    {
                        string currentPassword = line.Substring(10);
                        // Only save if both username and password are found
                        if (!string.IsNullOrEmpty(currentUrl) && !string.IsNullOrEmpty(currentUsername) && !string.IsNullOrEmpty(currentPassword))
                        {
                            Urls.Add(currentUrl);
                            usernames.Add(currentUsername);
                            passwords.Add(currentPassword);
                        }
                        currentUrl = "";
                        currentUsername = "";
                        currentPassword = "";
                    }
                }
            }
        }

        // Combine lists and save to a single file
        string current_location = AppDomain.CurrentDomain.BaseDirectory;
        var current_date = DateTime.Now;
        string folder_name = string.Format("Resul {0:[dd.mm.yyyy] [hh.mm.ss]}", current_date);
        Directory.CreateDirectory(folder_name);
        string combo = Path.Combine(folder_name, "combo.txt");
        string combo_users = Path.Combine(folder_name, "combo_usernames.txt");
        using (StreamWriter com_users = new StreamWriter(combo_users))
        using (StreamWriter write = new StreamWriter(combo))
        {
            foreach (var combinedData in Urls.Zip(usernames, (url, username) => new { Url = url, Username = username })
                                            .Zip(passwords, (data, password) => new { data.Url, data.Username, Password = password }))
            {
                if (combinedData.Username.Contains("@"))
                {
                    var combo_emai = $"{combinedData.Username}:{combinedData.Password}";
                    write.WriteLine(combo_emai);
                }
                else
                {
                    var combo_us = $"{combinedData.Username}:{combinedData.Password}";
                    com_users.WriteLine(combo_us);
                }
            }
        }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("");
        Console.WriteLine(" [>] Done !");
        Console.ReadKey();
    }

    static void banner()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"");
        Console.WriteLine("    _       ___    ____  _____ ______   ___     __   ___   ___ ___  ____    ___  ");
        Console.WriteLine("   | |     /   \\  /    |/ ___/|      | /   \\   /  ] /   \\ |   |   ||    \\  /   \\ ");
        Console.WriteLine("   | |    |     ||   __(   \\_ |      ||     | /  / |     || _   _ ||  o  )|     |");
        Console.WriteLine("   | |___ |  O  ||  |  |\\__  ||_|  |_||  O  |/  /  |  O  ||  \\_/  ||     ||  O  |");
        Console.WriteLine("   |     ||     ||  |_ |/  \\ |  |  |  |     /   \\_ |     ||   |   ||  O  ||     |");
        Console.WriteLine("   |     ||     ||     |\\    |  |  |  |     \\     ||     ||   |   ||     ||     |");
        Console.WriteLine("   |_____| \\___/ |___,_| \\___|  |__|   \\___/ \\____| \\___/ |___|___||_____| \\___/ ");
        Console.WriteLine(@"                                    @SaidosHits                            ");
        Console.WriteLine(@"");
        Console.WriteLine(@"");

    }
}
