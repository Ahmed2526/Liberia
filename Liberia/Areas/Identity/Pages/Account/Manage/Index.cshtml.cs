// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using BLL.ICustomService;
using DAL.Consts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Liberia.Areas.Identity.Pages.Account.Manage
{
	public class IndexModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IImageService _imageService;

		public IndexModel(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IImageService imageService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_imageService = imageService;
		}

		public string Username { get; set; }

		[TempData]
		public string StatusMessage { get; set; }

		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required]
			[Display(Name = "Full Name")]
			[RegularExpression(RegexPatterns.EnglishLettersOnlyWithSpacePattern, ErrorMessage = Errors.EnglishLettersOnly)]
			public string FullName { get; set; }

			[Phone]
			[Display(Name = "Phone number")]
			[RegularExpression(RegexPatterns.EgyPhonePattern, ErrorMessage = Errors.Phone)]
			public string PhoneNumber { get; set; }

			public IFormFile Avatar { get; set; }
		}

		private async Task LoadAsync(ApplicationUser user)
		{
			var userName = await _userManager.GetUserNameAsync(user);
			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

			Username = userName;

			Input = new InputModel
			{
				PhoneNumber = phoneNumber,
				FullName = user.FullName,
			};
		}

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			await LoadAsync(user);
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			//Handle Avatar.
			if (Input.Avatar is not null)
			{
				var response = _imageService.SaveProfileImage(Input.Avatar, user.Id);

				if (!response.Result)
				{
					StatusMessage = response.Message;
					return RedirectToPage();
				}

				StatusMessage = "Your profile has been updated";
				return RedirectToPage();
			}

			if (!ModelState.IsValid)
			{
				await LoadAsync(user);
				return Page();
			}

			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
			if (Input.PhoneNumber != phoneNumber)
			{
				var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
				if (!setPhoneResult.Succeeded)
				{
					StatusMessage = "Unexpected error when trying to set phone number.";
					return RedirectToPage();
				}
			}

			var fullname = Input.FullName.Trim();
			if (fullname != user.FullName)
			{
				user.FullName = fullname;
				var response = await _userManager.UpdateAsync(user);
				if (!response.Succeeded)
				{
					StatusMessage = "Unexpected error when trying to set Full Name.";
					return RedirectToPage();
				}
			}

			await _signInManager.RefreshSignInAsync(user);
			StatusMessage = "Your profile has been updated";
			return RedirectToPage();
		}
	}
}
