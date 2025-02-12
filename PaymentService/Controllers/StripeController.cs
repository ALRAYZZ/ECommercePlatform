using Microsoft.AspNetCore.Mvc;
using PaymentService.Interfaces;
using PaymentService.Models;
using Stripe;

namespace PaymentService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StripeController : ControllerBase
	{
		private readonly IPaymentSettings _paymentSettings;
		private readonly ILogger<StripeController> _logger;

		public StripeController(IPaymentSettings paymentSettings, ILogger<StripeController> logger)
		{
			_paymentSettings=paymentSettings;
			_logger=logger;
		}

		[HttpPost("create-payment")]
		public IActionResult CreateStripePayment([FromBody] StripePaymentRequest request)
		{
			StripeConfiguration.ApiKey = _paymentSettings.StripeSecretKey;

			var options = new ChargeCreateOptions
			{
				Amount = (int)(request.Amount * 100), //Converts to cents
				Currency = request.CurrencyCode,
				Description = request.Description,
				Source = request.Token // Stripe token from client
			};

			var service = new ChargeService();

			try
			{
				var charge = service.Create(options);

				_logger.LogInformation("Stripe payment created successfully. Charge ID: {ChargeId}", charge.Id);

				return Ok(new
				{
					ChargeId = charge.Id,
					Status = charge.Status
				});
			}
			catch (StripeException stripeException)
			{
				_logger.LogError(stripeException, "Failed to create Stripe payment.");
				return StatusCode(400, new
				{
					ErrorCode = stripeException.HttpStatusCode,
					ErrorMessage = stripeException.Message,
					ErrorDetails = stripeException.StripeResponse
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unexpected error occurred while creating Stripe payment.");
				return StatusCode(500, new
				{
					ErrorCode = 500,
					ErrorMessage = "An unexpected error occurred.",
					ErrorDetails = ex.Message
				});
			}
		}
	}
}
