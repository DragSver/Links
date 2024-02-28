using Links.Domain.Models;

namespace Links.UrlProcessor.Interfaces;

public interface ICacheUrlsService
{
    Task<LinkModel> GetCachedLink();
    Task ClearCache();
}
