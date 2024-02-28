namespace Links.Core.Models;

public abstract class RabbitMessage<T>
{
    public RabbitMessage(MessageAction action, T model)
    {
        Action = action;
        Model = model;
    }

    public MessageAction Action { get; set; }
    public T Model { get; set; }
}
