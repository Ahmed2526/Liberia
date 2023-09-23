using DAL.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BLL.CustomService
{
	public class EmailSender : IEmailSender
	{
		private readonly MailSettings _mailSettings;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public EmailSender(IOptions<MailSettings> mailSettings, IWebHostEnvironment webHostEnvironment)
		{
			_mailSettings = mailSettings.Value;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var message = new MailMessage()
			{
				From = new MailAddress(_mailSettings.Email!, _mailSettings.DisplayName),
				Body = htmlMessage,
				Subject = subject,
				IsBodyHtml = true
			};

			message.To.Add(_webHostEnvironment.IsDevelopment() ? "ahmedebrahem835@gmail.com" : email);

			var smtpClient = new SmtpClient(_mailSettings.Host)
			{
				Port = _mailSettings.Port,
				Credentials = new NetworkCredential(_mailSettings.Email, _mailSettings.Password),
				EnableSsl = true
			};

			await smtpClient.SendMailAsync(message);
			smtpClient.Dispose();
		}

	}
}
