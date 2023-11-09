using Application.Abstractions.IService;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Shopezy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{

    private readonly IPaymentService _paymentService;
    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("InitiatePayment/{id}/{orderId}")]
    public async Task<IActionResult> MakePayment([FromBody] CreatePaymentRequestModel model, [FromRoute] string id, [FromRoute] string orderId)
    {
        var payment = await _paymentService.InitiatePayment(model, id, orderId);
        return Ok(payment);
    }
    [HttpGet("Get/{transactionReference}")]
    public async Task<IActionResult> GetTransactionReceipt([FromRoute] string transactionReference)
    {
        var transaction = await _paymentService.GetTransactionRecieptAsync(transactionReference);
        if (transaction == null)
        {
            return BadRequest(transaction);
        }
        return new OkObjectResult(transaction);
    }

}
