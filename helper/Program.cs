using System.Text;
using System.Diagnostics;
using System.Net;
using System.Security.Principal;
using SixLabors.ImageSharp;
using System.Net.NetworkInformation;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Management;


class Helper
{

    //1-2
    public static int get_screen_width()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
        foreach (ManagementObject obj in searcher.Get())
        {
            return Convert.ToInt32(obj["CurrentHorizontalResolution"]);
        }
        return -1;
    }

    public static int get_screen_height()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
        foreach (ManagementObject obj in searcher.Get())
        {
            return Convert.ToInt32(obj["CurrentVerticalResolution"]);
        }
        return -1;
    }

    //3-4

    public static string encode_base64(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        byte[] bytes_encode = Encoding.UTF8.GetBytes(text); //Converts the string to a UTF-8 byte array
        string base64_encoded_text = Convert.ToBase64String(bytes_encode); //Encode in base64
        return base64_encoded_text;
    }

    public static string decode_base64(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        byte[] bytes_decode = Convert.FromBase64String(text); //Decode from base64 to bytes
        string base64_decoded_text = Encoding.UTF8.GetString(bytes_decode); //Convert the bytes to a UTF-8 string
        return base64_decoded_text;
    }

    //5
    public static string clean_string(string message)
    {
        StringBuilder clean_message = new StringBuilder();
        string all_chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 .,{}[]-_=/*+.,'\"";

        foreach (char c in message)
        {
            if (all_chars.Contains(c.ToString()))
            {
                clean_message.Append(c);
            }
        }

        return clean_message.ToString();
    }


    //6
    public static string send_power_shell_command(string command)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = command,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        return output;
    }

    //7
    public static string send_cmd_command(string command_cmd)
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        process.StandardInput.WriteLine(command_cmd);
        process.StandardInput.Flush();
        process.StandardInput.Close();


        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();

        return output;
    }

    //8
    public static string get_ip()
    {
        try
        {
            string host_name = Dns.GetHostName();
            IPAddress[] addresses = Dns.GetHostAddresses(host_name);

            foreach (IPAddress address in addresses)
            {
                // Check if it's an IPv4 address
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return address.ToString();
                }
            }


            return "No IPv4 address found";
        }
        catch (Exception ex)
        {
            // Handle any exceptions that may occur while obtaining the IP address
            return "Error obtaining IP address: " + ex.Message;
        }
    }

    //9
    public static string get_host_name()
    {
        try
        {
            // Get the hostname of the local machine
            string host_name = Environment.MachineName;
            return host_name;
        }
        catch (Exception ex)
        {

            return "Error obtaining hostname: " + ex.Message;
        }
    }

    //10
    public static string get_user_name()
    {
        try
        {
            // Get the username of the current user
            string user_name = Environment.UserName;
            return user_name;
        }
        catch (Exception ex)
        {

            return "Error obtaining username: " + ex.Message;
        }
    }

    //11
    public static int get_free_disk_space_gigabytes()
    {
        try
        {
            // Get free space
            DriveInfo driveInfo = new DriveInfo(Path.GetPathRoot(Environment.SystemDirectory));
            long free_space_bytes = driveInfo.AvailableFreeSpace;

            // Convert bytes to gigabytes
            long free_space_gigabytes = free_space_bytes / (1024 * 1024 * 1024);

            return (int)free_space_bytes;
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error obtaining free disk space: " + ex.Message);
            return -1; // Return error
        }
    }

    //12
    public static float get_used_cpu()
    {
        try
        {
            using PerformanceCounter cpu_counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpu_counter.NextValue(); //read it once to get a valid reading
            System.Threading.Thread.Sleep(1000);
            return cpu_counter.NextValue(); // Return the reading directly
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error obtaining CPU usage: " + ex.Message);
            return -1.0f; // Return a negative value to indicate an error
        }
    }

    //13
    public static long get_free_ram()
    {
        try
        {
            using PerformanceCounter ram_counter = new PerformanceCounter("Memory", "Available Bytes");
            return (long)ram_counter.NextValue(); // Return the available bytes of RAM
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error obtaining free RAM: " + ex.Message);
            return -1; // Return a negative value to indicate an error
        }
    }

    //14
    public static void get_resize_image(string inputPath, string outputPath, int targetWidth, int targetHeight)
    {
        try
        {
            using (var image = Image.Load(inputPath))
            {
                image.Mutate(x => x
                    .Resize(new ResizeOptions
                    {
                        Size = new Size(targetWidth, targetHeight),
                        Mode = ResizeMode.Max
                    }));
                image.Save(outputPath, new JpegEncoder());
                Console.WriteLine("Resized image saved to " + outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error resizing the image: " + ex.Message);
        }
    }

    //15
    public static string get_stack_trace()
    {
        try
        {
            throw new Exception("Throwing an exception to obtain the call stack.");
        }
        catch (Exception ex)
        {
            return ex.StackTrace;
        }
    }

    //16
    public static bool is_current_user_admin()
    {
        WindowsIdentity current_identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal current_principal = new WindowsPrincipal(current_identity);


        bool is_admin = current_principal.IsInRole(WindowsBuiltInRole.Administrator);

        return is_admin;
    }

    //17
    public static bool TestServerConnection(string server)
    {
        try
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(server);

            if (reply != null && reply.Status == IPStatus.Success)
            {
                return true; // Server is reachable
            }
            else
            {
                return false; // Server is not reachable
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error testing server connection: " + ex.Message);
            return false; // An error occurred while testing the connection
        }
    }


    static void Main(string[] args)
    {
        //----------function 1-2---------
        int screen_width = get_screen_width();
        int screen_height = get_screen_height();

        if (screen_width > 0 && screen_height > 0)
        {
            Console.WriteLine("Screen Width: " + screen_width);
            Console.WriteLine("Screen Height: " + screen_height);
        }
        else
        {
            Console.WriteLine("Unable to retrieve screen information.");
        }


        //-----------function 3-4 ---------------------

        string original_text = "Hello world";

        //Encode text
        string encoded_text = encode_base64(original_text);
        Console.WriteLine("Encoded text: " + encoded_text);

        //decode text
        string decoded_text = decode_base64(encoded_text);
        Console.WriteLine("Decoded text: " + decoded_text);


        //---------function 5 ---------------

        string input = "Strawberries&#190!??()555";
        string clean_message = clean_string(input);
        Console.WriteLine("Clean text: " + clean_message);


        //--------function 6 ----------------

        string command = "Get-Process"; // PowerShell command example, change the preference process
        string result = send_power_shell_command(command);

        Console.WriteLine(result);


        //-------function 7 -----------------

        string command_cmd = "dir"; // command exmaples CMD, change dir if you need other
        string result_cmd = send_cmd_command(command_cmd);

        Console.WriteLine(result);

        //--------function 8 ---------------

        string ip_address = get_ip();
        Console.WriteLine("IP Address: " + ip_address);

        //--------function 9 -------------
        string host_name = get_host_name();
        Console.WriteLine("Hostname: " + host_name);

        //-------function 10 ------------

        string user_name = get_user_name();
        Console.WriteLine("Username: " + user_name);

        //--------function 11 -------------

        int free_disk_space_ingb = get_free_disk_space_gigabytes();
        if (free_disk_space_ingb >= 0)
        {
            Console.WriteLine("Free Disk Space (GB): " + free_disk_space_ingb);
        }

        //--------function 12-------------

        float used_cpu_percentage = get_used_cpu();
        if (used_cpu_percentage >= 0)
        {
            Console.WriteLine("Used CPU Percentage: " + used_cpu_percentage + "%");
        }

        //--------function 13 ------------

        long free_ram_bytes = get_free_ram();
        if (free_ram_bytes >= 0)
        {
            Console.WriteLine("Free RAM (bytes): " + free_ram_bytes);
        }

        //-------function 14 -------------
        string inputPath = "C:\\Users\\inkaviation\\Downloads\\mario.jpg";
        string outputPath = "C:\\Users\\inkaviation\\Downloads\\mario_resized.jpg";
        int targetWidth = 200;
        int targetHeight = 100;

        get_resize_image(inputPath, outputPath, targetWidth, targetHeight);

        //--------function 15 ------------

        string stackTrace = get_stack_trace();
        Console.WriteLine("Error Stack Trace:");
        Console.WriteLine(stackTrace);

        //--------function 16 ------------

        bool is_user_admin = is_current_user_admin();
        if (is_user_admin)
        {
            Console.WriteLine("The actual user is an admin.");
        }
        else
        {
            Console.WriteLine("The actual user isn't an admin.");
        }

        //--------function 17 --------------

        string serverToTest = "example.com"; // Replace with the server you want to test
        bool isServerReachable = TestServerConnection(serverToTest);

        if (isServerReachable)
        {
            Console.WriteLine("Server is reachable.");
        }
        else
        {
            Console.WriteLine("Server is not reachable.");
        }

        Console.ReadKey();
    }


}



