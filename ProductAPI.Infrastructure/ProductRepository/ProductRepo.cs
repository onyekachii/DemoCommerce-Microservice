using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.DbContexts;
using ProductAPI.Infrastructure.Interfaces;
using System.Linq.Expressions;

namespace ProductAPI.Infrastructure.ProductRepository
{
    internal class ProductRepo(ProductDbContext context) : IProductRepo
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var getProduct = await GetByAsync(_ => _.Name.Equals(entity.Name));
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

        public async Task<Response> Delete(Product entity)
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
                if (product is not null)
                    return product;
                else
                    return null;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occured while trying to find product");
            }
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Response> Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
