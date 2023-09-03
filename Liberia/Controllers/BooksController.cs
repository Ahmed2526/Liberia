using BLL.ICustomService;
using DAL.Models;
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
        private readonly IImageService _imageService;
        public BooksController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }
        public IActionResult Index()
        {
            var books = _context.Books.Include(e => e.Author).Where(e => e.Author.IsActive == true).AsNoTracking().ToList();
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

            var CheckImg = HandleImage(vm.ImageUrl);
            if (string.IsNullOrEmpty(CheckImg))
            {
                ModelState.AddModelError("ImageUrl", "Invalid Photo");
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
                ImageName = HandleImage(vm.ImageUrl)
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var selected = _context.Books.Include(e => e.Categories).FirstOrDefault(e => e.Id == id);
            if (selected is null)
                return NotFound();

            var BookVm = new BookVM()
            {
                Id = selected.Id,
                AuthorId = selected.AuthorId,
                Title = selected.Title,
                Description = selected.Description,
                Hall = selected.Hall,
                ImagePath = selected.ImageName,
                IsAvailableForRental = selected.IsAvailableForRental,
                Publisher = selected.Publisher,
                PublishingDate = selected.PublishingDate,
                SelectedCategories = selected.Categories.Select(e => e.CategoryId).ToList()
            };

            var finalbookVm = GeneratedInitializedBookVM(BookVm);
            return View(finalbookVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookVM vm)
        {
            if (!ModelState.IsValid)
            {
                var intializedvm = GeneratedInitializedBookVM(vm);
                return View("Edit", intializedvm);
            }

            var selected = _context.Books.Include(e => e.Categories).FirstOrDefault(e => e.Id == vm.Id);

            if (selected is null)
                return NotFound();

            selected.Title = vm.Title;
            selected.Publisher = vm.Publisher;
            selected.PublishingDate = vm.PublishingDate;
            selected.Hall = vm.Hall;
            selected.IsAvailableForRental = vm.IsAvailableForRental;
            selected.Description = vm.Description;
            selected.AuthorId = vm.AuthorId;
            selected.ModifiedOn = DateTime.Now;

            if (vm.ImageUrl is not null)
            {
                var UpdatedImg = _imageService.UpdateImages(vm.ImageUrl, selected.ImageName);
                selected.ImageName = UpdatedImg;
            }

            //Remove Old Categories
            var OldCategories = selected.Categories.ToList();
            foreach (var item in OldCategories)
            {
                selected.Categories.Remove(item);
            }
            //Add New Categories
            foreach (var item in vm.SelectedCategories)
            {
                selected.Categories.Add(new BookCategory()
                {
                    CategoryId = item
                });
            }

            _context.Update(selected);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var Selected = await _context.Books.FindAsync(id);
            if (Selected is null)
                return NotFound();

            Selected.IsActive = !Selected.IsActive;
            Selected.ModifiedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(Selected.ModifiedOn.ToString());
        }

        public IActionResult checkUnique(BookVM vm)
        {
            var book = _context.Books
                .FirstOrDefault(t => t.Title == vm.Title && t.AuthorId == vm.AuthorId);

            if (book is null)
                return Json(true);

            else if (vm.Id == book.Id)
                return Json(true);

            return Json(false);
        }


        private string HandleImage(IFormFile img)
        {
            var ImgPath = _imageService.SaveImages(img);
            return ImgPath;
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
