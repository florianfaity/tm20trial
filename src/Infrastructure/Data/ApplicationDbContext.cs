using System.Reflection;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Domain.Entities;
using tm20trial.Domain.Common;
using tm20trial.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace tm20trial.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
    public new DbSet<Users> Users => Set<Users>();
    public DbSet<Records> Records => Set<Records>();
    public DbSet<Maps> Maps => Set<Maps>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.IdentityId;
                    if(entry.Entity.Created == default)
                    {
                        entry.Entity.Created = DateTimeOffset.Now;
                    }
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.IdentityId;
                    entry.Entity.LastModified = DateTimeOffset.Now;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
