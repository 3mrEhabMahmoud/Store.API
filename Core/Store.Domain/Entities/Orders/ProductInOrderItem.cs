namespace Store.Domain.Entities.Orders
{
    //Part of OrderItem Table
    public class ProductInOrderItem
    {
        public ProductInOrderItem()
        {
            
        }
        public ProductInOrderItem(int productId, string productName, string prictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PrictureUrl = prictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PrictureUrl { get; set; }


    }
}