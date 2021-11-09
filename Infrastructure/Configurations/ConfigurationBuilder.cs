using Microsoft.Extensions.Configuration;

namespace Infrastructure.Configurations
{
    public static class ConfigurationBuilder
    {

        private static readonly IConfigurationRoot Builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .SetBasePath("{BasePath}")
            .AddJsonFile("config.json", false)
            .Build();

        public static IConfigurationRoot Build() 
            => Builder;
    }
}