using Links.Core.Models;
using Links.Core.RabbitMq;
using Links.Domain.Interfaces;
using Links.Domain.Models;
using System.Text.Json;

namespace Links.BL.Services;

public class LinksService : ILinksService
{
    private readonly IRabbitService _rabbitService;

    public LinksService(IRabbitService rabbitService)
    {
        _rabbitService = rabbitService;
    }

    public Task UpdateLinkStatus(LinkModel linkModel)
    {
        _rabbitService.Publish("links", 
            JsonSerializer.Serialize(new LinkMessage(MessageAction.UpdateLinkStatus, linkModel)));

        return Task.CompletedTask;
    }
}
