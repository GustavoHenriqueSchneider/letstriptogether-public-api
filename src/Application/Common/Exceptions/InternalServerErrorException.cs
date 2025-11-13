using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class InternalServerErrorException : BaseException
{
    public InternalServerErrorException(InternalApiException apiException)
        : base(apiException)
    {
    }
}
