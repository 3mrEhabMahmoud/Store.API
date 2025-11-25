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
    public class OrdersController(IServiceManager serviceManager) : ControllerBase
    {
        [HttpPost]  
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderRequest request)
        {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);
            var result = await serviceManager.OrderService.CreateOrderAsync(request, userEmailClaim.Value);
            return Ok(result);
        }

        //get all deliveryMethods
        [HttpGet("deliveryMethods")]//GET: BaseUrl/api/orders/deliveryMethods
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var result = await serviceManager.OrderService.GetAllDeliveryMethodAsync();
            return Ok(result);
        }

        //get all order
        [HttpGet]//GET: BaseUrl/api/orders/orders
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser()
        {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);
            var result = await serviceManager.OrderService.GetOrdersForSpecificUserAsync(userEmailClaim.Value);
            return Ok(result);

        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderByIdForSpecificUserAsync(Guid id)
        {
            var userEmailClaim = User.FindFirst(ClaimTypes.Email);
            var result = await serviceManager.OrderService.GetOrderByIdForSpecificUserAsync(id, userEmailClaim.Value);
            return Ok(result);

        }
    }
}
