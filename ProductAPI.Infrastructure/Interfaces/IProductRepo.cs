using eCommerce.SharedLibrary.Interface;
using ProductAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductAPI.Infrastructure.Interfaces
{
    public interface IProductRepo: IGenericInterface<Product>
    {
    }
}
