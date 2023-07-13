using Liberia.Data;
using Liberia.Filters;
using Microsoft.EntityFrameworkCore;

namespace Liberia.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //TODO: use ViewModel
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return View(categories);
        }
        [HttpGet]
        [AjaxFilters]
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
                Name = vm.Name,
                IsActive = true
            };
            _context.Add(category);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet]
        [AjaxFilters]
        public async Task<IActionResult> Edit(int id)
        {
            var select = await _context.Categories.FindAsync(id);
            if (select is null)
                return NotFound();

            var vm = new CategoryVM()
            {
                Id = select.Id,
                Name = select.Name
            };
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

            select.Name = vm.Name;
            select.ModifiedOn = DateTime.Now;

            _context.Categories.Update(select);
            await _context.SaveChangesAsync();

            return Ok();
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




    }
}
