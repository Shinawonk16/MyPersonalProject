namespace Application.Dto;

public class VerificationDto
{
    public int Code { get; set; }
    public string Id { get; set; }

    public bool isExpired { get; set; }
}
public class ResetPasswordRequestModel
{
    public string Email { get; set; }
}