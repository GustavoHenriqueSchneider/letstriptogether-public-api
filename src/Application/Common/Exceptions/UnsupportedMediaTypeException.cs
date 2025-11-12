using Microsoft.AspNetCore.Http;

namespace Application.Common.Exceptions;

public class UnsupportedMediaTypeException : BaseException
{
    public UnsupportedMediaTypeException(string message, Exception? innerException = null)
        : base(message, StatusCodes.Status415UnsupportedMediaType, "Unsupported Media Type", innerException)
    {
    }
}
