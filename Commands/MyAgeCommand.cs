using System;
using System.Threading.Tasks;
using Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commands
{
    public class MyAgeCommand : Command
    {
        public sealed override string Name { get; init; }
        
        public MyAgeCommand(string commandName) : base(commandName)
        {
            Name = commandName;
        }
        
        public override async Task<Message> Execute(Message message, ITelegramBotClient client)
        {
            var result = ValidateMessage(message);
            return await client.SendTextMessageAsync(message.Chat.Id, result);
        }
        
        private string ValidateMessage(Message message)
        {
            var text = message.Text.Split(' ');

            if (text.Length < 2)
                return  "Недостаточно данных";

            DateTime.TryParse(text[1], out var dateBirth);
            
            if (dateBirth <= DateTime.MinValue || text.Length > 2)
                return  "Неверный формат данных. Введите команду в формате: \r\n /age " + DateTime.Today.ToShortDateString();

            if (dateBirth > DateTime.Today)
                return "Дата рождения не может быть позже текущей даты";
            
            var result = CreateMessage(dateBirth);
            
            return result;
        }
        
        private string CreateMessage(DateTime dateBirth)
        {
            var content = "Вам:\r\n";
            
            var today = DateTime.Now;
            var interval = today - dateBirth;
            var age = today.Subtract(dateBirth).Days / 365;

            content += $"{age} лет \r\n";
            content += $"или {(int)interval.TotalDays} дней ";
            content += $"{interval.Hours} часов ";
            content += $"{interval.Minutes} минут ";
            content += $"{interval.Seconds} секунд ";
            content += $"{interval.Milliseconds} миллисекунд";

            return content;
        }
    }
}