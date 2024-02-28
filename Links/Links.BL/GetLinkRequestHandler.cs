using Links.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Links.BL;

public class GetLinkRequest : IRequest<string>
{
    public Guid Id { get; set; }    
}

public class GetLinkRequestHandler : IRequestHandler<GetLinkRequest, string>
{
    private readonly AppDbContext _appDbContext;

    public GetLinkRequestHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<string> Handle(GetLinkRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var link = await _appDbContext.Links
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return link?.Url;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
