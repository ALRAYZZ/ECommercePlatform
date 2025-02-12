namespace NotificationService.Models
{
	public class SmsRequestModel
	{
		public string? ToPhoneNumber { get; set; }
		public string? Message { get; set; }
	}
}
