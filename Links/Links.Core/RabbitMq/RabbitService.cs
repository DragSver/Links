using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Links.Domain.ConfigureOptions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Links.Core.RabbitMq;

public class RabbitService : IRabbitService
{
    private readonly RpcOptions _rpcOptions;
    private readonly ILogger<RabbitService> _logger;

    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitService(IOptions<RpcOptions> rpcOptions, ILogger<RabbitService> logger)
    {
        _logger = logger;
        _rpcOptions = rpcOptions.Value;
        _connection = new ConnectionFactory()
        {
            HostName = _rpcOptions.Host,
            Port = _rpcOptions.Port,
            UserName = _rpcOptions.UserName,
            Password = _rpcOptions.UserPass
        }.CreateConnection();

        _channel = _connection.CreateModel();
    }

    public void StartConnection(params string[] queues)
    {
        _logger.LogInformation($"Starting connection on {_rpcOptions.Host}:{_rpcOptions.Port} ...");

        for (int i = 1; i < _rpcOptions.RetryCount; i++)
        {
            try
            {
                Connect(queues);
                return;
            }
            catch (BrokerUnreachableException ex)
            {
                _logger.LogError($"Connection failed. Trying again after {_rpcOptions.ResponseTimeout} ms...", ex);
                Thread.Sleep(_rpcOptions.ResponseTimeout);
            }
            catch (Exception e)
            {
                _logger.LogError($"Unhandled exception - {e.Message}", e);
                break;
            }
        }

        _logger.LogError($"Connecting to rabbitmq failed, shutting down...");
        Environment.Exit(1);
    }

    private void Connect(params string[] queues)
    {
        foreach (string q in queues)
        {
            _logger.LogInformation($"Using queue: {q}");
            _channel.QueueDeclare(queue: q,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        _logger.LogInformation($"Connected succesfully");
    }

    public void Publish(string queue, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: "",
                              routingKey: queue,
                              basicProperties: null,
                              body: body);
    }

    public void Consume(string queue, Action<string> onMessageRecieved)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            onMessageRecieved(message);
        };
        _channel.BasicConsume(queue: queue,
                             autoAck: true,
                             consumer: consumer);

        _logger.LogInformation($"Consumer attached");
    }

    public void EndConnection()
    {
        _connection.Close();
    }
}
