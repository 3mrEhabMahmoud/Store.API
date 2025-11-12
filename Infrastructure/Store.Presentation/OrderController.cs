using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using Store.Shard.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost]  
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);
            var result = await serviceManager.OrderService.CreateOrderAsync(request, userEmailClaim.Value);
            return Ok(result);
        }
    }
}
