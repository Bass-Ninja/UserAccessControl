using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserAccessControl.Application.Auth;
using UserAccessControl.Application.Resource;
using UserAccessControl.Application.User;
using UserAccessControl.Core.Database;
using UserAccessControl.Core.Entities;


namespace UserAccessControl.API.Infrastructure;

public static class StartupHelpers
{
    public static IConfigurationRoot GetConfiguration()
    {
        string? settingLocation = Environment.GetEnvironmentVariable("SETTINGS");
        if (string.IsNullOrEmpty(settingLocation))
            settingLocation = "appsettings.json";

        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(settingLocation, false)
            .AddEnvironmentVariables()
            .Build();
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DbConnection"));
        });
    }

    public static void AddAuthenticationConfig(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opts =>
            {
                opts.SaveToken = true;
                opts.RequireHttpsMetadata = false;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = configuration["JwtSettings:Audience"],
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                };
            });
    }

    public static void AddUserIdentity(this IServiceCollection service)
    {
        service.AddIdentity<UserEntity, IdentityRole<Guid>>(opts =>
            {
                opts.SignIn.RequireConfirmedAccount = false;
                opts.SignIn.RequireConfirmedEmail = false;
                opts.SignIn.RequireConfirmedPhoneNumber = false;
                opts.Password.RequiredLength = 8;
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireNonAlphanumeric = false;
                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection service)
    {
        return service
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IResourceRepository, ResourceRepository>();

    }

    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        return service

            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<IResourceService, ResourceService>();

    }
    public static IServiceCollection AddConfigs(this IServiceCollection service, IConfiguration configuration)
    {
        return service
            .Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    }
    public static async Task IdentitySeed(UserManager<UserEntity> userManager)
    {
        if (userManager == null)
            throw new ArgumentNullException(nameof(userManager));

        var admin = await userManager.FindByEmailAsync("admin@ninja.si");

        if (admin == null)
        {
            admin = new UserEntity
            {
                UserName = "admin@ninja.si",
                Email = "admin@ninja.si",
                FirstName = "Admin",
                LastName = "Admin",
            };

            var result = await userManager.CreateAsync(admin, "tempPass123!");
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create default admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

}