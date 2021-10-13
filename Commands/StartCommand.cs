using System.Collections.Generic;
using System.Threading.Tasks;
using Commands.Base;
using Infrastructure.Configurations;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commands
{
    public class StartCommand : Command
    {
        public sealed override string Name { get; init; }
        private static Dictionary<string, string> _commands;
        
        public StartCommand(string commandName, CommandsConfiguration configuration) : base(commandName)
        {
            Name = commandName;
            _commands = configuration.Commands;
        }

        public override Task<Message> Execute(Message message, ITelegramBotClient client)
        {
            var content = MakeMessage(); 
            return client.SendTextMessageAsync (message.Chat.Id, content); 
        }
        
        private static string MakeMessage()
        {
            var content = "Список доступных команд:";
            
            foreach (var command in _commands)
            {
                content += "\r\n";
                content += command.Value;
            }

            return content;
        }
    }
}