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
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{

        //}


    }
}
