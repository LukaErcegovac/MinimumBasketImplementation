namespace OrderMicroservice
{
    public record CreateOrderDTO(int userID, List<CreateOrderItemDTO> orderItems);
}
