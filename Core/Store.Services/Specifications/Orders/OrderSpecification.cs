using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications.Orders
{
    public class OrderSpecification : BaseSpecifications<Guid, Order>
    {
        public OrderSpecification(Guid id, string userEmail) : base(O=>O.Id == id && O.UserEmail.ToLower() == userEmail.ToLower())
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }

        public OrderSpecification(string userEmail) : base(o=>o.UserEmail.ToLower() == userEmail.ToLower())
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDescending(o => o.OrderDate);
        }
    }
}
