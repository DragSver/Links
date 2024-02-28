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

namespace Links.Core.RabbitMq;

public class RabbitService : IRabbitService
{
    private readonly string TAG = "rabbitmq service: ";

    //public RabbitConfiguration Config = new RabbitConfiguration();
    private readonly RpcOptions _rpcOptions;

    private IConnection _connection;
    private IModel _channel;

    public RabbitService(IOptions<RpcOptions> rpcOptions)
    {
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
        Console.WriteLine($"{TAG}starting connection on {_rpcOptions.Host}:{_rpcOptions.Port} ...");

        for (int i = 1; i < _rpcOptions.RetryCount; i++)
        {
            try
            {
                Connect(queues);
                return;
            }
            catch (BrokerUnreachableException)
            {
                Console.WriteLine($"{TAG}connection failed. Trying again after {_rpcOptions.ResponseTimeout} ms...");
                Thread.Sleep(_rpcOptions.ResponseTimeout);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{TAG}unhandled exception - {e.Message}");
                break;
            }
        }
        Console.WriteLine($"{TAG}connecting to rabbitmq failed, shutting down...");
        Environment.Exit(1);
    }

    private void Connect(params string[] queues)
    {
        //_connection = new ConnectionFactory()
        //{
        //    HostName = _rpcOptions.Host,
        //    Port = _rpcOptions.Port,
        //    UserName = _rpcOptions.UserName,
        //    Password = _rpcOptions.UserPass
        //}.CreateConnection();

        //_channel = _connection.CreateModel();

        foreach (string q in queues)
        {
            Console.WriteLine($"{TAG} useing queue: {q}");
            _channel.QueueDeclare(queue: q,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }

        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        Console.WriteLine($"{TAG}connected succesfully");
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

        Console.WriteLine($"{TAG}consumer attached");
    }

    public void EndConnection()
    {
        _connection.Close();
    }
}
