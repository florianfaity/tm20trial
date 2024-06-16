using tm20trial.Application.Maps.Queries;
using tm20trial.Application.Users.Queries;

namespace tm20trial.Application.Search.Queries;

public class ListSearchDto
{
    public ListSearchDto()
    {
        Users = Array.Empty<UserDto>();
        Maps = Array.Empty<MapDto>();
    }

    public ICollection<UserDto> Users { get; set; }
    public ICollection<MapDto> Maps { get; set; }
}
