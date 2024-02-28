using Links.Domain.Models;

namespace Links.Core.Models;

public class LinkMessage : RabbitMessage<LinkModel>
{
    public LinkMessage(MessageAction action, LinkModel model) : base(action, model) { }
}
