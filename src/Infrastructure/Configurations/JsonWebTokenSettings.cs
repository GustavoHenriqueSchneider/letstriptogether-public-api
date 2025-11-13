namespace Infrastructure.Configurations;

public class JsonWebTokenSettings
{
    public string Issuer { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
}
