using AutoMapper;
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
using System.Security.Cryptography.Xml;
using System.Text;

namespace EcommerceProject.Service.WebApi
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddMvc();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<ICustomersDomain, CustomersDomain>();
            services.AddScoped<ICustomersApplication, CustomersApplication>();
            services.AddScoped<ICustomersRepository, CustomerRepository>();
            services.AddScoped<IUsersApplication, UsersApplication>();
            services.AddScoped<IUsersDomain, UsersDomain>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);          

            return services;
        }

        public static void RegisterSwagger(this IServiceCollection services)
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "Enter JWT Bearer",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                },
            };
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ecommerce API",
                    Description = "API made for a Domain Driven Design project",
                    TermsOfService = new Uri("https://ecommerce.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Juan Scabuzzo",
                        Email = "Juanscabu@gmail.com",
                        Url = new Uri("https://ecommerce.com/contact"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Free Use",
                        Url = new Uri("https://ecommerce.com/lisence")
                    }

                });
                option.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id= JwtBearerDefaults.AuthenticationScheme
                            },
                        },
                        new string[]{ }
                    }
                });
            });
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
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
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
