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
            var mess = message.Text.Split(' ');
            int _minValue, _maxValue;
            
            if (mess.Length < 3)
                return  "Недостаточно данных";

            var minValue = int.TryParse(mess[1], out _minValue);
            var maxValue = int.TryParse(mess[2], out _maxValue);
            
            if (!minValue || !maxValue)
                return "Неверный формат данных"; 
            
            if (_minValue > _maxValue)
                return "Первое число должно быть меньше второго";
            
            var value = GetRandomValue(_minValue, _maxValue);
            return value.ToString();
        }
    }
}