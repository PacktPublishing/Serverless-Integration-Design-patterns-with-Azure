using Microsoft.EntityFrameworkCore;

namespace EventAPI.Models
{
    public class ProductCatalogueContext : DbContext
    {
        public ProductCatalogueContext(DbContextOptions<ProductCatalogueContext> options) : base(options)
        {


        }

        public DbSet<ProductCatalogue> ProductCatalogue { get; set; }
    }
}
