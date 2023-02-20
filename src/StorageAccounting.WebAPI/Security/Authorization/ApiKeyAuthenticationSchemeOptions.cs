using Microsoft.AspNetCore.Authentication;

namespace StorageAccounting.WebAPI.Security.Authorization
{
    internal class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string ApiKey { get; set; } = string.Empty;
    }
}
