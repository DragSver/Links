using Links.Domain.Models;
using Links.UrlProcessor.Interfaces;
using Links.UrlProcessor.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Links.UrlProcessor.Services;

public class LinkService : ILinkService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly UrlsOptions _urlsOptions;
    private readonly IStatusCodeService _statusCodeService;
    private readonly ILogger<LinkService> _logger;

    public LinkService(IHttpClientFactory clientFactory,        
        IStatusCodeService statusCodeService,
        ILogger<LinkService> logger,
        IOptions<UrlsOptions> urlsOptions)
    {
        _clientFactory = clientFactory;
        _statusCodeService = statusCodeService;
        _logger = logger;
        _urlsOptions = urlsOptions.Value; 
    }

    public async Task UpdateUrlStatus(LinkModel linkModel)
    {
        var client = _clientFactory.CreateClient();

        try
        {
            var statusCode = await _statusCodeService.GetStatusCode(linkModel.Url);

            if (statusCode == null) return;

            var model = new UpdateStatusModel(statusCode.Value);

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
