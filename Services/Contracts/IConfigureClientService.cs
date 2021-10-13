using Telegram.Bot;

namespace Services.Contracts
{
    public interface IConfigureClientService
    {
        TelegramBotClient CreateBot();
    }
}