namespace Links.Core.RabbitMq;

public interface IRabbitService
{
    public void StartConnection(params string[] queues);
    public void Publish(string queue, string message);
    public void Consume(string queue, Action<string> onMessageRecieved);
    public void EndConnection();
}
