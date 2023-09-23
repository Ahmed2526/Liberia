using AutoMapper;
using Liberia.Data;
using Liberia.Filters;

namespace Liberia.Controllers
{
	public class BooksCopies : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		public BooksCopies(ApplicationDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		[AjaxOnly]
		public IActionResult Create(int id)
		{
			var bookcopy = new BookCopyFormVM() { BookId = id };
			return PartialView("_Create", bookcopy);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BookCopyFormVM vm)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var book = await _context.Books.FindAsync(vm.BookId);

			if (book is null)
				return NotFound();

			var bookCopy = new BookCopy()
			{
				EditionNumber = vm.EditionNumber,
				IsAvailableForRental = vm.IsAvailableForRental,
				CreatedOn = DateTime.Now,
				IsActive = true,
				BookId = vm.BookId
			};

			await _context.BookCopies.AddAsync(bookCopy);
			await _context.SaveChangesAsync();

			var bookcp = _context.BookCopies.
				Where(e => e.SerialNumber == bookCopy.SerialNumber)
				.FirstOrDefault(e => e.BookId == bookCopy.BookId);


			var copyVM = _mapper.Map<BookCopyVM>(bookcp);

			#region noMapper
			//var copyVM = new BookCopyVM()
			//{
			//    Id = bookcp!.Id,
			//    SerialNumber = bookCopy.SerialNumber,
			//    CreatedOn = bookCopy.CreatedOn,
			//    EditionNumber = bookCopy.EditionNumber,
			//    IsActive = bookCopy.IsActive,
			//    IsAvailableForRental = bookCopy.IsAvailableForRental
			//};
			#endregion
			return PartialView("_BookCopyRow", copyVM);
		}

		[AjaxOnly]
		public IActionResult Edit(int id)
		{
			var bookcp = _context.BookCopies.Find(id);

			if (bookcp is null)
				return NotFound();

			var bookcpForm = new BookCopyFormVM()
			{
				Id = id,
				EditionNumber = bookcp.EditionNumber,
				IsAvailableForRental = bookcp.IsAvailableForRental
			};

			return PartialView("_Edit", bookcpForm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(BookCopyFormVM vm)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var bookcp = _context.BookCopies.Find(vm.Id);

			if (bookcp is null)
				return NotFound();

			bookcp.EditionNumber = vm.EditionNumber;
			bookcp.IsAvailableForRental = vm.IsAvailableForRental;
			bookcp.ModifiedOn = DateTime.Now;

			_context.Update(bookcp);
			_context.SaveChanges();

			var bookcpVM = _mapper.Map<BookCopyVM>(bookcp);

			return PartialView("_BookCopyRow", bookcpVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ToggleStatus(int id)
		{
			var Selected = await _context.BookCopies.FindAsync(id);
			if (Selected is null)
				return NotFound();

			Selected.IsActive = !Selected.IsActive;
			Selected.ModifiedOn = DateTime.Now;
			await _context.SaveChangesAsync();

			return Ok();
		}
	}
}
