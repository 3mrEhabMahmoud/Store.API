using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Services.Specifications
{
    public class OrderWithPaymentSpecifications: BaseSpecifications<Guid,Order>
    {
        public OrderWithPaymentSpecifications( string paymentIntentId) : base(o=>o.PaymentIntentId == paymentIntentId)
        {

        }


    }
}
