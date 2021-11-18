using System.Collections.Generic;
using Commands.Base;
using Infrastructure.Configurations;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Commands
{
    public readonly struct CommandsStruct
    {
        public CommandsStruct
        (
            CommandsConfiguration commandsConfiguration, 
            IOptions<AppOptions> options, 
            IOptions<SmtpClientOptions> smtpOptions
        )
        {
            var commands = commandsConfiguration.Commands;
            
            CommandList = new List<Command>
            {
                new StartCommand(commands[nameof(StartCommand)], commandsConfiguration),
                new RandomNumberCommand(commands[nameof(RandomNumberCommand)]),
                new TimeCommand(commands[nameof(TimeCommand)]),
                new AboutMeCommand(commands[nameof(AboutMeCommand)]),
                new MyAgeCommand(commands[nameof(MyAgeCommand)]),
                new SendMessageToEmailCommand(commands[nameof(SendMessageToEmailCommand)], options, smtpOptions)
            };
        }

        public readonly List<Command> CommandList;
    }
}