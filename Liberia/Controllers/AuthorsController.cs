using AutoMapper;
using DAL.Consts;
using Liberia.Data;
using Liberia.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Liberia.Controllers
{
    [Authorize(Roles = Roles.Archive)]
    public class AuthorsController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var authors = await _Context.Authors.AsNoTracking().ToListAsync();
            return View(authors);
        }

        [HttpGet]
        [AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var author = new Author
            {
                Name = vm.Name.Trim(),
                IsActive = true
            };

            _Context.Authors.Add(author);
            await _Context.SaveChangesAsync();

            return PartialView("_AuthorRow", author);
        }

        [HttpGet]
        [AjaxOnly]
        public async Task<IActionResult> Edit(int id)
        {
            var selectedAuthor = await _Context.Authors.FindAsync(id);

            if (selectedAuthor is null)
                return NotFound();

            var vm = _mapper.Map<AuthorVM>(selectedAuthor);

            return PartialView("_Edit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var select = await _Context.Authors.FindAsync(vm.Id);
            if (select is null)
                return NotFound();

            select.Name = vm.Name.Trim();
            select.ModifiedOn = DateTime.Now;

            _Context.Authors.Update(select);
            await _Context.SaveChangesAsync();

            return PartialView("_AuthorRow", select);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var Selected = await _Context.Authors.FindAsync(id);
            if (Selected is null)
                return NotFound();

            Selected.IsActive = !Selected.IsActive;
            Selected.ModifiedOn = DateTime.Now;
            await _Context.SaveChangesAsync();

            return Ok(Selected.ModifiedOn.ToString());
        }

        public IActionResult checkUnique(AuthorVM vm)
        {
            var author = _Context.Authors.FirstOrDefault(e => e.Name == vm.Name);

            if (author is null)
                return Json(true);

            else if (vm.Id == author.Id)
                return Json(true);

            return Json(false);
        }

    }
}
