using Microsoft.Extensions.Logging;
using mixer_api;
using mixer_api.Extensions;
using mixer_api.Utilities;
using System.Net;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main(string[] args)
    {
        // Prompt the user for the port number
        int port = GetPortFromUser();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureServices();
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        ConfigureKestrel(builder, port);

        var app = builder.Build();

        app.ConfigureMiddleware();

        // Get the local IP address
        var ipAddresses = NetworkUtilities.GetLocalIpAddresses();

        // Print the server information to the console
        PrintServerInfo(ipAddresses, port);

        // Run the app on the specified IP addresses and port
        app.Run();
    }

    private static void ConfigureKestrel(WebApplicationBuilder builder, int port)
    {
        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            // Bind to all network interfaces (0.0.0.0) on the specified port for HTTPS
            serverOptions.Listen(IPAddress.Any, port, listenOptions =>
            {
                listenOptions.UseHttps();
            });
        });
    }


    private static int GetPortFromUser()
    {
        Console.WriteLine("Please enter the port number you want to use (default is 5001):");
        var portInput = Console.ReadLine();
        int port;

        // Use the default port if the user presses Enter without input
        if (string.IsNullOrWhiteSpace(portInput))
        {
            port = 5001;
        }
        else if (!int.TryParse(portInput, out port))
        {
            port = 5001;
            Console.WriteLine("Invalid input. Defaulting to port 5001.");
        }

        return port;
    }

    private static void PrintServerInfo(IEnumerable<string> ipAddresses, int port)
    {

        Console.WriteLine("USE THE SERVER INFORMATION BELOW TO CONNECT FROM THE WEB APPLICATION" + Environment.NewLine);
        Console.WriteLine("Server is running on the following addresses:");

        foreach (var ipAddress in ipAddresses)
        {
            Console.WriteLine("-------------------------");
            Console.WriteLine($"IP Address: {ipAddress}");
            Console.WriteLine($"Port: {port}");
        }

        Console.WriteLine("-------------------------");
    }
}
