using DAL.Models.BaseModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Liberia.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //ManyTo many Relation Between Categories & Books.
            builder.Entity<BookCategory>().HasKey(e => new { e.BookId, e.CategoryId });
            base.OnModelCreating(builder);


            base.OnModelCreating(builder);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BooksCategories { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }

    }
}