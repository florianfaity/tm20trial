//using Microsoft.AspNetCore.Mvc;
using tm20trial.Application.Common.Models;

namespace tm20trial.Application.Common.Interfaces;
using System.Threading.Tasks;

public interface ITrackmaniaService
{
    Task<string> GetToken();
    Task<NadeoMapResponse> GetMapById(string MapId);
}
