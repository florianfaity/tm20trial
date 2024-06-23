using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using tm20trial.Application.Common.Configurations;
using tm20trial.Application.Common.Enums;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using Newtonsoft.Json.Linq;
using tm20trial.Domain.Exceptions;
using Mailjet.Client;
using Mailjet.Client.Resources;

namespace tm20trial.Infrastructure.Services;

public class MailJetService : IEmailClient

{
    private readonly MailJetConfiguration _configuration;
    private readonly ILogger<MailJetService> _logger;

    public MailJetService(IOptions<MailJetConfiguration> configuration, ILogger<MailJetService> logger)
    {
        _configuration = configuration.Value;
        _logger = logger;
    }

    // public Task SendTextEmailAsync(Email email)
    // {
    //     return SendEmailAsync(email, false);
    // }
    //
    // public Task SendHtmlEmailAsync(Email email)
    // {
    //     return SendEmailAsync(email, true);
    // }


    public Task SendEmailTemplateAsync(Email email, int templateId, JObject variable, JArray? attachment = null)
    {
        return SendMailWithTemplateAsync(email, templateId, variable, attachment);
    }

    public Task<int> GetTemplateIdAsync(TypeTemplateEmail typeTemplateEmail)
    {
        return Task.FromResult(typeTemplateEmail switch
        {
            TypeTemplateEmail.ResetPassword => 6078409,
            _ => 0
        });
    }

    private async Task SendMailWithTemplateAsync(Email email, int templateId, JObject variable,
        JArray? attachment = null)
    {
        if (email == null)
        {
            _logger.LogError("Email can't be null");
            throw new EmailException("Email can't be null");
        }

        if (email.To == null || !email.To.Any())
        {
            _logger.LogError("The email must have at least one recipient");
            throw new EmailException("The email must have at least one recipient");
        }

        email.From ??= _configuration.From;

        if (!string.IsNullOrEmpty(_configuration.DevelopmentEmail))
        {
            _logger.LogInformation($"Use email of dev or test: {_configuration.DevelopmentEmail}");

            email.To = new List<EmailPerson>
            {
                new EmailPerson { Email = _configuration.DevelopmentEmail, Name = "DEV - Debug" }
            };
            if (email.Cc != null)
            {
                email.Cc = new List<EmailPerson>
                {
                    new EmailPerson { Email = _configuration.DevelopmentEmail, Name = "DEV - Debug Cc" }
                };
            }
        }

        var client = new MailjetClient(_configuration.ApiKeyPublic, _configuration.ApiKeyPrivate);

        var request = new MailjetRequest { Resource = Send.Resource }
            .Property(Send.FromEmail, email.From.Email)
            .Property(Send.FromName, email.From.Name)
            .Property(Send.Subject, email.Subject)
            .Property(Send.To, String.Join(",", email.To.Select(x => $"{x.Name} <{x.Email}>")))
            .Property(Send.Cc, email.Cc != null ? String.Join(",", email.Cc.Select(x => $"{x.Name} <{x.Email}>")) : "")
            .Property(Send.Vars, variable)
            .Property(Send.MjTemplateID, templateId)
            .Property(Send.MjTemplateLanguage, true);

        if (attachment != null)
        {
            request = request.Property(Send.Attachments, attachment);
        }

        if (!string.IsNullOrEmpty(_configuration.DevelopmentEmail))
        {
            request = request.Property(Send.MjTemplateErrorReporting, _configuration.DevelopmentEmail);
        }

        var response = await client.PostAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                $"Error during send of the mail: Info = {response.GetErrorInfo()}, Message = {response.GetErrorMessage()}");
            throw new EmailException(
                $"Error during send of the mail: Info = {response.GetErrorInfo()}, Message = {response.GetErrorMessage()}");
        }
        else
        {
            _logger.LogDebug($"Email send with success");
        }
    }
}
