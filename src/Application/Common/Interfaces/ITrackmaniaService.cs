using Microsoft.AspNetCore.Mvc;

namespace tm20trial.Application.Common.Interfaces;
using System.Threading.Tasks;

public interface ITrackmaniaService
{
    Task<ContentResult> GetAutorize();
}
