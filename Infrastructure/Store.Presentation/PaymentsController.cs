using Microsoft.AspNetCore.Mvc;
using Store.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IServiceManager _serviceManager) : ControllerBase
    {
        //create paymentIntent
        [HttpPost("{basketId}")]
        public async Task<IActionResult> CreatePaymentIntent(string basketId)
        {
            var result = await _serviceManager.PaymentService.CreatePaymentIntentAsync(basketId);
            return Ok(result);
        }

        [HttpPost("WebHook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            await _serviceManager.PaymentService.UpdateOrderPaymentStatusAsync(json,
                Request.Headers["Stripe-Signature"]!);
            return new EmptyResult();
        }
    }
}
