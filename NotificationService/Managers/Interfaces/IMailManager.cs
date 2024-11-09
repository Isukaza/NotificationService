using Amazon.SimpleEmailV2.Model;
using NotificationService.Models.Response;

namespace NotificationService.Managers.Interfaces;

public interface IMailManager
{
    Task<EmailApiResponse<GetEmailTemplateResponse>> GetTemplateAsync(string templateName);

    Task<EmailApiResponse<ListEmailTemplatesResponse>> GetAllTemplatesAsync();

    Task<EmailApiResponse<CreateEmailTemplateResponse>> CreateTemplateAsync(
        Models.Request.CreateEmailTemplateRequest request);

    Task<EmailApiResponse<DeleteEmailTemplateResponse>> DeleteTemplate(string templateName);
}