using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class ConflictException : BaseException
{
    public ConflictException(InternalApiException apiException)
        : base(apiException)
    {
    }
}
