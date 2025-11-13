namespace Application.Common.Interfaces.Extensions;

public interface IApplicationUserContextExtensions
{
    Guid GetId();
    string GetTokenType();
}
