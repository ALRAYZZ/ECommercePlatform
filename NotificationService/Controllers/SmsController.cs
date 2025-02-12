using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SmsController : ControllerBase
	{
		private readonly NotificationSender _notificationSender;

		public SmsController(NotificationSender notificationSender)
		{
			_notificationSender = notificationSender;
		}

		[HttpPost]
		public async Task<IActionResult> SendSms([FromBody] SmsRequestModel smsRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			await _notificationSender.SendSmsAsync(smsRequest);
			return Ok();
		}
	}
}
