using Microsoft.AspNetCore.Http;

namespace Application.Dto;


public class ProductDto
{
    public string Id { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public int Quantity { get; set; }
    public CategoryDto CategoryDto { get; set; }
    public BrandsDto BrandsDto { get; set; }

    public DateTime AvailableTime { get; set; }

}
public class AddProductRequestModel
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public IFormFile Image { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public string BrandName { get; set; }

}
public class UpdateProductRequestModel
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public IFormFile Image { get; set; }
    public string Description { get; set; }

}