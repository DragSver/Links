using Links.DataAccess;
using Links.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Links.BL.Workflow;

public class UpdateLinkStatusRequest : IRequest<bool>
{
    public Guid Id { get; set; }    
    public int? StatusCode{ get; set; }
}

public class UpdateLinkStatusRequestHandler : IRequestHandler<UpdateLinkStatusRequest, bool>
{
    private readonly AppDbContext _appDbContext;

    public UpdateLinkStatusRequestHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<bool> Handle(UpdateLinkStatusRequest request, CancellationToken cancellationToken)
    {
        var link = await _appDbContext.Links
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (link == null) return false;

        link.Status = GetLinkStatus(request.StatusCode);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    private LinkStatus GetLinkStatus(int? statusCode) => statusCode >= 200 && statusCode <= 299
        ? LinkStatus.Valid : LinkStatus.Invalid;
}
