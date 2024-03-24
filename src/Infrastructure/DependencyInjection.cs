using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tm20trial.Application.Common.Configurations;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Domain.Interfaces;
using tm20trial.Domain.Service;
using tm20trial.Infrastructure.Data;
using tm20trial.Infrastructure.Data.Interceptors;
using tm20trial.Infrastructure.Identity;
using tm20trial.Infrastructure.Services;

namespace tm20trial.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
        //    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<ApplicationDbContextInitialiser>();
        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddUserStore<UserStore>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        services.Configure<TrackmaniaConfiguration>(configuration.GetSection("Trackmania"));
        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IEncryptionService, EncryptionService>();
  //      services.AddTransient<IProfileService, ProfileService>();
    
        services.AddAuthorization(ApplicationPolicy.AddTmTrialPolicies);
        
        services.AddTransient<ITrackmaniaService, TrackmaniaService>();
        
        return services;
    }
}
