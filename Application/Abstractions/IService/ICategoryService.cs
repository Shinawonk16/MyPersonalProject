using Application.Dto;

namespace Application.Abstractions.IService;

public interface ICategoryService
{
    Task<BaseResponse<IList<CategoryDto>>> GetAllCategory();
    Task<BaseResponse<CategoryDto>> GetById(string id);
    Task<BaseResponse<CategoryDto>> DeleteAsync(string id);

    Task<BaseResponse<CategoryDto>> AddCategoryAsync(CreateCategoryRequestModel model);
    Task<BaseResponse<CategoryDto>> UpdateCategoryAsync(string id, UpdateCategoryRequestModel model);

}
