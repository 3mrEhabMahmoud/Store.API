using AutoMapper;
using Store.Domain.Entities.Orders;
using Store.Shard.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Mapping.Orders
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<OrderAddress, OrderAddressDto>().ReverseMap();

            CreateMap<Order, OrderResponse>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.Total, o => o.MapFrom(s => s.GetTotal()));

            CreateMap<OrderItem, OrderItemDto>().ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                                               .ForMember(D => D.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                                               .ForMember(D => D.ProductUrl, o => o.MapFrom(s => s.Product.PrictureUrl));

            CreateMap<DeliveryMethod,DeliveryMethodResponse>();
        }
    }
}
