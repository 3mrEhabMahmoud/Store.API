using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Presentation.Attributes;
using Store.Services.Abstractions;
using Store.Shard;
using Store.Shard.Dtos.Products;
using Store.Shard.ErrorModels;
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
        [Cache(50)]
      
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetalis))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetalis))]
        public async Task<IActionResult> GetAllProducts([FromQuery]ProductQueryParameters parameters)
        {
            var result = await _serviceManager.ProductService.GetAllProductAsync(parameters);

            return Ok(result); //200
        }

        [HttpGet("{id}")]//GET: baseUrl/api/products/5
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetalis))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetalis))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetalis))]
        public async Task<IActionResult> GetProductById(int? id)
        {

            var result = await _serviceManager.ProductService.GetProductByIdAsync(id.Value);

            return Ok(result); //200
        }

        [HttpGet("brands")]//GET: baseUrl/api/products/brands
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetalis))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetalis))] 
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await _serviceManager.ProductService.GetAllBrandsAsync();
            if (result is null) return BadRequest();
            return Ok(result); //200
        }

        [HttpGet("types")]//GET: baseUrl/api/products/types
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetalis))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetalis))]
        public async Task<IActionResult> GetAlltypes()
        {
            var result = await _serviceManager.ProductService.GetAllTypesAsync();
            if (result is null) return BadRequest();
            return Ok(result); //200
        }
    }
}
