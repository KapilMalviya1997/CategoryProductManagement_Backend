using Microsoft.EntityFrameworkCore;

namespace Product_Category_Management_System.Models
{

    public class ProductCategoryContext : DbContext
    {
        public ProductCategoryContext(DbContextOptions<ProductCategoryContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
