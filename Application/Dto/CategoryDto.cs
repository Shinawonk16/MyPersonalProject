namespace Application.Dto;

public class CategoryDto
{
    public string CategoryName { get; set; }
    public string Id { get; set; }
    public string CategoryDescription { get; set; }
    public List<ProductDto> ProductDtos { get; set; }
    public DateTime CreatedAt { get; set; }

}
public class CreateCategoryRequestModel
{
    public string CategoryName { get; set; }
    // public DateTime CreatedAt { get; set; }
    public string CategoryDescription { get; set; }
}

public class UpdateCategoryRequestModel : CreateCategoryRequestModel
{

}