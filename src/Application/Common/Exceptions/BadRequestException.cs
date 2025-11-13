using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(InternalApiException apiException)
        : base(apiException)
    {
    }
}
