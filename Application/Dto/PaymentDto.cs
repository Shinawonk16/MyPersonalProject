namespace Application.Dto;

public class PaymentDto
{
    public int OrderId { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ResponseContent { get; set; }
}
public class CreatePaymentRequestModel
{
    public int Amount { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

}

public class UpdatePaymentRequestModel
{
    public bool Successful { get; set; }
}