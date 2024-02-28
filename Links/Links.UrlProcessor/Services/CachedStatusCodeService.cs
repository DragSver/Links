using Links.Core.Caching;
using Links.Domain.Models;
using Links.UrlProcessor.Interfaces;
using Links.UrlProcessor.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Links.UrlProcessor.Services;

public class CachedStatusCodeService : IStatusCodeService
{
    private const int CacheTimeToLive = 120;

    private readonly StatusCodeService _statusCodeService;
    private readonly ICacheProvider _cacheProvider;
    private readonly CacheOptions _cacheOptions;

    private static readonly SemaphoreSlim GetUrlsSemaphore = new(1, 1);

    public CachedStatusCodeService(StatusCodeService statusCodeService, 
        ICacheProvider cacheProvider, 
        IOptions<CacheOptions> cacheOptions)
    {
        _statusCodeService = statusCodeService;
        _cacheProvider = cacheProvider;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<int?> GetStatusCode(string url)
    {
        var cachedLink = await GetCachedResponse(url, GetUrlsSemaphore, () => _statusCodeService.GetStatusCode(url));
        return cachedLink?.StatusCode;
    }

    private async Task<CachedUrl> GetCachedResponse(string url, SemaphoreSlim semaphore, Func<Task<int?>> func)
    {
        var cachedUrl = await _cacheProvider.GetFromCache<CachedUrl>(url);

        if (cachedUrl != null) return cachedUrl;

        try
        {
            await semaphore.WaitAsync();

            // Recheck to make sure it didn't populate before entering semaphore
            cachedUrl = await _cacheProvider.GetFromCache<CachedUrl>(url);
            if (cachedUrl != null) return cachedUrl;

            var statusCode = await func();

            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(CacheTimeToLive));

            cachedUrl = new CachedUrl(url, statusCode);

            await _cacheProvider.SetCache(url, cachedUrl, cacheEntryOptions);
        }
        finally
        {
            semaphore.Release();
        }

        return cachedUrl;
    }
}
