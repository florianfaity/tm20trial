//using Microsoft.AspNetCore.Mvc;
using tm20trial.Application.Common.Models;

namespace tm20trial.Application.Common.Interfaces;
using System.Threading.Tasks;

public interface ITrackmaniaService
{
    Task<NadeoMapResponse> GetMapById(string MapId);
    
    Task<NadeoRecordResponse> GetUserRecordByMapId(string MapId, string userId);

}
