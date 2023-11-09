namespace Application.Dto;

public class BrandsDto
{
    
        public string BrandName{get;set;}
        public string BrandDescription{get;set;}
        public DateTime CreatedAt{get;set;}

        public IEnumerable<ProductDto> ProductDtos{get;set;}
    
}
public class CreateBrandsRequestModel
    {
        public string BrandName{get;set;}
        public string BrandDescription{get;set;}
    }
    
    public class UpdateBrandsRequestModel:CreateBrandsRequestModel
    {
    }
