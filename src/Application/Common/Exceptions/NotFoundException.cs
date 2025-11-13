using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(InternalApiException apiException)
        : base(apiException)
    {
    }
}
