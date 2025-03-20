using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } //представлява таблица в базата
        public DbSet<Category> Categories { get; set; } // таблицата за категории
    }
}
