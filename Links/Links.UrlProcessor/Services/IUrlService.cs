using Links.Domain.Models;

namespace Links.UrlProcessor.Services;

public interface IUrlService
{
    public Task UpdateUrlStatus(LinkModel linkModel);
}
