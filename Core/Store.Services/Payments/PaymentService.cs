using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Domain.Contracts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions;
using Store.Services.Specifications;
using Store.Shard.Dtos;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Domain.Entities.Products.Product;

namespace Store.Services.Abstractions.Payments
{

    internal class PaymentService(IBasketRepository basketRepository,
                                IUnitofWork _unitofWork,
                                IConfiguration _configuration,
                                IMapper mapper) : IPaymentService
    {
        public async Task<BasketDto> CreatePaymentIntentAsync(string basketId)
        {
            //Calculate Amount = Subtotal + Deliverymethod Cost

            //get basket by id
            var basket = await basketRepository.GetBasketAsync(basketId);
            if (basket is null) throw new BasketNotFoundExcepetion(basketId);


            //check product and its price
            foreach (var item in basket.Items)
            {
                var product = await _unitofWork.GetRepository<int, Product>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundExceptions(item.Id);

                item.Price = product.Price;
            }

            //Calculate subtotal

            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);

            //Get Dlivery Method By id

            if (!basket.DeliveryMethodId.HasValue) throw new DeliveryMethodNotFoundException(-1);

            var deliveryMethod = await _unitofWork.GetRepository<int, DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingCost = deliveryMethod.Price;

            var amount = subTotal + deliveryMethod.Price;

            //send Amount To Stripe

            StripeConfiguration.ApiKey = _configuration["StripeOptions:SecretKey"];
            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (basket.PaymentIntenId is null)
            {
                //Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)amount * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(options);

            }
            else
            {
                //Update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)amount * 100,
                };
                paymentIntent = await paymentIntentService.UpdateAsync(basket.PaymentIntenId, options);
            }
            basket.PaymentIntenId = paymentIntent.Id;
            basket.ClientSecret = paymentIntent.ClientSecret;

            //Create
            basket = await basketRepository.UpdateBasketAsync(basket, TimeSpan.FromDays(1));
            return mapper.Map<BasketDto>(basket);
        }

        public async Task UpdateOrderPaymentStatusAsync(string jsonRequest, string stripeHeader)
        {
            var endpointSecret = _configuration.GetRequiredSection("Stripe")["EndPointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(jsonRequest,
                      stripeHeader, endpointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentPaymentFailed:
                    await UpdatePaymentFailedAsync(paymentIntent.Id);
                    break;
                case EventTypes.PaymentIntentSucceeded:
                    await UpdatePaymentReceivedAsync(paymentIntent.Id);
                    break;
                // ... handle other event types
                default:
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }


        }
        private async Task UpdatePaymentReceivedAsync(string paymentIntentId)
        {
            var order = await _unitofWork.GetRepository<Guid,Order>()
                .GetAsync(new OrderWithPaymentSpecifications(paymentIntentId));

            order.Status = OrderStatus.PaymentSuccess;

            _unitofWork.GetRepository<Guid,Order>().Update(order);

            await _unitofWork.SaveChangesAsync();
        }
        private async Task UpdatePaymentFailedAsync(string paymentIntentId)
        {
            var order = await _unitofWork.GetRepository<Guid,Order>()
                .GetAsync(new OrderWithPaymentSpecifications(paymentIntentId));

            order.Status = OrderStatus.PaymentFaild;


            _unitofWork.GetRepository<Guid,Order>().Update(order);

            await _unitofWork.SaveChangesAsync();
        }
    }
}
