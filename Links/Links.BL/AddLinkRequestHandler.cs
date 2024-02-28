using Links.DataAccess;
using Links.Domain.Entities;
using MediatR;

namespace Links.BL;

public class AddLinkRequest : IRequest<Guid?>
{
    public string Url { get; set; }
}

public class AddLinkRequestHandler : IRequestHandler<AddLinkRequest, Guid?>
{
    private readonly AppDbContext _appDbContext;

    public AddLinkRequestHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Guid?> Handle(AddLinkRequest request, CancellationToken cancellationToken)
    {
        var entity = new Link { Url= request.Url };

        try
        {
            await _appDbContext.AddAsync(entity, cancellationToken);
            await _appDbContext.SaveChangesAsync();

            return entity.Id;
        }
        catch(Exception ex)
        {
            return null;
        }
    }
}