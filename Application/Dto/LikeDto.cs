namespace Application.Dto;
public class LikeDto
{
    public int NumberOfLikes { get; set; }
    public ICollection<CustomerDto> CustomerDto { get; set; }

}
public class CreateLikeRequestModel
{
    public string UserId { get; set; }
    public string ReviewId { get; set; }
}

