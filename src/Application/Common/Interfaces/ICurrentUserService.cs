namespace tm20trial.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? IdentityId { get; }
    
    int UserId { get; }

    bool IsAdmin { get; }
    
    List<string> Roles { get;  }

}
