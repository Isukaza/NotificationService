using Amazon.SimpleEmailV2.Model;
using RabbitMQ.Messaging.Models;

namespace NotificationService.Factories.Interfaces;

public interface IEmailRequestFactory
{
    SendEmailRequest CreateSendEmailRequest(UserUpdateMessage message);
    GetEmailTemplateRequest CreateGetEmailTemplateRequest(string templateName);
    ListEmailTemplatesRequest CreateListEmailTemplatesRequest();
    CreateEmailTemplateRequest CreateEmailTemplateRequest(
        NotificationService.Models.Request.CreateEmailTemplateRequest createRequest);
    DeleteEmailTemplateRequest CreateDeleteEmailTemplateRequest(string templateName);
}