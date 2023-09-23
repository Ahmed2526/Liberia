using AutoMapper;
using DAL.Consts;
using Liberia.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Liberia.Controllers
{
	[Authorize(Roles = Roles.Admin)]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper _mapper;
		private readonly IEmailSender _emailSender;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
			_emailSender = emailSender;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			var Usersvm = _mapper.Map<IEnumerable<UserVM>>(users);
			return View(Usersvm);
		}

		[HttpGet]
		[AjaxOnly]
		public IActionResult Create()
		{
			var initUser = GenerateInitializedUserFormVM();
			return PartialView("_Create", initUser);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserFormVM vm)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = _mapper.Map<ApplicationUser>(vm);

			var CurrentuserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			user.Id = Guid.NewGuid().ToString();
			user.CreatedOn = DateTime.Now;
			user.IsActive = true;
			user.UserName = vm.UserName.Trim();
			user.FullName = vm.FullName.Trim();
			user.CreatedById = CurrentuserId;

			var result = await _userManager.CreateAsync(user, vm.Password);
			if (!result.Succeeded)
				return BadRequest();

			//Handle Roles
			var RoleResult = await _userManager.AddToRolesAsync(user, vm.Roles);
			if (!RoleResult.Succeeded)
				return BadRequest();

			var Uservm = _mapper.Map<UserVM>(user);

			return PartialView("_UserRow", Uservm);
		}

		[HttpGet]
		[AjaxOnly]
		public async Task<IActionResult> Edit(string id)
		{
			var selected = await _userManager.FindByIdAsync(id);

			if (selected is null)
				return NotFound();

			var Role = await _userManager.GetRolesAsync(selected);

			var uservm = new EditUserVM()
			{
				FullName = selected.FullName,
				Email = selected.Email,
				UserName = selected.UserName,
				PhoneNumber = selected.PhoneNumber,
				UserId = selected.Id
			};

			if (Role.Any())
			{
				uservm.Roles = Role.ToList();
			}

			var inituservm = GeneratedInitializedEditUserVM(uservm);

			return PartialView("_Edit", inituservm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditUserVM vm)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var select = await _userManager.FindByIdAsync(vm.UserId);
			if (select is null)
				return NotFound();

			var CurrentuserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			select.FullName = vm.FullName.Trim();
			select.Email = vm.Email;
			select.NormalizedEmail = vm.Email.ToUpper();
			select.UserName = vm.UserName.Trim();
			select.NormalizedUserName = vm.UserName.Trim().ToUpper();
			select.PhoneNumber = vm.PhoneNumber;
			select.ModifiedById = CurrentuserId;
			select.ModifiedOn = DateTime.Now;

			var result = await _userManager.UpdateAsync(select);

			if (!result.Succeeded)
				return BadRequest();

			//Refresh Login
			await _userManager.UpdateSecurityStampAsync(select);

			//handle Roles
			var UserRoles = await _userManager.GetRolesAsync(select);
			if (UserRoles.Any())
				await _userManager.RemoveFromRolesAsync(select, UserRoles);

			var AddUserToRoles = await _userManager.AddToRolesAsync(select, vm.Roles);
			if (!AddUserToRoles.Succeeded)
				return BadRequest();

			var Uservm = _mapper.Map<UserVM>(select);

			return PartialView("_UserRow", Uservm);
		}

		[HttpGet]
		[AjaxOnly]
		public async Task<IActionResult> UpdatePassword(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
				return NotFound();

			var passform = new PasswordVM() { UserId = user.Id };

			return PartialView("_PasswordForm", passform);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdatePassword(PasswordVM vm)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var user = await _userManager.FindByIdAsync(vm.UserId);
			if (user is null)
				return NotFound();

			var result = await _userManager
				.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);

			if (!result.Succeeded)
				return BadRequest("Incorrect Current Password!");

			var CurrentuserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			user.ModifiedOn = DateTime.Now;
			user.ModifiedById = CurrentuserId;

			await _userManager.UpdateAsync(user);

			return Ok();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ToggleStatus(string id)
		{
			var Selected = await _userManager.FindByIdAsync(id);

			if (Selected is null)
				return NotFound();

			var CurrentuserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			Selected.IsActive = !Selected.IsActive;
			Selected.ModifiedOn = DateTime.Now;
			Selected.ModifiedById = CurrentuserId;

			await _userManager.UpdateAsync(Selected);

			if (!Selected.IsActive)
				await _userManager.UpdateSecurityStampAsync(Selected);

			return Ok(Selected.ModifiedOn.ToString());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UnlockUser(string id)
		{
			var Selected = await _userManager.FindByIdAsync(id);

			if (Selected is null)
				return NotFound();

			var CurrentuserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var islocked = await _userManager.IsLockedOutAsync(Selected);

			if (islocked)
				await _userManager.SetLockoutEndDateAsync(Selected, null);

			Selected.ModifiedOn = DateTime.Now;
			Selected.ModifiedById = CurrentuserId;

			await _userManager.UpdateAsync(Selected);

			return Ok(Selected.ModifiedOn.ToString());
		}

		public async Task<IActionResult> checkUnique(UserFormVM vm)
		{
			if (!string.IsNullOrEmpty(vm.Email))
			{
				var checkEmailExist = await _userManager.FindByEmailAsync(vm.Email);

				if (checkEmailExist is null)
					return Json(true);

				else if (vm.UserId == checkEmailExist.Id)
					return Json(true);
			}
			else if (!string.IsNullOrEmpty(vm.UserName))
			{
				var checkuserNameExist = await _userManager.FindByNameAsync(vm.UserName);

				if (checkuserNameExist is null)
					return Json(true);

				else if (vm.UserId == checkuserNameExist.Id)
					return Json(true);
			}
			return Json(false);

		}

		private UserFormVM GenerateInitializedUserFormVM()
		{
			var initUser = new UserFormVM()
			{
				SelectedRoles = _roleManager.Roles.Select(e => new SelectListItem()
				{
					Text = e.Name,
					Value = e.Name
				})
			};

			return initUser;
		}
		private EditUserVM GeneratedInitializedEditUserVM(EditUserVM vm)
		{
			var roles = _roleManager.Roles.Select(e => new SelectListItem()
			{
				Text = e.Name,
				Value = e.Name
			}).OrderBy(e => e.Text);

			vm.SelectedRoles = roles;
			return vm;
		}
	}
}
