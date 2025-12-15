namespace MinimumBasketImplementation.DTO
{
    public class CreateOrderItemsDTO
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public CreateOrderItemsDTO(int orderId, int quantity, decimal price)
        {
            ProductID = orderId;
            Quantity = quantity;
            UnitPrice = price;
        }

        public CreateOrderItemsDTO() { }
    }
}
