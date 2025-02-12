namespace PaymentService.Models
{
	public class PayPalPaymentRequest
	{
		public decimal Amount { get; set; }
		public string CurrencyCode { get; set; } = "EUR";
		public string Description { get; set; } = "Payment";
	}
}
