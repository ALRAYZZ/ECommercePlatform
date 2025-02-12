using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services;

namespace NotificationService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EmailController : ControllerBase
	{
		private readonly NotificationSender _notificationSender;

		public EmailController(NotificationSender notificationSender)
		{
			_notificationSender=notificationSender;
		}

		[HttpPost]
		public async Task<IActionResult> SendEmail([FromBody] EmailRequestModel request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			await _notificationSender.SendEmailAsync(request);
			return Ok();
		}
	}
}
