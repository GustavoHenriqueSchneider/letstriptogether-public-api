using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class UnsupportedMediaTypeException : BaseException
{
    public UnsupportedMediaTypeException(InternalApiException apiException)
        : base(apiException)
    {
    }
}
