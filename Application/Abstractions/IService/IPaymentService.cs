using Application.Dto;

namespace Application.Abstractions.IService;

public interface IPaymentService
{
    Task<BaseResponse<PaymentResponse>> InitiatePayment(CreatePaymentRequestModel model, string customerId, string orderId);
    // Task<string> InitiatePayment(CreatePaymentRequestModel model, string customerId, string orderId);
    Task<BaseResponse<PaymentResponse>> GetTransactionRecieptAsync(string transactionReference);
    Task<string> VerifyPayment(UpdatePaymentRequestModel model, string customerId);

}
