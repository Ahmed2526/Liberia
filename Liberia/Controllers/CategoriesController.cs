using Liberia.Data;

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
            var categories = _context.Categories.Where(e => e.IsActive == true)
                .ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var category = new Category
            {
                Name = vm.Name,
                IsActive = true
            };
            _context.Add(category);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Selected = _context.Categories.Find(id);
            if (Selected is null)
                return NotFound();

            _context.Remove(Selected);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        

    }
}
