using Microsoft.Extensions.Options;
using PaymentService.Interfaces;
using PaymentService.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection("PaymentSettings"));
builder.Services.AddSingleton<IPaymentSettings>(provider =>
{
	var settings = provider.GetRequiredService<IOptions<PaymentSettings>>().Value;
	return settings;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
