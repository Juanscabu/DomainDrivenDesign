using AutoMapper;
using EcommerceProject.Application.Interface.Features;
using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Application.Validator;
using EcommerceProject.Feature.Main;
using EcommerceProject.Persistence.Data;
using EcommerceProject.Persistence.Repository;
using EcommerceProject.Service.WebApi.HealthCheck;
using EcommerceProject.Service.WebApi.Helpers;
using EcommerceProject.Service.WebApi.Swagger;
using EcommerceProject.Transversal.Common;
using EcommerceProject.Transversal.Logging;
using EcommerceProject.Transversal.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using WatchDog;

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
            services.AddSingleton<DapperContext>();
            services.AddScoped<ICustomersApplication, CustomersApplication>();
            services.AddScoped<ICustomersRepository, CustomerRepository>();
            services.AddScoped<IUsersApplication, UsersApplication>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICategoriesApplication, CategoriesApplication>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
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

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("Config");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            
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

        public static void AddMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingsProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddFeature(this IServiceCollection services, IConfiguration configuration)
        {
            string myPolicy = "policyApiEcommerce";
            var appSettingsSection = configuration.GetSection("Config");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddCors(options => options.AddPolicy(myPolicy, builder => builder.WithOrigins(appSettings.OriginCors)
                                                                                   .AllowAnyHeader()
                                                                                   .AllowAnyMethod()));
        }

        public static void AddValidator(this IServiceCollection services)
        {
           services.AddTransient<UsersDtoValidator>();
        }

        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                //Query String
                //o.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                // Header
                //o.ApiVersionReader = new HeaderApiVersionReader("x-version");
                // URL
                o.ApiVersionReader = new UrlSegmentApiVersionReader();
            }); 

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("NorthwindConnection"), tags: new[] { "database" })
                .AddRedis(configuration.GetConnectionString("RedisConnection"), tags: new[] { "Cache" })
                .AddCheck<HealthCheckCustom>("healthCheckCustom", tags: new[] { "custom" });
            services.AddHealthChecksUI().AddInMemoryStorage();
            
            return services;
        }

        public static IServiceCollection AddWatchDog(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWatchDogServices(opt =>
            {
                opt.SetExternalDbConnString = configuration.GetConnectionString("NorthwindConnection");
                opt.DbDriverOption = WatchDog.src.Enums.WatchDogDbDriverEnum.MSSQL;
                opt.IsAutoClear = true;
                opt.ClearTimeSchedule = WatchDog.src.Enums.WatchDogAutoClearScheduleEnum.Monthly;
            });

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
            });

            return services;
        }

        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            var fixedWindowPolicy = "fixedWindow";
            services.AddRateLimiter(configureOptions =>
            {
            configureOptions.AddFixedWindowLimiter(policyName: fixedWindowPolicy, fixedWindow =>
            {
                fixedWindow.PermitLimit = int.Parse(configuration["RateLimiting:PermitLimit"]);
                fixedWindow.Window = TimeSpan.FromSeconds(int.Parse(configuration["RateLimiting:Window"]));
                fixedWindow.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
                fixedWindow.QueueLimit = int.Parse(configuration["RateLimiting:QueueLimit"]);
            });
            configureOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            });

            return services;
        }

    }
}
