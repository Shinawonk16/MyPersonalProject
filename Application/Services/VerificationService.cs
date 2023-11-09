using Application.Abstractions.IRepository;
using Application.Abstractions.IService;
using Application.Dto;

namespace Application.Services;

public class VerificationService : IVerificationService
{
    private readonly IVerificationRepository _verificationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEmailService _mailService;

    public VerificationService(IVerificationRepository verificationRepository, ICustomerRepository customerRepository, IEmailService mailService)
    {
        _verificationRepository = verificationRepository;
        _customerRepository = customerRepository;
        _mailService = mailService;
    }


    public async Task<BaseResponse<VerificationDto>> SendForgetPasswordVerificationCode(ResetPasswordRequestModel model)
    {
        var customer = await _customerRepository.GetAsync(x => x.User.Email == model.Email);
        if (customer == null)
        {
            return new BaseResponse<VerificationDto>
            {
                Message = "Email not found",
                Status = false
            };
        }
        var code = await _verificationRepository.GetAsync(x => x.CustomerId == customer.Id);
        if (code == null)
        {
            return new BaseResponse<VerificationDto>
            {
                Message = "No Code has been sent to at registration point",
                Status = false
            };
        }
        int random = new Random().Next(10000, 99999);
        code.Code = random;
        code.CreatedAt = DateTime.Now;
        var mailRequest = new MailRequest
        {
            Subject = "Reset Password",
            ToEmail = customer.User.Email,
            ToName = $"{customer.User.FirstName} {customer.User.LastName}",
            HtmlContent =
                $"<html><body><h1>Hello {customer.User.FirstName} {customer.User.LastName}, Welcome</h1><h4>Your Password reset code is {code.Code} to reset your password</h4></body></html>",
        };
        _mailService.SendEMailAsync(mailRequest);
        customer.User.IsDeleted = true;
        await _customerRepository.UpdateAsync(customer);
        await _customerRepository.SaveAsync();
        await _verificationRepository.UpdateAsync(code);
        await _verificationRepository.SaveAsync();

        return new BaseResponse<VerificationDto>
        {
            Message = "Reset Password Code Successfully Reset",
            Status = true,
            Data = new VerificationDto { Id = customer.Id, }
        };
    }

    public async Task<BaseResponse<VerificationDto>> UpdateVerificationCodeAsync(string id)
    {
        var customer = await _customerRepository.GetAsync(id);
        if (customer == null)
        {
            return new BaseResponse<VerificationDto>
            {
                Message = "Customer not found",
                Status = false
            };
        }
        var code = await _verificationRepository.GetAsync(x => x.CustomerId == id);
        if (code == null)
        {
            return new BaseResponse<VerificationDto>
            {
                Message = "No code has been sent to you before",
                Status = false
            };
        }
        int random = new Random().Next(10000, 99999);
        code.Code = random;
        code.CreatedAt = DateTime.Now;
        var mailRequest = new MailRequest
        {
            Subject = "Confirmation Code",
            ToEmail = customer.User.Email,
            ToName = $"{customer.User.FirstName} {customer.User.LastName}",
            HtmlContent =
                $"<html><body><h1>Hello {customer.User.FirstName} {customer.User.LastName}, Welcome to Dansnom Farm Limited.</h1><h4>Your confirmation code is {code.Code} to continue with the registration</h4></body></html>",
        };
        _mailService.SendEMailAsync(mailRequest);
        await _verificationRepository.UpdateAsync(code);
        await _verificationRepository.SaveAsync();
        return new BaseResponse<VerificationDto>
        {
            Message = "Code Successfully resent",
            Status = true
        };
    }

    public async Task<BaseResponse<VerificationDto>> VerifyCode(string id, int verificationcode)
    {
        var code = await _verificationRepository.GetAsync(x => x.Customer.Id == id && x.Code == verificationcode);
        if (code == null)
        {
            return new BaseResponse<VerificationDto>
            {
                Message = "invalid code",
                Status = false
            };
        }
        else if ((DateTime.Now - code.CreatedAt).TotalSeconds > 1000)
        {
            return new BaseResponse<VerificationDto>
            {
                Message = "Code Expired",
                Status = false,
            };
        }
        var customer = await _customerRepository.GetAsync(x => x.Id == id);
        customer.User.IsDeleted = false;
        await _customerRepository.UpdateAsync(customer);
        await _customerRepository.SaveAsync();

        return new BaseResponse<VerificationDto>
        {
            Message = "Email Successfully Verified",
            Status = true,
        };

    }

}
