using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException(InternalApiException apiException)
        : base(apiException)
    {
    }
}
