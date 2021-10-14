using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog.Web;
using Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace RandomTelegramBot
{
    public class Program
    {
        private static TelegramBotClient _bot;

        private static async Task Main()
        {
            var pathToLogFile = "path";
            var logger = NLogBuilder.ConfigureNLog(pathToLogFile).GetCurrentClassLogger();
            try
            {
                logger.Debug("init main");
                
                var startup = new Startup();
                IServiceCollection services = new ServiceCollection();
                startup.ConfigureServices(services);
                
                IServiceProvider serviceProvider = services.BuildServiceProvider();
                var configureService = serviceProvider.GetService<IConfigureClientService>();
                var handlersService = serviceProvider.GetService<IHandlersService>();
            
                _bot = configureService.CreateBot();

                using var cts = new CancellationTokenSource();

                _bot.StartReceiving(new DefaultUpdateHandler(handlersService.HandleUpdateAsync, handlersService.HandleErrorAsync),
                    cts.Token);

                var me = await _bot.GetMeAsync(cts.Token);
                Console.WriteLine($"Start listening for @{me.Username}");
                
                Console.ReadLine();

                cts.Cancel();
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}