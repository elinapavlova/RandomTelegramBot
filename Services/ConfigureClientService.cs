using Infrastructure.Configurations;
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

        public ConfigureClientService(AppConfiguration configuration)
        {
            _token = configuration.Token;
        }

        public TelegramBotClient CreateBot()
        {
            _client = new TelegramBotClient(_token);
            return _client;
        }
    }
}