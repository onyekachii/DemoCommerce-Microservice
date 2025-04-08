using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Domain.Entities;
using ProductAPI.Domain.Interface;
using ProductAPI.Infrastructure.DbContexts;
using System.Linq.Expressions;

namespace ProductAPI.Infrastructure.ProductRepository
{
    internal class ProductRepo(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var getProduct = await GetByAsync(p => p.Name.Equals(entity.Name));
                if (getProduct is not null) 
                {
                    return new Response(false, $"{entity.Name} already added");
                }
                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                return (currentEntity is not null && currentEntity.Id > 0) ? new Response(true, $"{entity.Name} added") : new Response(false, $"Error occured while adding {entity.Name}");                
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while adding new product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is not null)
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    return new Response(true, $"{entity.Name} deleted");
                }
                return new Response(false, $"{entity.Name} not found");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while deleting new product");
            }
        }

        public async Task<Product?> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product :null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while trying to find product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await context.Products.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while trying to products");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                return await context.Products.Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while trying to find product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await context.Products.FindAsync(entity.Id);
                if( product is null)
                    return new Response(false, $"{entity.Name} not found");
                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} updated");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while trying to find product");
            }
        }
    }
}
