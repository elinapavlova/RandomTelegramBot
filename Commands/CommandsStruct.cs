using System.Collections.Generic;
using Commands.Base;
using Commands;
using Infrastructure.Configurations;

namespace Models
{
    public readonly struct CommandsStruct
    {
        public CommandsStruct(CommandsConfiguration commandsConfiguration)
        {
            var commands = commandsConfiguration.Commands;
            
            CommandList = new List<Command>
            {
                new StartCommand(commands[nameof(StartCommand)], commandsConfiguration),
                new RandomNumberCommand(commands[nameof(RandomNumberCommand)]),
                new TimeCommand(commands[nameof(TimeCommand)])
            };
        }

        public readonly List<Command> CommandList;
    }
}