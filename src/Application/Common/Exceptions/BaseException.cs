namespace Application.Common.Exceptions;

public class BaseException : Exception
{
    public int StatusCode { get; }
    
    public string? Title { get; }

    public BaseException() { }
    
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
}
