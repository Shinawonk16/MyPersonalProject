using Application.Dto;

namespace Application.Abstractions.IService;

public interface IEmailService
{
    public void SendEMailAsync(MailRequest mailRequest);
}
