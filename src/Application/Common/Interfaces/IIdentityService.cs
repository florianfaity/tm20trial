using tm20trial.Application.Common.Models;
using tm20trial.Domain.Interfaces;

namespace tm20trial.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);
    
    Task<IApplicationUser> FindUserByIdAsync(string id, CancellationToken token = default);

    Task<IApplicationUser> FindUserByIdAsync(int id, CancellationToken token = default);
}
