namespace PaymentService.Models
{
	public class StripePaymentRequest
	{
		public decimal Amount { get; set; }
		public string CurrencyCode { get; set; } = "eur";
		public string Description { get; set; } = "Payment";
		public string Token { get; set; } = string.Empty;
	}
}
