using Consul;
using Microsoft.EntityFrameworkCore;
using UserService.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok());

await RegisterWithConsul(app);


app.Run();


static async Task RegisterWithConsul(WebApplication app)
{
	try
	{
		Console.WriteLine("Registering User Service with Consul...");
		// Create Consul Client
		var consulClient = new ConsulClient(config =>
		{
			config.Address = new Uri("http://consul:8500");
		});
		// Get service hostname and port
		var serviceName = "user-service";
		var serviceId = $"{serviceName}-{Guid.NewGuid().ToString().Substring(0, 8)}"; // Unique ID for the service
		var servicePort = app.Urls.FirstOrDefault()?.Split(':')?.Last() switch
		{
			string port when int.TryParse(port, out int parsedPort) => parsedPort,
			_ => 80 // Default port to 80 if not specified
		};

		// Define the service registration
		var registration = new AgentServiceRegistration()
		{
			ID = serviceId,
			Name = serviceName,
			Address = "user-service",
			Port = servicePort,
			Check = new AgentServiceCheck()
			{
				HTTP = $"http://user-service:{servicePort}/health",
				Interval = TimeSpan.FromSeconds(10),
				Timeout = TimeSpan.FromSeconds(2),	
			}
		};

		await consulClient.Agent.ServiceRegister(registration);
		Console.WriteLine($"User Service registered with Consul (ID: {serviceId}");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Error registering with Consul: {ex.Message}");
	}
}
