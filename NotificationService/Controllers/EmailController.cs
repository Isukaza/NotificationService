using System.ComponentModel.DataAnnotations;
using System.Net;
using Amazon.SimpleEmailV2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.DAL.Roles;
using Helpers;
using NotificationService.Managers.Interfaces;
using CreateEmailTemplateRequest = NotificationService.Models.Request.CreateEmailTemplateRequest;

namespace NotificationService.Controllers;

[ApiController]
[Route("/[controller]")]
[Authorize(Policy = nameof(RolePolicy.RequireAnyAdmin))]
public class EmailController(IMailManager mailManager) : Controller
{
    /// <summary>
    /// Adds a new email template to the AWS SES.
    /// </summary>
    /// <param name="request">The details of the email template to be created (name, subject, and HTML content).</param>
    /// <returns>Returns the status of the creation operation.</returns>
    /// <response code="200">Template added successfully.</response>
    /// <response code="400">The request is invalid, such as when required template details are missing or incorrect.</response>
    /// <response code="500">An error occurred during the creation of the template.</response>
    [HttpPut("add-email-template")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddEmailTemplate([FromBody] CreateEmailTemplateRequest request)
    {
        var result = await mailManager.CreateTemplateAsync(request);
        return result.Response.HttpStatusCode == HttpStatusCode.OK
            ? await StatusCodes.Status200OK.ResultState("Template added successfully")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }

    /// <summary>
    /// Deletes an existing email template from the AWS SES.
    /// </summary>
    /// <param name="templateName">The name of the template to be deleted.</param>
    /// <returns>Returns the status of the deletion operation.</returns>
    /// <response code="200">Template was successfully deleted.</response>
    /// <response code="400">The request is invalid, such as when the template name is missing.</response>
    /// <response code="500">An error occurred during the deletion of the template.</response>
    [HttpDelete("delete-email-template")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteEmailTemplate([Required] string templateName)
    {
        var result = await mailManager.DeleteTemplate(templateName);
        return result.Response.HttpStatusCode == HttpStatusCode.OK
            ? await StatusCodes.Status200OK.ResultState("Template successfully removed")
            : await StatusCodes.Status500InternalServerError.ResultState();
    }
}