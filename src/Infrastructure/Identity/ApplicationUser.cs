using Microsoft.AspNetCore.Identity;
using tm20trial.Domain.Entities;
using tm20trial.Domain.Interfaces;

namespace tm20trial.Infrastructure.Identity
{

    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public Users UserDetails { get; set; }
    }
}