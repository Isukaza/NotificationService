using Amazon.SimpleEmailV2.Model;

using RabbitMQ.Messaging.Models;

using NotificationService.Configuration;
using NotificationService.DAL.Models;
using NotificationService.Factories.Interfaces;

namespace NotificationService.Factories;

public class SendEmailRequestFactory : IEmailRequestFactory
{
    public SendEmailRequest CreateSendEmailRequest(UserUpdateMessage message)
    {
        var template = GenerateConfirmationContent(message);
        return new SendEmailRequest
        {
            FromEmailAddress = MailConfig.Values.Mail,
            Destination = new Destination
            {
                ToAddresses = [message.UserEmail]
            },
            Content = new EmailContent
            {
                Template = new Template
                {
                    TemplateName = message.ChangeType.ToString(),
                    TemplateData = template
                }
            }
        };
    }

    public GetEmailTemplateRequest CreateGetEmailTemplateRequest(string templateName) =>
        new()
        {
            TemplateName = templateName
        };

    public ListEmailTemplatesRequest CreateListEmailTemplatesRequest() => new();

    public CreateEmailTemplateRequest CreateEmailTemplateRequest(
        NotificationService.Models.Request.CreateEmailTemplateRequest createRequest) =>
        new()
        {
            TemplateName = createRequest.TemplateName,
            TemplateContent = new EmailTemplateContent
            {
                Subject = createRequest.Subject,
                Html = createRequest.HtmlContent
            }
        };

    public DeleteEmailTemplateRequest CreateDeleteEmailTemplateRequest(string templateName) =>
        new()
        {
            TemplateName = templateName
        };

    private static string GenerateConfirmationContent(UserUpdateMessage message) =>
        message.ChangeType switch
        {
            TokenType.RegistrationConfirmation =>
                $"{{\"username\":\"{message.UserName}\", " +
                $"\"confirmationLink\":\"{message.ConfirmationLink}\"}}",

            TokenType.PasswordReset =>
                $"{{\"username\":\"{message.UserName}\", " +
                $"\"confirmationLink\":\"{message.ConfirmationLink}\"}}",

            TokenType.PasswordChange =>
                $"{{\"username\":\"{message.UserName}\", " +
                $"\"confirmationLink\":\"{message.ConfirmationLink}\"}}",

            TokenType.UsernameChange =>
                $"{{\"newUsername\":\"{message.NewValue}\", " +
                $"\"oldUsername\":\"{message.OldValue}\", " +
                $"\"confirmationLink\":\"{message.ConfirmationLink}\"}}",

            TokenType.EmailChangeNew =>
                $"{{\"username\":\"{message.UserName}\", " +
                $"\"newEmail\":\"{message.NewValue}\", " +
                $"\"confirmationLink\":\"{message.ConfirmationLink}\"}}",

            TokenType.EmailChangeOld =>
                $"{{\"username\":\"{message.UserName}\", " +
                $"\"newEmail\":\"{message.NewValue}\", " +
                $"\"oldEmail\":\"{message.OldValue}\", " +
                $"\"confirmationLink\":\"{message.ConfirmationLink}\"}}",

            _ => string.Empty
        };
}