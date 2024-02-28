namespace Links.UrlProcessor.Interfaces;

public interface IStatusCodeService
{
    Task<int?> GetStatusCode(string url);
}
