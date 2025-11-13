namespace Domain.Common.Exceptions;

public class DomainBusinessRuleException : Exception
{
    public int StatusCode { get; } = 422;
    
    public string? Title { get; }
    
    public DomainBusinessRuleException(InternalApiException internalApiException)
        : base(internalApiException.Detail, null)
    {
        Title = internalApiException.Title;
    }
}
