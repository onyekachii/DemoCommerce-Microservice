
using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Infrastructure.DbContexts
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> opt): DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
