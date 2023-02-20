using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace StorageAccounting.WebAPI.Security.Authorization
{
    internal class ApiKeyAuthenticationSchemeHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        internal const string API_KEY_IDENTITY = "apiKey";
        internal const string KEY_HEADER_NAME = "apiKey";

        public ApiKeyAuthenticationSchemeHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var receivedKey = GetReceivedKey();

            var test = Request.Headers.Keys.ToArray();

            if (IsNotEqual(Options.ApiKey, receivedKey))
            {
                return Task.FromResult(AuthenticateResult.Fail(AuthenticateFailMessage));
            }

            return Task.FromResult(AuthenticateResult.Success(GetAuthenticationTicket()));
        }
        private string? GetReceivedKey()
        {
            string? header = Request.Headers[KEY_HEADER_NAME];

            if (header is not null &&
                header.StartsWith(KEY_HEADER_NAME, StringComparison.OrdinalIgnoreCase))
            {
                return header.Substring(KEY_HEADER_NAME.Length).Trim();
            }

            return header;
        }

        private static bool IsNotEqual(string apiKey, string? receivedKey)
        {
            if (string.IsNullOrEmpty(receivedKey))
                return true;

            var apiKeySpan = MemoryMarshal.Cast<char, byte>(apiKey.AsSpan());

            var receivedKeySpan = MemoryMarshal.Cast<char, byte>(receivedKey.AsSpan());

            return !CryptographicOperations.FixedTimeEquals(apiKeySpan, receivedKeySpan);
        }

        private AuthenticationTicket GetAuthenticationTicket()
        {
            var claimIdentity = new ClaimsIdentity(API_KEY_IDENTITY);

            var claimPrincipal = new ClaimsPrincipal(claimIdentity);

            return new AuthenticationTicket(claimPrincipal, Scheme.Name);
        }

        private static string AuthenticateFailMessage = $"Authentication fail: Api key not the same. " +
            $"Example: \"{ApiKeyAuthenticationSchemeHandler.KEY_HEADER_NAME} {{token}}\"";
    }
}
