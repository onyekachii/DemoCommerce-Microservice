using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.App.DTO;
using ProductAPI.App.Mapper;
using ProductAPI.Domain.Interface;

namespace ProductAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (products is not null)
            {
                var (_, productsDTO) = ProductMapper.FromEntity(null, products);
                if(productsDTO?.Count() > 1)
                    return Ok(productsDTO);
            }
            return NotFound("No product found");
        }

        [HttpGet("/id")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await productInterface.FindByIdAsync(id);
            if (product is not null)
            {
                var (productDto, _) = ProductMapper.FromEntity(product, null);
                return Ok(productDto);
            }
            return NotFound($"Product with id {id} not found");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {
                if (productDto is null)
                    return BadRequest("Product data is null");

                var product = ProductMapper.ToEntity(productDto);
                var response = await productInterface.CreateAsync(product);
                return response.Flag ? Ok() : BadRequest(response.Message);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDTO productDto)
        {
            if (ModelState.IsValid)
            {
                if (productDto is null)
                    return BadRequest("Product data is null");

                var product = ProductMapper.ToEntity(productDto);
                var response = await productInterface.UpdateAsync(product);
                return response.Flag ? Ok() : BadRequest(response.Message);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await productInterface.FindByIdAsync(id);
            if (product is not null)
            {
                var response = await productInterface.DeleteAsync(product);
                return response.Flag ? Ok() : BadRequest(response.Message);
            }
            return NotFound($"Product with id {id} not found");
        }
    }
}
