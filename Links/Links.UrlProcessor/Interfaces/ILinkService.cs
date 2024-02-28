using Links.Domain.Models;

namespace Links.UrlProcessor.Interfaces;

public interface ILinkService
{
    public Task UpdateUrlStatus(LinkModel linkModel);
}
