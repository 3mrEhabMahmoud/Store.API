using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Shard.Orders
{
    public class OrderRequest
    {
        public string BasketId { get; set; }
        public int DeliveryMethoId { get; set; }
        public OrderAddressDto ShioToAddress { get; set; }

    }
}
