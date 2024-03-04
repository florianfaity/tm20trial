using Microsoft.EntityFrameworkCore;
using tm20trial.Application.Common.Interfaces;
using tm20trial.Application.Common.Models;
using tm20trial.Application.Common.Security;

namespace tm20trial.Application.Users.Queries;

[Authorize]
public record GetUserQuery : IRequest<UserDto>
{
    public int IdUser {get;set;}
}


public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;


    public GetUserQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
                .Where(x => x.IdUser == request.IdUser)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("GetUser","id");
        }

        return user;
    }
}
