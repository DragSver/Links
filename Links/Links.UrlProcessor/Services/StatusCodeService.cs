using Links.UrlProcessor.Interfaces;

namespace Links.UrlProcessor.Services;

public class StatusCodeService : IStatusCodeService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<StatusCodeService> _logger;

    public StatusCodeService(IHttpClientFactory clientFactory, 
        ILogger<StatusCodeService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<int?> GetStatusCode(string url)
    {
        var client = _clientFactory.CreateClient();

        try
        {
            var linkRequest = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(linkRequest);
            return (int)response.StatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return null;
        }
    }
}
