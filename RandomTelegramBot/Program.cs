using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace RandomTelegramBot
{
    public static class Program
    {
        private static TelegramBotClient _bot;

        private static async Task Main()
        {
            var startup = new Startup();
            IServiceCollection services = new ServiceCollection();
            startup.ConfigureServices(services);
            
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var configureService = serviceProvider.GetService<IConfigureClientService>();

            try
            {
                _bot = configureService.CreateBot();

                using var cts = new CancellationTokenSource();

                _bot.StartReceiving(new DefaultUpdateHandler(Handlers.HandleUpdateAsync, Handlers.HandleErrorAsync),
                    cts.Token);

                var me = await _bot.GetMeAsync(cts.Token);
                Console.WriteLine($"Start listening for @{me.Username}");
                
                Console.ReadLine();

                cts.Cancel();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}