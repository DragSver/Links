using Links.Core.Models;
using Links.Core.RabbitMq;
using Links.UrlProcessor.Interfaces;
using System.Text.Json;

namespace Links.UrlProcessor
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRabbitService _rabbitMq;
        private readonly ILinkService _linkService;

        public Worker(ILogger<Worker> logger, IRabbitService rabbitService, ILinkService urlService)
        {
            _logger = logger;
            _rabbitMq = rabbitService;
            _linkService = urlService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _rabbitMq.StartConnection("links");
            _rabbitMq.Consume("links", ProcessItemMessage);

            _logger.LogInformation("Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(2000, stoppingToken);
            }
        }

        private void ProcessItemMessage(string json)
        {
            _logger.LogInformation("From RabiitMQ (queue = 'links') recieved: " + json);

            var message = JsonSerializer.Deserialize<LinkMessage>(json);

            switch (message.Action)
            {
                case MessageAction.UpdateLinkStatus: 
                    _linkService.UpdateUrlStatus(message.Model);
                    break;

                default: throw new NotImplementedException();
            }
        }
    }
}