using tm20trial.Application.Common.Enums;
using tm20trial.Application.Common.Models;
using Newtonsoft.Json.Linq;

namespace tm20trial.Application.Common.Interfaces;

public interface IEmailClient
{
    // public Task SendTextEmailAsync(Email email);
    //
    // public Task SendHtmlEmailAsync(Email email);

    public Task SendEmailTemplateAsync(Email email, int templateId, JObject variable, JArray? attachment = null);

    public Task<int> GetTemplateIdAsync(TypeTemplateEmail typeTemplateEmail);
}
