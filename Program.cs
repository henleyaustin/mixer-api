using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Net;
using windows_audio_api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFirebaseAndLocal",
        policy => policy
            .WithOrigins(
                "https://web-mixer.web.app",         
                "https://web-mixer.firebaseapp.com", 
                "http://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Register AudioManager with DI
builder.Services.AddScoped<AudioManager>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Log to the console

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use the CORS policy before any request handling
app.UseCors("AllowFirebaseAndLocal");

app.UseAuthorization();

app.MapControllers();

// Get the local IP address
var ipAddresses = GetLocalIpAddresses();
var port = builder.Configuration.GetValue("PORT", "5001");

// Print the server information to the console
Console.WriteLine("Server is running on the following addresses:");
foreach (var ipAddress in ipAddresses)
{
    Console.WriteLine($"IP Address {ipAddress}");
    Console.WriteLine($"Port: {port}");
    Console.WriteLine("-------------------------");
}

app.Run();

static IEnumerable<string> GetLocalIpAddresses()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork)
        {
            yield return ip.ToString();
        }
    }
}
