using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<BaseResponse<CategoryDto>> AddCategoryAsync(CreateCategoryRequestModel model)
    {
        var check = await _categoryRepository.GetCategoryAsync(
            x => x.CategoryName.ToLower() == model.CategoryName.ToLower()
        );
        if (check == null)
        {
            var category = new Category
            {
                CategoryDescription = model.CategoryDescription,
                CategoryName = model.CategoryName,
            };
            await _categoryRepository.CreateAsync(category);
            await _categoryRepository.SaveAsync();
            return new BaseResponse<CategoryDto>
            {
                Message = "Successful",
                Status = true,
                Data = new CategoryDto
                {
                    CategoryDescription = category.CategoryDescription,
                    CategoryName = category.CategoryName,
                    CreatedAt = category.CreatedAt
                }
            };
        }
        return new BaseResponse<CategoryDto> { Message = "Failed", Status = false, };
    }

    public async Task<BaseResponse<CategoryDto>> DeleteAsync(string id)
    {
        var get = await _categoryRepository.GetCategoryAsync(id);
        if (get != null)
        {
            get.IsDeleted = true;
            get.DeletedBy = "Super-Admin";
            await _categoryRepository.SaveAsync();
            return new BaseResponse<CategoryDto>
            {
                Message = "Successfully deleted",
                Status = true,
            };
        }
        return new BaseResponse<CategoryDto> { Message = "Fail to deleted", Status = false, };
    }

    public async Task<BaseResponse<IList<CategoryDto>>> GetAllCategory()
    {
        var check = await _categoryRepository.GetAllCategoryAsync();
        if (check != null)
        {
            return new BaseResponse<IList<CategoryDto>>
            {
                Message = "Successful",
                Status = true,
                Data = check
                    .Select(
                        x =>
                            new CategoryDto
                            {
                                CategoryDescription = x.CategoryDescription,
                                CategoryName = x.CategoryName.ToLower(),
                                Id = x.Id,
                                CreatedAt = x.CreatedAt,

                            }
                    )
                    .ToList()
            };
        }
        return new BaseResponse<IList<CategoryDto>> { Message = "Failed", Status = false, };
    }



    public async Task<BaseResponse<CategoryDto>> GetById(string id)
    {
        var check = await _categoryRepository.GetCategoryAsync(x => x.Id == id);
        if (check != null)
        {
            return new BaseResponse<CategoryDto>
            {
                Message = "Successful",
                Status = true,
                Data = new CategoryDto
                {
                    CategoryDescription = check.CategoryDescription,
                    CategoryName = check.CategoryName,
                    CreatedAt = check.CreatedAt
                }
            };
        }
        return new BaseResponse<CategoryDto> { Message = "Failed", Status = false, };
    }

    public async Task<BaseResponse<CategoryDto>> UpdateCategoryAsync(
        string id,
        UpdateCategoryRequestModel model
    )
    {
        var check = await _categoryRepository.GetCategoryAsync(x => x.Id == id);
        if (check != null)
        {
            check.CategoryName = model.CategoryName.ToLower() ?? check.CategoryName;
            check.CategoryDescription = model.CategoryDescription ?? check.CategoryDescription;
            await _categoryRepository.UpdateAsync(check);
            await _categoryRepository.SaveAsync();
            return new BaseResponse<CategoryDto>
            {
                Message = "Successful",
                Status = true,
                Data = new CategoryDto
                {
                    CategoryDescription = check.CategoryDescription,
                    CategoryName = check.CategoryName,
                    CreatedAt = check.CreatedAt
                }
            };
        }
        return new BaseResponse<CategoryDto> { Message = "Failed", Status = false, };
    }

}
