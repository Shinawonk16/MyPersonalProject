using Application.Dto;

namespace Application.Abstractions.IService;

public interface IVerificationService
{
    Task<BaseResponse<VerificationDto>> UpdateVerificationCodeAsync(string id);
    Task<BaseResponse<VerificationDto>> VerifyCode(string id, int verificationcode);
    Task<BaseResponse<VerificationDto>> SendForgetPasswordVerificationCode(ResetPasswordRequestModel model);

}
