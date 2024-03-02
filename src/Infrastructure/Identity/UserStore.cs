using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using tm20trial.Domain.Entities;
using tm20trial.Infrastructure.Data;

namespace tm20trial.Infrastructure.Identity
{
    public class UserStore : UserStore<ApplicationUser, IdentityRole, ApplicationDbContext>
    {
        public UserStore(ApplicationDbContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {
        }

        public override async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await base.FindByIdAsync(userId, cancellationToken);

            if (user != null)
            {
                user.UserDetails = await GetUserDetailsAsync(user.Id);
            }

            return user;
        }

        public override async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await base.FindByNameAsync(normalizedUserName, cancellationToken);

            if (user != null)
            {
                user.UserDetails = await GetUserDetailsAsync(user.Id);
            }

            return user;
        }

        protected override async Task<ApplicationUser?> FindUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await base.FindUserAsync(userId, cancellationToken);

            if (user != null)
            {
                user.UserDetails = await GetUserDetailsAsync(user.Id);
            }

            return user;
        }

        public override async Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await base.FindByLoginAsync(loginProvider, providerKey, cancellationToken);

            if (user != null)
            {
                user.UserDetails = await GetUserDetailsAsync(user.Id);
            }

            return user;
        }

        public override async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await base.FindByEmailAsync(normalizedEmail, cancellationToken);

            if (user != null)
            {
                user.UserDetails = await GetUserDetailsAsync(user.Id);
            }

            return user;
        }

        private async Task<Users> GetUserDetailsAsync(string userId)
        {
            var user = await Users
                .Include(x => x.UserDetails)
                .FirstAsync(x => x.Id == userId);

            return user.UserDetails;
        }
    }
}
