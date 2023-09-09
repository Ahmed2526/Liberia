using AutoMapper;
using BLL.ICustomService;
using DAL.Models;
using DAL.Models.BaseModels;
using Liberia.Data;
using Liberia.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Liberia.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        public BooksController(ApplicationDbContext context, IImageService imageService, IMapper mapper)
        {
            _context = context;
            _imageService = imageService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBooks()
        {
            var skip = int.Parse(Request.Form["start"]);
            var PageSize = int.Parse(Request.Form["length"]);
            var searchValue = Request.Form["search[value]"];

            var orderColumnIndex = Request.Form["order[0][column]"];
            var orderColumn = Request.Form[$"columns[{orderColumnIndex}][name]"];
            var orderDirection = Request.Form["order[0][dir]"];

            IQueryable<Book> booksQuery = _context.Books
                .Include(e => e.Author)
                .Include(e => e.Categories)
                .ThenInclude(e => e.Category);

            if (!string.IsNullOrEmpty(searchValue))
                booksQuery = booksQuery
                    .Where(e => e.Title.Contains(searchValue)
                   || e.Author!.Name.Contains(searchValue));

            //LINQ Dynamic Core Package
            var books = booksQuery.Where(e => e.Author!.IsActive == true)
              .Skip(skip).Take(PageSize)
              .OrderBy($"{orderColumn} {orderDirection}").ToList();

            var data = _mapper.Map<List<BookVM>>(books);

            var recordsTotal = _context.Books.Where(e => e.Author!.IsActive == true).Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

            return Ok(jsonData);
        }

        public IActionResult Details(int Id)
        {
            var book = _context.Books
                .Include(e => e.Author)
                .Include(e => e.Categories)
                .ThenInclude(e => e.Category)
                .FirstOrDefault(e => e.Id == Id);

            if (book is null)
                return NotFound();

            var bookvm = _mapper.Map<BookVM>(book);

            return View(bookvm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var BookFormVM = GeneratedInitializedBookFormVM();
            return View(BookFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                var intializedvm = GeneratedInitializedBookFormVM(vm);
                return View("Create", intializedvm);
            }

            var CheckImg = HandleImage(vm.ImageUrl!);
            if (CheckImg is null)
            {
                ModelState.AddModelError("ImageUrl", "Invalid Photo");
                var intializedvm = GeneratedInitializedBookFormVM(vm);
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
                PublishingDate = vm.PublishingDate
            };
            var CheckOgImgExist = CheckImg.TryGetValue("OriginalImg", out string? OriginalImg);
            var CheckThumbNailExist = CheckImg.TryGetValue("Thumbnail", out string? thumbNail);

            if (!CheckOgImgExist || !CheckThumbNailExist)
                ModelState.AddModelError("ImageUrl", "Invalid Photo");

            book.ImageName = OriginalImg!;
            book.ThumbNail = thumbNail!;

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

            var BookFormVM = new BookFormVM()
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

            var finalBookFormVM = GeneratedInitializedBookFormVM(BookFormVM);
            return View(finalBookFormVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                var intializedvm = GeneratedInitializedBookFormVM(vm);
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

            //Handle Image
            if (vm.ImageUrl is not null)
            {
                var UpdatedImg = _imageService.UpdateImages(vm.ImageUrl, selected.ImageName, selected.ThumbNail);
                if (UpdatedImg is not null)
                {
                    var CheckOgImgExist = UpdatedImg.TryGetValue("OriginalImg", out string? OriginalImg);
                    var CheckThumbNailExist = UpdatedImg.TryGetValue("Thumbnail", out string? thumbNail);

                    if (!CheckOgImgExist || !CheckThumbNailExist)
                        ModelState.AddModelError("ImageUrl", "Invalid Photo");

                    selected.ImageName = OriginalImg!;
                    selected.ThumbNail = thumbNail!;
                }
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

            return RedirectToAction(nameof(Details), new { Id = selected.Id });
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

        public IActionResult checkUnique(BookFormVM vm)
        {
            var book = _context.Books
                .FirstOrDefault(t => t.Title == vm.Title && t.AuthorId == vm.AuthorId);

            if (book is null)
                return Json(true);

            else if (vm.Id == book.Id)
                return Json(true);

            return Json(false);
        }


        private Dictionary<string, string> HandleImage(IFormFile img)
        {
            var ImgPath = _imageService.SaveImages(img);
            return ImgPath;
        }
        private BookFormVM GeneratedInitializedBookFormVM()
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

            var BookFormVM = new BookFormVM()
            {
                Authors = authors,
                Categories = categories
            };
            return BookFormVM;
        }
        private BookFormVM GeneratedInitializedBookFormVM(BookFormVM vm)
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
