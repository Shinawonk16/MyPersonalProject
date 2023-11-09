using Application.Dto;

namespace Application.Abstractions.IService;

public interface IOrderService
{
    Task<BaseResponse<OrderDto>> CreateOrderAsync(CreateOrderRequestModel model, string customerId);
    Task<BaseResponse<OrderDto>> UpdateOrderAsync(string id, UpdateOrderRequestModel model);
    Task<BaseResponse<IEnumerable<OrderProductDto>>> GetAllDeliveredOrdersAsync();
    Task<BaseResponse<IEnumerable<OrderProductDto>>> GetAllOrdersAsync();
    Task<BaseResponse<IEnumerable<OrderProductDto>>> GetAllUnDeliveredOrdersAsync();
    Task<BaseResponse<OrderProductDto>> GetOrderByIdAsync(string id);
    Task<BaseResponse<IList<OrderProductDto>>> GetOrdersByCustomerIdAsync(string customerId);

}
