using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(InternalApiException apiException)
        : base(apiException)
    {
    }
}
