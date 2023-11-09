namespace Application.Dto;

public class BaseResponse<T>
{
    public string Message { get; set; }
    public T Data { get; set; }
    public bool Status { get; set; }
}


public class PaymentResponse
{
    public string authorization_url { get; set; }
    public string access_code { get; set; }
    public string reference { get; set; }
    public PaymentDto PaymentDto{get;set;}
}