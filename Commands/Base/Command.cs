using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commands.Base
{
    public abstract class Command
    {
        protected Command(string commandName)
        {
            Name = commandName;
        }

        public abstract string Name { get; init; }
        public abstract Task<Message> Execute(Message message, ITelegramBotClient botClient);
        public bool Contains(string message)
            => Name.Equals(message);
    }
}