namespace OnlineShoppingApp.Business.Types;

public class ServiceMessage
{
    public bool IsSucceed { get; set; }
    public string Message { get; set; }
}

public class ServiceMessage<T>
{
    public bool IsSucceed { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
}