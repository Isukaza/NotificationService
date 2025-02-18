using System.Net;

using Amazon.Runtime;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;

using RabbitMQ.Messaging.Models;

using NotificationService.Configuration;
using NotificationService.Factories.Interfaces;
using NotificationService.Managers.Interfaces;
using NotificationService.Models.Response;

namespace NotificationService.Managers;

public class MailManager(IEmailRequestFactory emailRequestFactory) : IMailManager
{
    #region Fields

    private readonly AmazonSimpleEmailServiceV2Client _sesClient = new(
        MailConfig.Values.AwsAccessKeyId,
        MailConfig.Values.AwsSecretAccessKey,
        MailConfig.Values.RegionEndpoint);

    #endregion

    #region Email Template Management

    public async Task<EmailApiResponse<SendEmailResponse>> SendEmailAsync(UserUpdateMessage message)
    {
        var request = emailRequestFactory.CreateSendEmailRequest(message);
        return await ExecuteSesRequestAsync(() => _sesClient.SendEmailAsync(request));
    }

    public async Task<EmailApiResponse<GetEmailTemplateResponse>> GetTemplateAsync(string templateName)
    {
        var request = emailRequestFactory.CreateGetEmailTemplateRequest(templateName);
        return await ExecuteSesRequestAsync(() => _sesClient.GetEmailTemplateAsync(request));
    }

    public async Task<EmailApiResponse<ListEmailTemplatesResponse>> GetAllTemplatesAsync()
    {
        var request = emailRequestFactory.CreateListEmailTemplatesRequest();
        return await ExecuteSesRequestAsync(() => _sesClient.ListEmailTemplatesAsync(request));
    }

    public async Task<EmailApiResponse<CreateEmailTemplateResponse>> CreateTemplateAsync(
        Models.Request.CreateEmailTemplateRequest createRequest)
    {
        var request = emailRequestFactory.CreateEmailTemplateRequest(createRequest);
        return await ExecuteSesRequestAsync(() => _sesClient.CreateEmailTemplateAsync(request));
    }

    public async Task<EmailApiResponse<DeleteEmailTemplateResponse>> DeleteTemplate(string templateName)
    {
        var request = emailRequestFactory.CreateDeleteEmailTemplateRequest(templateName);
        return await ExecuteSesRequestAsync(() => _sesClient.DeleteEmailTemplateAsync(request));
    }

    #endregion

    #region Request Execution

    private static async Task<EmailApiResponse<T>> ExecuteSesRequestAsync<T>(Func<Task<T>> action)
        where T : AmazonWebServiceResponse, new()
    {
        try
        {
            var response = await action();
            var errorMessage = response.HttpStatusCode == HttpStatusCode.OK ? null : "Internal Server Error";
            return new EmailApiResponse<T>(response, errorMessage);
        }
        catch (AccountSuspendedException)
        {
            return new EmailApiResponse<T>(HttpStatusCode.Forbidden,
                "The account's ability to send email has been permanently restricted");
        }
        catch (MailFromDomainNotVerifiedException)
        {
            return new EmailApiResponse<T>(HttpStatusCode.BadRequest, "The sending domain is not verified");
        }
        catch (MessageRejectedException)
        {
            return new EmailApiResponse<T>(HttpStatusCode.BadRequest, "The message content is invalid");
        }
        catch (SendingPausedException)
        {
            return new EmailApiResponse<T>(HttpStatusCode.Forbidden,
                "The account's ability to send email is currently paused");
        }
        catch (TooManyRequestsException)
        {
            return new EmailApiResponse<T>(HttpStatusCode.TooManyRequests,
                "Too many requests were made. Please try again later");
        }
        catch (AmazonSimpleEmailServiceV2Exception ex)
            when (ex.StatusCode == HttpStatusCode.Unauthorized || ex.StatusCode == HttpStatusCode.Forbidden)
        {
            return new EmailApiResponse<T>(ex.StatusCode, "Invalid security token");
        }
        catch (AmazonSimpleEmailServiceV2Exception ex)
        {
            return new EmailApiResponse<T>(ex.StatusCode, $"An error occurred: {ex.Message}");
        }
    }

    #endregion
}