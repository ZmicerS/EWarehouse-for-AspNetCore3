using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using EWarehouse.Services;
using EWarehouse.Services.Contracts;
using EWarehouse.Web.Services.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using EWarehouse.Repository;
using EWarehouse.Repository.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using EWarehouse.Web.Services.Authorization;

namespace EWarehouse.Web.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            string connectionString = configuration["ConnectionStrings:ConnectionMSSql"];
            services.AddDbContext<ApplicationContext>(options => options.UseLoggerFactory(loggerFactory).EnableSensitiveDataLogging(true).UseSqlServer(connectionString));
            services.AddScoped<IUnitOfWork>(ctx => new UnitOfWork(ctx.GetRequiredService<ApplicationContext>(), loggerFactory));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new UserMappingProfile());
                mc.AddProfile(new StoreMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IStoreService, StoreService>();

            services.AddSingleton<IAuthorizationHandler, PermissionToActionHandler>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.RequireHttpsMetadata = false;
                      options.TokenValidationParameters = new TokenValidationParameters
                      {
                          ValidateIssuer = true,
                          ValidIssuer = configuration["Jwt:Issuer"],
                          ValidateAudience = true,
                          ValidAudience = configuration["Jwt:Issuer"],
                          ValidateLifetime = true,
                          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                          ValidateIssuerSigningKey = true,
                      };
                  });
            return services;
        }
    }
}
