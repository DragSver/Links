using Links.Core.Models;
using Links.Core.RabbitMq;
using Links.Domain.ConfigureOptions;
using Links.Domain.Interfaces;
using Links.Domain.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Links.BL.Services;

public class LinksService : ILinksService
{
    private readonly IRabbitService _rabbitService;
    private readonly QueueOptions _queueOptions;

    public LinksService(IRabbitService rabbitService,
        IOptions<QueueOptions> queueOptions)
    {
        _rabbitService = rabbitService;
        _queueOptions = queueOptions.Value;
    }

    public void UpdateLinkStatus(LinkModel linkModel)
    {
        _rabbitService.Publish(_queueOptions.LinksQueue,
            JsonSerializer.Serialize(new LinkMessage(MessageAction.UpdateLinkStatus, linkModel)));
    }
}
