
using tm20trial.Application.Common.Models;

namespace tm20trial.Application.Openplanet.Queries;

public class NadeoMapDto
{
    public string? Author { get; set; }
    
    public string? Name { get; set; }
    
    
    public string? FileUrl { get; set; }
    
    public string? ThumbnailUrl { get; set; }
    
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<NadeoMapResponse, NadeoMapDto>();
        }
    }
}
