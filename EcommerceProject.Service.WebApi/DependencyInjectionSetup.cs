using AutoMapper;
using EcommerceProject.Application.Feature.Categories;
using EcommerceProject.Application.Feature.Common.Mappings;
using EcommerceProject.Application.Feature.Customers;
using EcommerceProject.Application.Feature.Discounts;
using EcommerceProject.Application.Feature.Users;
using EcommerceProject.Application.Interface.Features;
using EcommerceProject.Application.Interface.Persistence;
using EcommerceProject.Application.Validator;
using EcommerceProject.Persistence.Contexts;
using EcommerceProject.Persistence.Interceptors;
using EcommerceProject.Persistence.Repositories;
using EcommerceProject.Service.WebApi.HealthCheck;
using EcommerceProject.Service.WebApi.Helpers;
using EcommerceProject.Service.WebApi.Swagger;
using EcommerceProject.Transversal.Common;
using EcommerceProject.Transversal.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
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

            return services;
        }

        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DapperContext>();
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("NorthwindConnection"),
            builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();
            services.AddScoped<IDiscountRepository, DiscountsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomersApplication, CustomersApplication>();
            services.AddScoped<IUsersApplication, UsersApplication>();
            services.AddScoped<ICategoriesApplication, CategoriesApplication>();
            services.AddScoped<IDiscountsApplication, DiscountsApplication>();
            
            services.AddTransient<UsersDtoValidator>();
            services.AddTransient<DiscountDtoValidator>();

            return services;
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

        public static IServiceCollection AddFeature(this IServiceCollection services, IConfiguration configuration)
        {
            string myPolicy = "policyApiEcommerce";
            
            var appSettingsSection = configuration.GetSection("Config");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddCors(options => options.AddPolicy(myPolicy, builder => builder.WithOrigins(appSettings.OriginCors)
                                                                                   .AllowAnyHeader()
                                                                                   .AllowAnyMethod()));
            services.AddMvc();
            services.AddControllers().AddJsonOptions(opts =>
            {
                var EnumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(EnumConverter);
            });

            return services;
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
