using Application.Abstractions;
using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;
using Domain.Entities;

namespace Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IShopezyUpload _image;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IBrandRepository brandRepository, IShopezyUpload image)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _image = image;
    }


    public async Task<BaseResponse<ProductDto>> CreateProductAsync(AddProductRequestModel model)
    {
        var check = await _productRepository.GetAsync(x => x.ProductName == model.ProductName.ToLower());
        if (check == null)
        {
            var category = await _categoryRepository.GetCategoryAsync(x => x.CategoryName.ToLower() == model.CategoryName.ToLower());
            if (category == null)
            {
                return new BaseResponse<ProductDto>
                {
                    Message = $"{model.CategoryName} not found",
                    Status = false,
                };
            }
            var brand = await _brandRepository.GetBrand(x => x.BrandName.ToLower() == model.BrandName.ToLower());
            if (brand == null)
            {
                return new BaseResponse<ProductDto>
                {
                    Message = $"{model.BrandName} not found",
                    Status = false,
                };
            }
            var images = await _image.UploadFiles(model.Image);
            var product = new Product
            {
                ProductName = model.ProductName.ToLower(),
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                Image = images,
                IsAvailable = true,
                CategoryId = category.Id,
                BrandId = brand.Id,

            };
            await _productRepository.CreateAsync(product);
            await _productRepository.SaveAsync();
            return new BaseResponse<ProductDto>
            {
                Message = "Successful added a product",
                Status = true,
                Data = new ProductDto
                {
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,
                    Image = product.Image,
                    IsAvailable = true,
                    Id = product.Id,
                    Quantity = product.Quantity,
                    CategoryDto = new CategoryDto
                    {
                        CategoryName = category.CategoryName,
                        CategoryDescription = category.CategoryDescription
                    },
                    BrandsDto = new BrandsDto
                    {
                        BrandDescription = brand.BrandDescription,
                        BrandName = brand.BrandName
                    }
                }
            };
        }
        else if (check != null && check.ProductName == model.ProductName.ToLower())
        {
            var image = await _image.UploadFiles(model.Image);
            var prod = new Product
            {
                ProductName = model.ProductName.ToLower(),
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                Image = image,

            };
            await _productRepository.UpdateAsync(prod);
            await _productRepository.SaveAsync();
            return new BaseResponse<ProductDto>
            {
                Message = $"{model.ProductName} Already exist but updated Successfully",
                Status = true,
                Data = new ProductDto
                {
                    ProductName = prod.ProductName,
                    Description = prod.Description,
                    Price = prod.Price,
                    Image = prod.Image,
                    Quantity = prod.Quantity,

                }

            };
        }

        return new BaseResponse<ProductDto>
        {
            Message = $"{model.ProductName} not added",
            Status = false,

        };
    }

    public async Task<BaseResponse<ProductDto>> DeleteAsync(string id)
    {
        var get = await _productRepository.GetAsync(id);
        if (get != null)
        {
            get.IsDeleted = true;
            get.DeletedBy = "Super-Admin";
            await _productRepository.SaveAsync();
            return new BaseResponse<ProductDto>
            {
                Message = "Successfully deleted",
                Status = true,
            };
        }
        return new BaseResponse<ProductDto> { Message = "Fail to deleted", Status = false, };

    }

    public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAsync()
    {
        var all = await _productRepository.GetAllProductAsync();
        if (all != null)
        {
            return new BaseResponse<IEnumerable<ProductDto>>
            {
                Message = "Product list found successfully",
                Status = true,
                Data = all.Select(x => new ProductDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Description = x.Description,
                    IsAvailable = true,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Image = x.Image,
                    CategoryDto = new CategoryDto
                    {
                        CategoryName = x.Category.CategoryName
                    },
                    BrandsDto = new BrandsDto
                    {
                        BrandName = x.Brand.BrandName
                    }

                })
            };
        }
        return new BaseResponse<IEnumerable<ProductDto>>
        {
            Message = "operation failed",
            Status = false,

        };

    }

    public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAllAvailableProduct()
    {
        var all = await _productRepository.GetSelectedAsync(x => x.IsAvailable == true);
        if (all != null)
        {
            return new BaseResponse<IEnumerable<ProductDto>>
            {
                Message = "Product list found successfully",
                Status = true,
                Data = all.Select(x => new ProductDto
                {
                    Id = x.Id,
                    ProductName = x.ProductName,
                    Description = x.Description,
                    IsAvailable = true,
                    Price = x.Price,
                    Image = x.Image,
                    CategoryDto = new CategoryDto
                    {
                        CategoryName = x.Category.CategoryName
                    },
                    BrandsDto = new BrandsDto
                    {
                        BrandName = x.Brand.BrandName
                    }

                })
            };
        }
        return new BaseResponse<IEnumerable<ProductDto>>
        {
            Message = "operation failed",
            Status = false,

        };
    }

    public async Task<BaseResponse<ProductDto>> GetAsync(string id)
    {
        var get = await _productRepository.GetAsync(x => x.Id == id);
        if (get == null)
        {
            return new BaseResponse<ProductDto>
            {
                Message = "product not found",
                Status = false,
            };
        }
        return new BaseResponse<ProductDto>
        {
            Message = $"{get.ProductName} found successfully",
            Status = true,
            Data = new ProductDto
            {
                Id = get.Id,
                ProductName = get.ProductName,
                IsAvailable = true,
                Price = get.Price,
                Quantity = get.Quantity,
                Description = get.Description,
                Image = get.Image,
                CategoryDto = new CategoryDto
                {
                    CategoryName = get.Category.CategoryName,
                },
                BrandsDto = new BrandsDto
                {
                    BrandName = get.Brand.BrandName
                }


            }
        };
    }

    public async Task<BaseResponse<IEnumerable<ProductDto>>> GetByProductCategoryAsync(string categoryId)
    {
        var get = await _productRepository.GetProductByCategoryAsync(categoryId);
        if (get == null)
        {
            return new BaseResponse<IEnumerable<ProductDto>>
            {
                Message = "Product list not found ",
                Status = false,
            };
        }
        return new BaseResponse<IEnumerable<ProductDto>>
        {
            Message = "Product list found successfully",
            Status = true,
            Data = get.Select(x => new ProductDto
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                IsAvailable = true,
                Price = x.Price,
                Image = x.Image,
                Quantity = x.Quantity,
                CategoryDto = new CategoryDto
                {
                    CategoryName = x.Category.CategoryName
                },
                BrandsDto = new BrandsDto
                {
                    BrandName = x.Brand.BrandName
                }

            })
        };
    }

    public async Task<BaseResponse<IEnumerable<ProductDto>>> GetProductsByPriceAsync(decimal price)
    {
        var get = await _productRepository.GetByPriceAsync(price);
        if (get == null)
        {
            return new BaseResponse<IEnumerable<ProductDto>>
            {

            };
        }
        return new BaseResponse<IEnumerable<ProductDto>>
        {
            Message = "Product list found successfully",
            Status = true,
            Data = get.Select(x => new ProductDto
            {
                ProductName = x.ProductName,
                Description = x.Description,
                Price = x.Price,
                Image = x.Image,
                IsAvailable = true,
                Quantity = x.Quantity,
                CategoryDto = new CategoryDto
                {
                    CategoryName = x.Category.CategoryName
                },
                BrandsDto = new BrandsDto
                {
                    BrandName = x.Brand.BrandName
                }

            }).ToList()
        };
    }

    public async Task<BaseResponse<ProductDto>> UpdateProductAsync(string id, UpdateProductRequestModel model)
    {
        var update = await _productRepository.GetAsync(x => x.Id == id);
        if (update == null)
        {
            return new BaseResponse<ProductDto>
            {
                Message = "not found",
                Status = false
            };
        }

        var images = await _image.UploadFiles(model.Image);
       
        update.ProductName = model.ProductName.ToLower() ?? update.ProductName;
        update.Price = model.Price;
        update.Quantity = model.Quantity;
        update.Image = images ?? update.Image;
        update.Description = model.Description ?? update.Description;
        await _productRepository.UpdateAsync(update);
        await _productRepository.SaveAsync();
        return new BaseResponse<ProductDto>
        {
            Message = "Successful update a product",
            Status = true,
            Data = new ProductDto
            {
                ProductName = update.ProductName,
                Description = update.Description,
                Price = update.Price,
                Image = update.Image,
                Quantity = update.Quantity,

            }
        };

    }

}
