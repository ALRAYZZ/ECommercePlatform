using Microsoft.Extensions.Options;
using NotificationService.Models;
using NotificationService.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace NotificationService.Services
{
	public class NotificationSender
	{
		private readonly string _twilioAccountSid;
		private readonly string _twilioAuthToken;
		private readonly string _twilioFromPhoneNumber;
		private readonly string _sendGridApiKey;
		private readonly string _senderEmail;

		public NotificationSender(IOptions<NotificationSettings> settings)
		{
			_twilioAccountSid = settings.Value.Twilio.AccountSid;
			_twilioAuthToken = settings.Value.Twilio.AuthToken;
			_twilioFromPhoneNumber = settings.Value.Twilio.FromPhoneNumber;
			_sendGridApiKey = settings.Value.SendGrid.ApiKey;
			_senderEmail = settings.Value.SendGrid.SenderEmail;
		}

		public async Task SendSmsAsync(SmsRequestModel request)
		{
			TwilioClient.Init(_twilioAccountSid, _twilioAuthToken);

			var message = await MessageResource.CreateAsync(
				body: request.Message,
				from: new Twilio.Types.PhoneNumber(_twilioFromPhoneNumber),
				to: new Twilio.Types.PhoneNumber(request.ToPhoneNumber)
			);

			Console.WriteLine($"SMS sent: {message.Sid}");
		}

		public async Task SendEmailAsync(EmailRequestModel request)
		{
			var client = new SendGridClient(_sendGridApiKey);
			var msg = MailHelper.CreateSingleEmail(
				new EmailAddress(_senderEmail),
				new EmailAddress(request.ToEmail),
				request.Subject,
				request.Body,
				request.Body
			);

			var response = await client.SendEmailAsync(msg);
			if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
			{
				Console.WriteLine("Email sent successfully.");
			}
			else
			{
				Console.WriteLine($"Failed to send email. Status code {response.StatusCode}");
			}
		}
	}
		
}
