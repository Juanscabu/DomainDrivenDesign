using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EcommerceProject.Service.WebApi.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider"> The <see cref= "IApiVersionDescriptionProvider"> provider</see> used to generated swagger documents /param>
    
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
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

            };

            if (description.IsDeprecated)
            {
                info.Description += "this API version is deprecated";
            }
            return info;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // add a Swagger document for each discovered API version
            //note: you might choose to skip or document deprecated API versions differently

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }
    }

}
