namespace OrderMicroservice
{
    public record CreateOrderItemDTO(int productID, int quantity, decimal unitPrice);
}
