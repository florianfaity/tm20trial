using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace tm20trial.Infrastructure.Data.Seeds;
internal class ApplicationRolesSeed : IApplicationDbSeed
{
    private readonly RoleManager<IdentityRole> _roleManager;
    public ApplicationRolesSeed(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public Task PreMigrationSeedAsync()
    {
        return Task.CompletedTask;
    }

    public async Task PostMigrationSeedAsync()
    {
        if (!await _roleManager.RoleExistsAsync("Administrator"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
        }
        if (!await _roleManager.RoleExistsAsync("Mapper"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Mapper"));
        }
    }
}
