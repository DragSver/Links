using Links.Domain.Models;

namespace Links.Domain.Interfaces;

public interface ILinksService
{
    public void UpdateLinkStatus(LinkModel linkModel);
}
