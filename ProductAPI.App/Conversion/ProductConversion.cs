using ProductAPI.App.DTO;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.App.Conversion
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO productDTO) => new Product() { Id = productDTO.Id, Name = productDTO.Name, Price = productDTO.Price, Quantity = productDTO.Quantity};

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            if (product is not null || products is null)
            {
                var singleProduct = new ProductDTO(product.Id, product.Name, product.Quantity, product.Price);
                return (singleProduct, null);
            }
            if(products is not null || product is null)
            {
                var _products = products.Select(p => new ProductDTO(p.Id, p.Name, p.Quantity, p.Price));
                return (null, _products);
            }
            return (null, null);
        }
    }
}
