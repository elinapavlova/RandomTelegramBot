using System.Threading.Tasks;
using Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commands
{
    public sealed class AboutMeCommand : Command
    {
        public AboutMeCommand(string commandName) : base(commandName)
        {
            Name = commandName;
        }

        public override string Name { get; init; }
        public override async Task<Message> Execute(Message message, ITelegramBotClient client)
        {
            var content = MakeMessage(message.From); 
            return await client.SendTextMessageAsync (message.Chat.Id, content); 
        }
        
        private static string MakeMessage(User user)
        {
            var content = "";
            content += "User Id: " + user.Id + "\r\n";
            content += "Username: " + user.Username + "\r\n";
            content += "Is User a bot: " + user.IsBot + "\r\n";
            content += "User Firstname: " + user.FirstName + "\r\n";
            content += "User Lastname" + user.LastName + "\r\n";

            return content;
        }
    }
}