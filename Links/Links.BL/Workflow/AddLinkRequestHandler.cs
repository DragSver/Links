using Links.DataAccess;
using Links.Domain.Entities;
using Links.Domain.Interfaces;
using Links.Domain.Models;
using MediatR;

namespace Links.BL.Workflow;

public class AddLinkRequest : IRequest<Guid?>
{
    public string Url { get; set; }
}

public class AddLinkRequestHandler : IRequestHandler<AddLinkRequest, Guid?>
{
    private readonly AppDbContext _appDbContext;
    private readonly ILinksService _linksService;

    public AddLinkRequestHandler(AppDbContext appDbContext, ILinksService linksService)
    {
        _appDbContext = appDbContext;
        _linksService = linksService;
    }

    public async Task<Guid?> Handle(AddLinkRequest request, CancellationToken cancellationToken)
    {
        var entity = new Link { Url = request.Url };

        try
        {
            await _appDbContext.AddAsync(entity, cancellationToken);
            await _appDbContext.SaveChangesAsync();

            await _linksService.UpdateLinkStatus(new LinkModel { Id = entity.Id, Url = entity.Url });

            return entity.Id;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
}