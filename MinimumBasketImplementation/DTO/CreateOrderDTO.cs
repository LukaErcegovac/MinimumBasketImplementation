namespace MinimumBasketImplementation.DTO
{
    public class CreateOrderDTO
    {
        public int UserID { get; set; }
        public List<CreateOrderItemsDTO>? OrderItems { get; set; }

        public CreateOrderDTO(int userId, decimal totalPrice, List<CreateOrderItemsDTO> items)
        {
            UserID = userId;
            OrderItems = items;
        }
        public CreateOrderDTO() { }
    }
}
