﻿using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using tm20trial.Domain.Entities;
using tm20trial.Domain.Interfaces;

namespace tm20trial.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, List<string> roles, Users userDetail)
    {     
        if (userDetail == null)
        {
            throw new Exception("Error during user creation: userDetail cannot be null");
        }
        
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName,
            UserDetails = userDetail
        };

        var result = await _userManager.CreateAsync(user, password);

        user = await _userManager.FindByIdAsync(user.Id);
        if (user == null)
        {
            throw new Exception("Error during user creation: user null for no reason");
        }
        
        foreach (var role in roles)
        {
            if (!string.IsNullOrWhiteSpace(role))
            {
                await _userManager.AddToRoleAsync(user, role); 
            }
        }
        
        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }
    
    public async Task<IApplicationUser> FindUserByIdAsync(string id, CancellationToken token = default)
    {
        var user = await _userManager.Users.Include(x => x.UserDetails).FirstOrDefaultAsync(u => u.Id == id, token);

        return user?? new ApplicationUser();
    }
    public async Task<IApplicationUser> FindUserByIdAsync(int id, CancellationToken token = default)
    {
        var user = await _userManager.Users.Include(x => x.UserDetails).FirstOrDefaultAsync(u => u.UserDetails != null && u.UserDetails.IdUser == id, token);
         
        return user?? new ApplicationUser();
    }
    
    public async Task<bool> UserExistAsync(string username, CancellationToken token = default)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == username, cancellationToken: token);

        return user != null;
    }

}
