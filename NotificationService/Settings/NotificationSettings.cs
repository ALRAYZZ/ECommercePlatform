namespace NotificationService.Settings
{
	public class NotificationSettings
	{
		public TwilioSettings Twilio { get; set; } = new TwilioSettings();
		public SendGridSettings SendGrid { get; set; } = new SendGridSettings();
	}
	public class TwilioSettings
	{
		public string AccountSid { get; set; } = string.Empty;
		public string AuthToken { get; set; } = string.Empty;
		public string FromPhoneNumber { get; set; } = string.Empty;
	}

	public class SendGridSettings
	{
		public string ApiKey { get; set; } = string.Empty;
		public string SenderEmail { get; set; } = string.Empty;
	}
}
