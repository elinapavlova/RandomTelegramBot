﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commands.Base;
using Infrastructure.Configurations;
using Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Services
{
public class Handlers
    {
        private static List<Command> _commands;
        
        public Handlers(CommandsConfiguration commandsConfiguration)
        {
            _commands = new CommandsStruct(commandsConfiguration).CommandList;
        }
        
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message       => BotOnMessageReceived(botClient, update.Message),
                _                        => botClient.SendTextMessageAsync(update.Message.Chat.Id, 
                                           "Команда не обнаружена", cancellationToken: cancellationToken)
            };

            try
            {
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
        private static async Task BotOnMessageReceived(ITelegramBotClient botClient, Message message)
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