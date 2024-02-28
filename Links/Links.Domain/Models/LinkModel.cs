using Links.Domain.Enums;

namespace Links.Domain.Models;

public class LinkModel
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public LinkStatusModel Status { get; set; }
}
