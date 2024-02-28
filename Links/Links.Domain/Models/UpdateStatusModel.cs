namespace Links.Domain.Models;

public class UpdateStatusModel
{
    public UpdateStatusModel(int statusCode) => StatusCode = statusCode;

    public int StatusCode { get; set; }
}
