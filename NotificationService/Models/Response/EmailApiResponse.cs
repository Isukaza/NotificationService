using System.Net;
using Amazon.Runtime;

namespace NotificationService.Models.Response;

public class EmailApiResponse<T>(T response, string errorMessage = null)
    where T : AmazonWebServiceResponse, new()
{
    public T Response { get; set; } = response;
    public string ErrorMessage { get; set; } = errorMessage;

    public EmailApiResponse(HttpStatusCode statusCode, string errorMessage = null)
        : this(new T { HttpStatusCode = statusCode }, errorMessage)
    { }
}