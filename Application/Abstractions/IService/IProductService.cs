using Application.Dto;

namespace Application.Abstractions.IService;

public interface IProductService
{
    Task<BaseResponse<ProductDto>> CreateProductAsync(AddProductRequestModel model);
    Task<BaseResponse<ProductDto>> UpdateProductAsync(string id,UpdateProductRequestModel model);
    Task<BaseResponse<ProductDto>> GetAsync(string id);
    Task<BaseResponse<ProductDto>> DeleteAsync( string id);
    Task<BaseResponse<IEnumerable<ProductDto>>> GetByProductCategoryAsync(string categoryId);
    Task<BaseResponse<IEnumerable<ProductDto>>> GetProductsByPriceAsync(decimal price);
    Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync();
    Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAvailableProduct();
}
