namespace MultipleAuthSample.Interfaces;

/// <summary>
/// you need to implement your own logic
/// </summary>
public interface ITokenRepository
{
    Task<CustomToken> GetAsync(string customToken);
}

public class CustomToken
{
    public string UserId { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
