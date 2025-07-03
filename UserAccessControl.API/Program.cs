using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserAccessControl.API.Infrastructure;
using UserAccessControl.Core.Database;
using UserAccessControl.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

var configuration = StartupHelpers.GetConfiguration();

builder.Services.AddDbContext(configuration);
builder.Services.AddUserIdentity();
builder.Services.AddAuthenticationConfig(builder.Configuration);

builder.Services.AddConfigs(configuration);
builder.Services.AddServices();
builder.Services.AddRepositories();

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "UserAccessControl API",
        Version = "v1",
    });
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "API Key Authentication (Bearer)",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "ApiKey"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception migrateEx)
    {
        throw new Exception("Migration failed: " + migrateEx.Message, migrateEx);
    }

    try
    {
        UserManager<UserEntity>? userManager = scope.ServiceProvider.GetService<UserManager<UserEntity>>();
        if (userManager != null) await StartupHelpers.IdentitySeed(userManager);
    }
    catch(Exception ex)
    {
        throw new Exception(ex.Message);
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();