using Infrastructure;
using Infrastructure.Configurations;
using Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Contracts;
using ConfigurationBuilder = Infrastructure.Configurations.ConfigurationBuilder;

namespace RandomTelegramBot
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup()
        {
            Configuration = ConfigurationBuilder.Build();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppOptions>(Configuration.GetSection(AppOptions.App));
            var appOptions = Configuration.GetSection(AppOptions.App).Get<AppOptions>();
            var appConfiguration = new AppConfiguration(appOptions.Token);
            
            services.Configure<CommandsOptions>(Configuration.GetSection(CommandsOptions.Commands));
            var commandsOptions = Configuration.GetSection(CommandsOptions.Commands).Get<CommandsOptions>();
            var commandsConfiguration = new CommandsConfiguration(commandsOptions.Command);
            
            services.AddSingleton(appConfiguration);
            services.AddSingleton(commandsConfiguration);

            services.AddScoped<IConfigureClientService, ConfigureClientService>();

            // Инициализация списка команд
            var handlers = new Handlers(commandsConfiguration);
        }
    }
}