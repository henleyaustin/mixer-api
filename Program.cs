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

        // Build the web application
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.ConfigureServices();

        // Configure Kestrel with the certificate
        ConfigureKestrel(builder, port);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(); // Log to the console

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.ConfigureMiddleware();

        // Get the local IP address
        var ipAddresses = NetworkUtilities.GetLocalIpAddresses();

        // Print the server information to the console
        PrintServerInfo(ipAddresses, port);

        // Run the app on the specified IP addresses and port
        app.Run($"https://0.0.0.0:{port}");
    }

    private static void ConfigureKestrel(WebApplicationBuilder builder, int port)
    {
        // Load the certificate
        var cert = new X509Certificate2("mycert.pfx", "MixerPass123");

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            // Bind to the local IP address with HTTPS using the certificate
            serverOptions.Listen(IPAddress.Any, port, listenOptions =>
            {
                listenOptions.UseHttps(cert);
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
            port = 5001; // Default port
        }
        else if (!int.TryParse(portInput, out port))
        {
            port = 5001; // Default port if input is invalid
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
