// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using BLL.CustomService;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace Liberia.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterConfirmationModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IEmailSender _sender;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IEmailSender _emailSender;

		public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
		{
			_userManager = userManager;
			_sender = sender;
			_webHostEnvironment = webHostEnvironment;
			_emailSender = emailSender;
		}

		public string Email { get; set; }

		public bool DisplayConfirmAccountLink { get; set; }

		public string EmailConfirmationUrl { get; set; }

		public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
		{
			//if (email == null)
			//{
			//	return RedirectToPage("/Index");
			//}
			returnUrl = returnUrl ?? Url.Content("~/");

			//var user = await _userManager.FindByEmailAsync(email);
			//if (user == null)
			//{
			//	return NotFound($"Unable to load user with email '{email}'.");
			//}
			Email = email;

			//if (false)
			//{
			//	var userId = await _userManager.GetUserIdAsync(user);
			//	var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			//	code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			//	var callbackUrl = Url.Page(
			//				"/Account/ConfirmEmail",
			//	pageHandler: null,
			//				values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
			//			protocol: Request.Scheme);

			//	var Filepath = $"{_webHostEnvironment.WebRootPath}/Templetes/Email.html";
			//	var str = new StreamReader(Filepath);
			//	var body = str.ReadToEnd();
			//	str.Close();
			//	body = body.Replace("[url]", callbackUrl);

			//	await _emailSender.SendEmailAsync(Email, "Confirm your email", body);
			//}

			return Page();
		}
		//public async Task<IActionResult> OnPostAsync(string email, string returnUrl = null)
		//{
		//	if (email == null)
		//	{
		//		return RedirectToPage("/Index");
		//	}
		//	returnUrl = returnUrl ?? Url.Content("~/");

		//	var user = await _userManager.FindByEmailAsync(email);
		//	if (user == null)
		//	{
		//		return NotFound($"Unable to load user with email '{email}'.");
		//	}
		//	Email = email;



		//	return Page();

		//}
	}
}