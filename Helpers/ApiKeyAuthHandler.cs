using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ApiKeyAuth
{
    public class ApiKeyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string ApiKeyAuthHeader = "X-Api-Key";
        private readonly IConfiguration _config;

        public ApiKeyAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IConfiguration config
        )
            : base(options, logger, encoder)
        {
            _config = config;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyAuthHeader, out StringValues key))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            string? validKey = _config["Auth:ApiKey"];
            if (key != validKey)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid API key"));
            }

            Claim[] claims = new[] { new Claim(ClaimTypes.Role, "Service") };
            ClaimsIdentity identity = new(claims, Scheme.Name);
            AuthenticationTicket ticket = new(new ClaimsPrincipal(identity), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
