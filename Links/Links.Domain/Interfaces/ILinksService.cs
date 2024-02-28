using Links.Domain.Models;

namespace Links.Domain.Interfaces;

public interface ILinksService
{
    public Task UpdateLinkStatus(LinkModel linkModel);
}
