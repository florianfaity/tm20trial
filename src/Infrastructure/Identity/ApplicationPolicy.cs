using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using tm20trial.Application.Common.Models;

namespace tm20trial.Infrastructure.Identity;

public class ApplicationPolicy
{
    public static void AddTmTrialPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(
            Constants.UserPolicies.AdministratorPolicy,
            builder => builder.RequireAssertion(context =>
                HasRoleClaim(context.User, Constants.UserRoles.Administrator,
                    new List<string> ()))
        );
        
        options.AddPolicy(
            Constants.UserPolicies.PlayerPolicy,
            builder => builder.RequireAssertion(context =>
                HasRoleClaim(context.User, Constants.UserRoles.Player,
                    new List<string> { Constants.UserRoles.Administrator, }))
        );
        
        options.AddPolicy(
            Constants.UserPolicies.MapperPolicy,
            builder => builder.RequireAssertion(context =>
                HasRoleClaim(context.User, Constants.UserRoles.Mapper,
                    new List<string> { Constants.UserRoles.Administrator, }))
        );
           
        options.AddPolicy(
            Constants.UserPolicies.ConnectedPolicy,
            builder => builder.RequireAssertion(context =>
                HasRoleClaim(context.User, new List<string>
                    {
                        Constants.UserRoles.Mapper, Constants.UserRoles.Player
                    },
                    new List<string> { Constants.UserRoles.Administrator, }))
        );
        
    }
    private static bool HasRoleClaim(ClaimsPrincipal claimsPrincipal, string roleName, List<string> managedBy)
    {
        return claimsPrincipal.HasClaim(c =>
            (c.Type == ClaimTypes.Role && c.Value == roleName) ||
            (c.Type == ClaimTypes.Role && managedBy.Contains(c.Value)) ||
            (c.Type == ClaimTypes.Role && c.Value == Constants.UserRoles.Administrator));
    }
    
    private static bool HasRoleClaim(ClaimsPrincipal claimsPrincipal, List<string> roleNames, List<string> managedBy)
    {
        return roleNames.Any(roleName => HasRoleClaim(claimsPrincipal, roleName, managedBy));
    }
}
