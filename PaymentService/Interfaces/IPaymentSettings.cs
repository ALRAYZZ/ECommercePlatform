namespace PaymentService.Interfaces
{
	public interface IPaymentSettings
	{
		string PaypalClientId { get; }
		string PaypalClientSecret { get; }
		string StripeSecretKey { get; }
	}
}
