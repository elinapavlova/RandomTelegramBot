using System;
using System.Threading.Tasks;
using Commands.Base;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Commands
{
    public sealed class RandomNumberCommand : Command
    {
        private static readonly Random Random = new ();
        public override string Name { get; init; }
        
        public RandomNumberCommand(string commandName) : base(commandName)
        {
            Name = commandName;
        }

        /// Сгенерировать число
        private static int GetRandomValue(int minValue, int maxValue) 
            => Random.Next(minValue, maxValue);

        public override async Task<Message> Execute(Message message, ITelegramBotClient client)
        {
            var value = ValidateMessage(message);
            return await client.SendTextMessageAsync(message.Chat.Id, value);
        }
        
        private static string ValidateMessage(Message message)
        {
            var text = message.Text.Split(' ');

            if (text.Length < 3)
                return  "Недостаточно данных";

            int.TryParse(text[1], out var minValue);
            int.TryParse(text[2], out var maxValue);
            
            if ((text[1] != "0" || text[2] != "0") && (minValue == 0 || maxValue == 0))
                return "Неверный формат данных";

            var value = GetRandomValue(minValue, maxValue);
            return value.ToString();
        }
    }
}