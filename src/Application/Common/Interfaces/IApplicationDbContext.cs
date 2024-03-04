using tm20trial.Domain.Entities;

namespace tm20trial.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Domain.Entities.Users> Users { get; }

    DbSet<Records> Records { get; }

    DbSet<Maps> Maps { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
