using PaymentService.Interfaces;

namespace PaymentService.Settings
{
	public class PaymentSettings : IPaymentSettings
	{
		public string PaypalClientId { get; set; } = string.Empty;

		public string PaypalClientSecret { get; set; } = string.Empty;

		public string StripeSecretKey { get; set; } = string.Empty;
	}
}
