using Microsoft.OpenApi.Models;
using StorageAccounting.WebAPI.Security.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace StorageAccounting.WebAPI.Extensions
{
    internal static class SwaggerGenOptionsExtensions
    {
        public static void AddSimpleApiKeySecurityDefinition(this SwaggerGenOptions options) =>
            options.AddSecurityDefinition("SimpleApiKey", new OpenApiSecurityScheme
            {
                Description = $"Simple ApiKey Authorization. " +
                    $"Example: \"{ApiKeyAuthenticationSchemeHandler.KEY_HEADER_NAME} {{token}}\"",
                In = ParameterLocation.Header,
                Name = ApiKeyAuthenticationSchemeHandler.KEY_HEADER_NAME,
                Type = SecuritySchemeType.ApiKey
            });

        public static void AddSimpleApiKeySecurityRequirement(this SwaggerGenOptions options) =>
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
              {
                new OpenApiSecurityScheme
                {
                  Reference = new OpenApiReference
                  {
                      Type = ReferenceType.SecurityScheme,
                      Id = "SimpleApiKey"
                  }
                },
                new List<string>()
              }
            });
    }
}
