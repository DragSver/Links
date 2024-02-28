using Links.DataAccess;
using Links.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Links.BL.Workflow;

public class GetLinkRequest : IRequest<LinkStatusModel>
{
    public Guid Id { get; set; }
}

public class GetLinkRequestHandler : IRequestHandler<GetLinkRequest, LinkStatusModel>
{
    private readonly AppDbContext _appDbContext;

    public GetLinkRequestHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<LinkStatusModel> Handle(GetLinkRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var link = await _appDbContext.Links
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return new LinkStatusModel { Status = link.Status.ToString(), Url = link.Url };
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}
