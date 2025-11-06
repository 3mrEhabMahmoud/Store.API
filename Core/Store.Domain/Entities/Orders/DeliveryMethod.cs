using Microsoft.EntityFrameworkCore.Query;

namespace Store.Domain.Entities.Orders
{
    public class DeliveryMethod : BaseEntity<int>
    {
        public string ShortName{set; get;}
        public string Description { set; get; }
        public string DeliveryTime { set; get; }
        public decimal Price { set; get; }

    }
}