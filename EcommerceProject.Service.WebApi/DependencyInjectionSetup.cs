using EcommerceProject.Application.Interface;
using EcommerceProject.Application.Main;
using EcommerceProject.Domain.Core;
using EcommerceProject.Domain.Interface;
using EcommerceProject.Infrastructure.Data;
using EcommerceProject.Infrastructure.Interface;
using EcommerceProject.Infrastructure.Repository;
using EcommerceProject.Service.WebApi.Helpers;
using EcommerceProject.Transversal.Common;
using EcommerceProject.Transversal.Logging;
using EcommerceProject.Transversal.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EcommerceProject.Service.WebApi
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddAutoMapper(x => x.AddProfile(new MappingsProfile()));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddMvc();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<ICustomersDomain, CustomersDomain>();
            services.AddScoped<ICustomersApplication, CustomersApplication>();
            services.AddScoped<ICustomersRepository, CustomerRepository>();
            services.AddScoped<IUsersApplication, UsersApplication>();
            services.AddScoped<IUsersDomain, UsersDomain>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
                {
                    Description = "Authorization by key",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Authorization"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Authorization"
                        },
                        Scheme = "Authorization",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new string[]{}
                }
            });
                    });

            return services;
        }

        public static void RegisterAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var Issuer = appSettings.Issuer;
            var Audience = appSettings.Audience;

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userid = int.Parse(context.Principal.Identity.Name);
                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = false;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = Issuer,
                        ValidateAudience = true,
                        ValidAudience = Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }
    }
}
