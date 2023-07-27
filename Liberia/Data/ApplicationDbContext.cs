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
            builder.Entity<Book>()
           .HasMany(e => e.Categories)
           .WithMany(e => e.Books)
           .UsingEntity<BooksCategories>();




            base.OnModelCreating(builder);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }


    }
}