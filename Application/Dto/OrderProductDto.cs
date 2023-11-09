namespace Application.Dto;

public class OrderProductDto
{
  public ProductDto ProductDto { get; set; }
  public decimal AmountPaid { get; set; }
  public List<OrderDto> OrderDto { get; set; }
  public AddressDto AddressDto { get; set; }
  public int Quantity { get; set; }
}
