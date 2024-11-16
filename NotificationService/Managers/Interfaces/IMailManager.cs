using Amazon.SimpleEmailV2.Model;
using RabbitMQ.Messaging.Models;
using NotificationService.Models.Response;

namespace NotificationService.Managers.Interfaces;

public interface IMailManager
{
    Task<EmailApiResponse<SendEmailResponse>> SendEmailAsync(UserUpdateMessage message);

    Task<EmailApiResponse<GetEmailTemplateResponse>> GetTemplateAsync(string templateName);

    Task<EmailApiResponse<ListEmailTemplatesResponse>> GetAllTemplatesAsync();

    Task<EmailApiResponse<CreateEmailTemplateResponse>> CreateTemplateAsync(
        Models.Request.CreateEmailTemplateRequest request);

    Task<EmailApiResponse<DeleteEmailTemplateResponse>> DeleteTemplate(string templateName);
}