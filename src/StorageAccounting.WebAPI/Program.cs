using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using StorageAccounting.WebAPI.Extensions;
using StorageAccounting.WebAPI.Security.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStorageAccountingDbContext(builder.Configuration);

builder.Services.AddStorageAccountingAutoMapper();

builder.Services.AddRepositories();
builder.Services.AddStorageAccountingServices();

builder.Services.AddAuthentication("SimpleApiKey")
    .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>(
        "SimpleApiKey",
        opts => opts.ApiKey = builder.Configuration.GetValue<string>("ApiKey")
    );

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationCustomResultMiddleware>();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo() { Title = "StorageAccounting.WebAPI", Version = "v1" });


    opt.AddSecurityDefinition("SimpleApiKey", new OpenApiSecurityScheme
    {
        Description = $"Simple ApiKey Authorization. " +
            $"Example: \"{ApiKeyAuthenticationSchemeHandler.KEY_HEADER_NAME} {{token}}\"",
        In = ParameterLocation.Header,
        Name = ApiKeyAuthenticationSchemeHandler.KEY_HEADER_NAME,
        Type = SecuritySchemeType.ApiKey
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
