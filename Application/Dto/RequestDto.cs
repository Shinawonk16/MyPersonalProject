using Domain.Enum;

namespace Application.Dto;

public class RequestDto
{
   
    public string ProductId { get; set; }
    public string Id { get; set; }
    public string AdditionalNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public decimal Cost { get; set; }
    public ProductDto Product { get; set; }
    public bool IsApproved { get; set; }
    public int Quantity { get; set; }
    public ApprovalStatus EnumApprovalStatus { get; set; }
    public string StringApprovalStatus { get; set; }
    public string CreatedTime { get; set; }
    public string AdminImage { get; set; }
    public string PostedTime { get; set; }
    public string AdminName { get; set; }
}
public class RejectRequestRequestModel
{
    public string Message { get; set; }
}
public class CreateRequestRequestModel
{
    public decimal Cost { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }
    public string AdditionalNote { get; set; }




}
public class UpdateRequestRequestModel : CreateRequestRequestModel
{
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}
