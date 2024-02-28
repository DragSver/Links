using Links.Domain.Enums;

namespace Links.Domain.Entities;

public class Link
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public LinkStatus Status { get; set; }
}