using Microsoft.AspNetCore.Http;

namespace tm20trial.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? IdentityId { get; }
    
    int UserId { get; }

    bool IsAdmin { get; set; }
    
    List<string> Roles { get; set; }

}
