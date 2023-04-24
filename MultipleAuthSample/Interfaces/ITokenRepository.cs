namespace MultipleAuthSample.Interfaces;

/// <summary>
/// you need to implement your own logic
/// </summary>
public interface ITokenRepository
{
    ValueTask<CustomToken> GetAsync(string customToken);
}

public class TokenRepository : ITokenRepository
{
    public ValueTask<CustomToken> GetAsync(string customToken) => ValueTask.FromResult(new CustomToken { UserId = Guid.NewGuid().ToString() });

}

public class CustomToken
{
    public string UserId { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
