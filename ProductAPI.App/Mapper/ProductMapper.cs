using eCommerce.SharedLibrary.DTO;
using ProductAPI.Domain.Entities;

namespace ProductAPI.App.Mapper
{
    public static class ProductMapper
    {
        public static Product ToEntity(ProductDTO productDTO) => new Product() { Id = productDTO.Id, Name = productDTO.Name, Price = productDTO.Price, Quantity = productDTO.Quantity};

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            if (product is not null)
            {
                var singleProduct = new ProductDTO(product.Id, product.Name, product.Quantity, product.Price);
                return (singleProduct, null);
            }
            else if(products is not null)
            {
                var _products = products.Select(p => new ProductDTO(p.Id, p.Name, p.Quantity, p.Price));
                return (null, _products);
            }
            return (null, null);
        }
    }
}
