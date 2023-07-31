using DAL.Models.BaseModels;
using Liberia.Data;
using Liberia.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Liberia.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var books = _context.Books.AsNoTracking().ToList();
            return View(books);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            var bookVM = GeneratedInitializedBookVM();
            return View(bookVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookVM vm)
        {
            if (!ModelState.IsValid)
            {
                var intializedvm = GeneratedInitializedBookVM(vm);
                return View("Create", intializedvm);
            }

            var book = new Book()
            {
                Title = vm.Title,
                AuthorId = vm.AuthorId,
                Description = vm.Description,
                CreatedOn = DateTime.Now,
                IsActive = true,
                Hall = vm.Hall,
                IsAvailableForRental = vm.IsAvailableForRental,
                Publisher = vm.Publisher,
                PublishingDate = vm.PublishingDate,
                ImageUrl = "test.jpg"
            };

            foreach (var item in vm.SelectedCategories)
            {
                book.Categories.Add(new BookCategory()
                {
                    CategoryId = item
                });
            }
            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        private BookVM GeneratedInitializedBookVM()
        {
            var authors = _context.Authors.Where(a => a.IsActive == true).
               Select(a => new SelectListItem
               {
                   Text = a.Name,
                   Value = a.Id.ToString()
               }).OrderBy(a => a.Text).ToList();

            var categories = _context.Categories.Where(a => a.IsActive == true)
                .Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
                .OrderBy(a => a.Text).ToList();

            var bookVM = new BookVM()
            {
                Authors = authors,
                Categories = categories
            };
            return bookVM;
        }
        private BookVM GeneratedInitializedBookVM(BookVM vm)
        {
            var authors = _context.Authors.Where(a => a.IsActive == true).
               Select(a => new SelectListItem
               {
                   Text = a.Name,
                   Value = a.Id.ToString()
               }).OrderBy(a => a.Text).ToList();

            var categories = _context.Categories.Where(a => a.IsActive == true)
                .Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
                .OrderBy(a => a.Text).ToList();

            vm.Authors = authors;
            vm.Categories = categories;

            return vm;
        }

    }
}
