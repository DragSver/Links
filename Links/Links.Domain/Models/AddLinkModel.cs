using System.ComponentModel.DataAnnotations;

namespace Links.Domain.Models;

public class AddLinkModel
{
    [Required(AllowEmptyStrings = false)]
    public string Url { get; set; }
}
