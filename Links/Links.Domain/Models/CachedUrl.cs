namespace Links.Domain.Models;

public class CachedUrl
{
    public CachedUrl(string url, int? statusCode)
    {
        Url = url;
        StatusCode = statusCode;
    }
    
    public string Url { get; set; }
    public int? StatusCode { get; set; }
}
