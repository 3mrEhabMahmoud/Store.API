using Store.Shard.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Abstractions.Orders
{
    public interface IOrderService
    {
        Task<OrderResponse?> CreateOrderAsync(OrderRequest request, string userEmail);
        Task<IEnumerable<DeliveryMethodResponse>> GetAllDeliveryMethodAsync();
        Task<OrderResponse?> GetOrderByIdForSpecificUserAsync(Guid id, string UserEmail);
        Task<IEnumerable<OrderResponse>> GetOrdersForSpecificUserAsync(string UserEmil);
    }
}
