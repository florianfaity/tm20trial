namespace tm20trial.Application.Common.Models;

public class Email
{
    public EmailPerson? From { get; set; }
        
    public IEnumerable<EmailPerson>? Cc { get; set; }

    public IEnumerable<EmailPerson>? To { get; set; }

    public string? Subject { get; set; }

    public string? Content { get; set; }
}

public class EmailPerson
{
    public required string Email { get; set; }

    public string? Name { get; set; }
}
