
namespace tm20trial.Application.Common.Interfaces;

public interface IPointsService
{
    Task<int> CalculPointsByUser(int idUser, CancellationToken token);
}
