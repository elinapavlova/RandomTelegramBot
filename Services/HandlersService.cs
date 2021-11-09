using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands;
using Commands.Base;
using Infrastructure.Configurations;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Services.Contracts;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Services
{
public class HandlersService : IHandlersService
    {
        private static List<Command> _commands;
        
        public HandlersService
        (
            CommandsConfiguration commandsConfiguration, 
            IOptions<AppOptions> options,
            IOptions<SmtpClientOptions> smtpOptions
        )
        {
            _commands = new CommandsStruct(commandsConfiguration, options, smtpOptions).CommandList;
        }
        
        /// <summary>
        /// Обработка ошибок
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => "Telegram API Error:\n" +
                                                           $"[{apiRequestException.ErrorCode}]\n" +
                                                           $"{apiRequestException.Message}",
                _                                       => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Обработка обновления
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                var handler = update.Type switch
                {
                    UpdateType.Message => BotOnMessageReceived(botClient, update.Message),
                    _                  => botClient.SendTextMessageAsync(update.Message.Chat.Id, "Команда не обнаружена", 
                                                cancellationToken: cancellationToken)
                };
                
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }

        /// <summary>
        /// Обработка полученного сообщения
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="message"></param>
        private async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
        {
            // Если сообщение не текст - игнорировать
            if (message.Type != MessageType.Text)
                return;
            try
            {
                // Отделение первого слова (команды)
                var messageCommand = message.Text.Split(' ').FirstOrDefault();
                var isExistingCommand = _commands.Any(comm => comm.Contains(messageCommand));
                
                if (!isExistingCommand)
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Команда не обнаружена");

                var command = _commands.First(c => c.Name == messageCommand);
                await command.Execute(message, botClient as TelegramBotClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}