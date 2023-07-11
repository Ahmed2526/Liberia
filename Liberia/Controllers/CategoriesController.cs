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
        public IActionResult Index()
        {
            //TODO: use ViewModel
            var categories = _context.Categories.AsNoTracking().ToList();
            return View(categories);
        }
        [AjaxFilters]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var category = new Category
            {
                Name = vm.Name,
                IsActive = true
            };
            _context.Add(category);
            _context.SaveChanges();

            return Ok();
        }
        [AjaxFilters]
        public IActionResult Edit(int id)
        {
            var select = _context.Categories.Find(id);
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
        public IActionResult Edit(CategoryVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var select = _context.Categories.Find(vm.Id);
            if (select is null)
                return NotFound();


            select.IsActive = true;
            select.Name = vm.Name;
            select.ModifiedOn = DateTime.Now;

            _context.Categories.Update(select);
            _context.SaveChanges();

            return Ok();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var Selected = _context.Categories.Find(id);
            if (Selected is null)
                return NotFound();

            Selected.IsActive = !Selected.IsActive;
            Selected.ModifiedOn = DateTime.Now;
            _context.SaveChanges();

            return Ok(Selected.ModifiedOn.ToString());
        }




    }
}
