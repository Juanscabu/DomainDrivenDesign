using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EcommerceProject.Infrastructure.Options
{
    public class RabbitMqOptionsSetup : IConfigureOptions<RabbitMqOptions>
    {
        private const string ConfigurationSectionName = "RabbitMqOptions";
        private readonly IConfiguration _configuration;

        public RabbitMqOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(RabbitMqOptions options)
        {
            //Binding appsettings sections with class RabbitMqOptions
            _configuration.GetSection(ConfigurationSectionName).Bind(options);
        }
    }
}
