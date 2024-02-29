using Microsoft.AspNetCore.Identity;
using tm20trial.Domain.Entities;
using tm20trial.Domain.Interfaces;

namespace tm20trial.Infrastructure.Identity
{
#pragma warning disable CS8618 
    public class ApplicationUser : IdentityUser, IApplicationUser
    {  
        public Users UserDetails { get; set; }
    }
#pragma warning restore CS8618
}
