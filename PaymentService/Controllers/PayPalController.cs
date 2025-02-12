using Microsoft.AspNetCore.Mvc;
using PaymentService.Interfaces;
using PaymentService.Models;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using Stripe;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PayPalController : ControllerBase
	{
		private readonly IPaymentSettings _paymentSettings;
		private readonly ILogger<PayPalController> _logger;
		public PayPalController(IPaymentSettings paymentSettings, ILogger<PayPalController> logger)
		{
			_paymentSettings = paymentSettings;
			_logger = logger;
		}


		// PayPal: Create Payment
		[HttpPost("create-payment")]
		public async Task<IActionResult> CreatePayPalPayment([FromBody] PayPalPaymentRequest request)
		{
			var environment = new SandboxEnvironment(_paymentSettings.PaypalClientId, _paymentSettings.PaypalClientSecret);
			var client = new PayPalHttpClient(environment);

			// Create order request
			var orderRequest = new OrdersCreateRequest();
			orderRequest.Prefer("return=representation");
			orderRequest.RequestBody(new OrderRequest
			{
				CheckoutPaymentIntent = "CAPTURE",
				PurchaseUnits = new List<PurchaseUnitRequest>
				{
					new PurchaseUnitRequest
					{
						AmountWithBreakdown = new AmountWithBreakdown
						{
							CurrencyCode = request.CurrencyCode ?? "EUR",
							Value = request.Amount.ToString("F2")
						},
						Description = request.Description // Pass the payment description
					}
				},
				ApplicationContext = new ApplicationContext()
				{
					ReturnUrl = "http://localhost:5001/api/payments/capture-paypal-order", // URL that we will redirect that shows the payment success
					CancelUrl = "http://localhost:5001/api/payments/cancel-paypal-order" // URL that we will redirect that shows the payment cancel
				}
			});

			try
			{
				var response = await client.Execute(orderRequest);
				var result = response.Result<Order>();

				// Extract approval URL
				var approvalUrl = result.Links.FirstOrDefault(x => x.Rel == "approve")?.Href;

				// Log success
				_logger.LogInformation("PayPal payment created successfully. Order  ID: {OrderId}", result.Id);

				// Return essential details
				return Ok(new
				{
					OrderId = result.Id,
					ApprovalUrl = approvalUrl
				});
			}
			catch (HttpException httpException)
			{
				_logger.LogError(httpException, "Failed to create PayPal payment");

				return StatusCode((int)httpException.StatusCode, new
				{
					ErrorCode = httpException.StatusCode,
					ErrorMessage = httpException.Message,
					ErrorDetails = httpException.Data
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unexpected error ocurred while creating PayPal payment");

				return StatusCode(500, new
				{
					ErrorCode = 500,
					ErrorMessage = "An unexpected error ocurred while creating PayPal payment",
					ErrorDetail = ex.Message
				});
			}
		}
	}
}
