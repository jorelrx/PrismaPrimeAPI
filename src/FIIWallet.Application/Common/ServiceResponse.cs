namespace FIIWallet.Application.Common;

public class ServiceResponse<TData>(bool success, string? message, TData? data)
{
    public bool Success { get; set; } = success;
    public string? Message { get; set; } = message;
    public TData? Data { get; set; } = data;

    public static ServiceResponse<TData> CreateSuccess(TData data)
    {
        return new ServiceResponse<TData>(true, null, data);
    }

    public static ServiceResponse<TData> CreateFail(string message)
    {
        return new ServiceResponse<TData>(false, message, default);
    }
}
