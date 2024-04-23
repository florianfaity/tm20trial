

using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;

namespace tm20trial.Infrastructure.Services;

public class PointsService : IPointsService
{
    private readonly IApplicationDbContext _context;
    
    public PointsService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CalculPointsByUser(int idUser, CancellationToken token)
    {
        var records = await _context.Records
            .Include(x => x.Map).Where(u => u.IdUser == idUser && u.IsValidated).ToListAsync(token);

        return records.Sum(x => x.Map?.Points ?? 0);
    }
}
