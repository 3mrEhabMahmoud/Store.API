using AutoMapper;
using Store.Domain.Contracts;
using Store.Domain.Entities.Orders;
using Store.Domain.Entities.Products;
using Store.Domain.Exceptions;
using Store.Services.Abstractions.Orders;
using Store.Services.Specifications;
using Store.Services.Specifications.Orders;
using Store.Shard.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Orders
{
    public class OrderService(IUnitofWork _unitofWork,IMapper _mapper,IBasketRepository _basketRepository) : IOrderService
    {
        public async Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            //Get Order Address
            var OrderAddress = _mapper.Map<OrderAddress>(request.ShioToAddress);

            //2.Get Delivery Method By Id

            var deliveryMethod = await _unitofWork.GetRepository<int, DeliveryMethod>().GetAsync(request.DeliveryMethoId);
            if (deliveryMethod is null) throw new DeliveryMethodNotFoundException(request.DeliveryMethoId);


            //3. Get Order Items

              //1.Get Basket By Id
              var basket = await _basketRepository.GetBasketAsync(request.BasketId);
              if (basket is null) throw new BadImageFormatException(request.BasketId);

            //2.Convert Every Basket Item To Order Item
            var orderItems = new List<OrderItem>();

            foreach(var item in basket.Items)
            {
                //check Price
                //Get product from Db
                var product = await _unitofWork.GetRepository<int, Product>().GetAsync(item.Id);
                if (product is null) throw new ProductNotFoundExceptions(item.Id);

                if (product.Price != item.Price) item.Price = product.Price;

                var productInOrderItem = new ProductInOrderItem(item.Id, item.ProductName, item.PictureUrl);
                var orderItem = new OrderItem(productInOrderItem, item.Price, item.Quantity);
                orderItems.Add(orderItem);

            }

            //4. Calculate Subtotal

            var subTotal = orderItems.Sum(oi => oi.Price * oi.Quantity);

            //5. ToDO :: Pyment Inteny Id
            //check Order Exists

            var spec = new OrderWithPaymentSpecifications(basket.PaymentIntenId);
            var existsOrder = await _unitofWork.GetRepository<Guid,Order>().GetAsync(spec);

            if (existsOrder is not null)
                _unitofWork.GetRepository<Guid,Order>().Delete(existsOrder);
          

            //Create order

            var order = new Order(userEmail, OrderAddress, deliveryMethod, orderItems, subTotal,basket.PaymentIntenId);

            //Add order in Database
            await _unitofWork.GetRepository<Guid, Order>().AddAsync(order);
            var Count = await _unitofWork.SaveChangesAsync();
            if (Count <= 0) throw new CreateOrderBadRequestException();

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethods = await _unitofWork.GetRepository<int, DeliveryMethod>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }

        public async Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string UserEmail)
        {
            var spec = new OrderSpecification(id, UserEmail);
            var order = await _unitofWork.GetRepository<Guid, Order>().GetAsync(spec);
            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersForSpecificUserAsync(string UserEmil)
        {
            var spec = new OrderSpecification(UserEmil);
            var order = await _unitofWork.GetRepository<Guid, Order>().GetAllAsync(spec);
            return _mapper.Map<IEnumerable<OrderResponse>>(order);
        }
    }
}
