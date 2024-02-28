namespace Links.Domain.ConfigureOptions;

public class RpcOptions
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string UserPass { get; set; }
    public int ResponseTimeout { get; set; }
    public int RetryCount { get; set; }
}
