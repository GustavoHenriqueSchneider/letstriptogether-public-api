using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class BaseException : Exception
{
    public int StatusCode { get; }
    
    public string? Title { get; }
    
    protected BaseException(
        string message,
        int statusCode,
        string? title = null,
        Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        Title = title;
    }
    
    protected BaseException(InternalApiException apiException)
        : base(apiException.Detail, null)
    {
        StatusCode = apiException.Status;
        Title = apiException.Title;
    }
}
