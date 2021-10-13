using System;
using System.Threading.Tasks;
using Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commands
{
    public class TimeCommand : Command
    {
        public sealed override string Name { get; init; }

        public TimeCommand(string commandName) : base(commandName)
        {
            Name = commandName;
        }
        
        private static DateTime GetTime() 
            => DateTime.Now;

        public override async Task<Message> Execute(Message message, ITelegramBotClient client)
            => await client.SendTextMessageAsync(message.Chat.Id, $"Сейчас: {GetTime()}");
    }
}