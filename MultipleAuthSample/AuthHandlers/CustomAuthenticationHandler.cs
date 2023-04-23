using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using MultipleAuthSample.Interfaces;
using System.Data;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MultipleAuthSample.AuthHandlers
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthSchemeOptions>
    {
        private readonly ITokenRepository _tokenRepository;
        public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenRepository tokenRepository) : base(options, logger, encoder, clock)
        {
            _tokenRepository = tokenRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var customToken))
                return AuthenticateResult.Fail("No AccessToken");

            var dbToken = await _tokenRepository.GetAsync(customToken);

            if (dbToken is null)
                return AuthenticateResult.Fail("invalid token");

            var claims = new ClaimsIdentity(nameof(CustomAuthenticationHandler));
            claims.AddClaim(new Claim("userId", dbToken.UserId));
            // you can more claim based on authorization for example add Roles or Policies

            var ticket = new AuthenticationTicket(
                       new ClaimsPrincipal(claims), Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }

    public class CustomAuthSchemeOptions
       : AuthenticationSchemeOptions
    {

    }
}
