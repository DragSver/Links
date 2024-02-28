using Links.Domain.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Links.UrlProcessor.Services;

public class UrlService : IUrlService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly UrlsOptions _urlsOptions;
    private readonly ILogger<UrlService> _logger;

    public UrlService(IHttpClientFactory clientFactory, 
        IOptions<UrlsOptions> urlsOptions,
        ILogger<UrlService> logger)
    {
        _clientFactory = clientFactory;
        _urlsOptions = urlsOptions.Value;
        _logger = logger;
    }

    public async Task UpdateUrlStatus(LinkModel linkModel)
    {
        var client = _clientFactory.CreateClient();

        var linkRequest = new HttpRequestMessage(
            HttpMethod.Get,
            linkModel.Url);

        try
        {
            var response = await client.SendAsync(linkRequest);
            var statusCode = (int)response.StatusCode;

            var model = new UpdateStatusModel(statusCode);

            var content = new StringContent(
                JsonConvert.SerializeObject(model),
                Encoding.UTF8,
                "application/json"
            );

            var updateUrl = $"{_urlsOptions.DefaultLinkProjectUrl}/Links/{linkModel.Id}/Status";

            await client.PutAsync(updateUrl, content);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}
