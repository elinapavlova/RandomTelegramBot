using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Services.Contracts;
using Telegram.Bot;

namespace Services
{
    /// <summary>
    /// Сервис для создания клиента
    /// </summary>
    public class ConfigureClientService : IConfigureClientService
    {
        private readonly string _token;
        private static TelegramBotClient _client;

        public ConfigureClientService(IOptions<AppOptions> options)
        {
            _token = options.Value.Token;
        }

        public TelegramBotClient CreateBot()
        {
            _client = new TelegramBotClient(_token);
            return _client;
        }
    }
}