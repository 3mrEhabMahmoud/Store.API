using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Store.Services.Abstractions;
using Store.Shard.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("API/[Controller]")]
    public class BasketsController(IServiceManager serviceManager) :ControllerBase
    {
        [HttpGet] //Get: /api/baskets?id=sadas
        public async Task<IActionResult> GetBasketById(string id)
        {
            var result = await serviceManager.BasketService.GetBasketAsync(id);
            return Ok(result);
        }
        [HttpPost] //Post: /api/baskets
        public async Task<IActionResult> UpdateBasket(BasketDto basketDto)
        {
            var result = await serviceManager.BasketService.UpdateBasketAsync(basketDto);
            return Ok(result);
        }
        [HttpDelete] //Delete: /api/baskets
        public async Task<IActionResult> DeleteBasket(string id)
        {
           await serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent(); //204
        }
    }
}
