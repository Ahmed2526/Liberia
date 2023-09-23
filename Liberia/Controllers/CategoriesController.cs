using AutoMapper;
using DAL.Consts;
using Liberia.Data;
using Liberia.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Liberia.Controllers
{
	[Authorize(Roles = Roles.Archive)]
	public class CategoriesController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		public CategoriesController(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _context.Categories.AsNoTracking().ToListAsync();

			return View(categories);
		}

		[HttpGet]
		[AjaxOnly]
		public IActionResult Create()
		{
			return PartialView("_Create");
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryVM vm)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var category = new Category
			{
				Name = vm.Name.Trim(),
				IsActive = true
			};

			_context.Add(category);
			await _context.SaveChangesAsync();

			return PartialView("_CategoryRow", category);
		}

		[HttpGet]
		[AjaxOnly]
		public async Task<IActionResult> Edit(int id)
		{
			var selectedCategory = await _context.Categories.FindAsync(id);

			if (selectedCategory is null)
				return NotFound();

			var vm = _mapper.Map<CategoryVM>(selectedCategory);

			return PartialView("_Edit", vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(CategoryVM vm)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var select = await _context.Categories.FindAsync(vm.Id);
			if (select is null)
				return NotFound();

			select.Name = vm.Name.Trim();
			select.ModifiedOn = DateTime.Now;

			_context.Categories.Update(select);
			await _context.SaveChangesAsync();

			return PartialView("_CategoryRow", select);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ToggleStatus(int id)
		{
			var Selected = await _context.Categories.FindAsync(id);
			if (Selected is null)
				return NotFound();

			Selected.IsActive = !Selected.IsActive;
			Selected.ModifiedOn = DateTime.Now;
			await _context.SaveChangesAsync();

			return Ok(Selected.ModifiedOn.ToString());
		}

		public IActionResult checkUnique(CategoryVM vm)
		{
			var category = _context.Categories.FirstOrDefault(e => e.Name == vm.Name);

			if (category is null)
				return Json(true);

			else if (vm.Id == category.Id)
				return Json(true);

			return Json(false);
		}

	}
}
