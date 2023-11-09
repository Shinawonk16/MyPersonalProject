namespace Application.Dto;

public class ReviewDto
{
    public bool Seen { get; set; }
    public string Comment { get; set; }
    public string Id { get; set; }
    public CustomerDto CustomerDto { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ProductDto ProductDto { get; set; }
}

public class CreateReviewRequestModel
{
    public string Comment { get; set; }
}


public class UpdateReviewRequestModel
{
    public string Comment { get; set; }
}

