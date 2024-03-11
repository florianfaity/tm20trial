using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;

namespace tm20trial.Infrastructure.Identity;

public class ProfileService : IProfileService
{
          private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;

        public ProfileService(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            ApplicationUser? user = new ApplicationUser();

            if (subject.HasClaim(x => x.Type == "name"))
            {
                var name = subject.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
                if (String.IsNullOrEmpty(name))
                {
                    throw new ArgumentException("Invalid name identifier");
                }
                user = await _userManager.FindByNameAsync(name);
            }

            if (user == null)
            {
                throw new ArgumentException("Invalid name identifier");
            }

            var claims = GetClaimsFromUser(user);

            context.IssuedClaims = claims.ToList();
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.CompletedTask;
        }

        private IEnumerable<Claim> GetClaimsFromUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(Constants.UserCustomClaims.UserId, user.UserDetails != null ? user.UserDetails.IdUser.ToString():""),
            };


       
            return claims;
        }
    }
