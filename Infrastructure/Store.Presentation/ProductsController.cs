using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Shard.Dtos.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController(IServiceManager _serviceManager): ControllerBase
    {
        [HttpGet]//GET: baseUrl/api/products
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
            var result = await _serviceManager.ProductService.GetAllProductAsync(parameters);
            if (result is null) return BadRequest();
            return Ok(result); //200
        }

        [HttpGet("{id}")]//GET: baseUrl/api/products/5
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest();
            var result = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);
            if (result is null) return NotFound();//404
            return Ok(result); //200
        }

        [HttpGet("{brands}")]//GET: baseUrl/api/products/brands
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();
            return Ok(result); //200
        }

        [HttpGet("{types}")]//GET: baseUrl/api/products/types
        public async Task<IActionResult> GetAlltypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest();
            return Ok(result); //200
        }
    }
}
