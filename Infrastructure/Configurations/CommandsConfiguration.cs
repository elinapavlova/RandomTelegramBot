using System.Collections.Generic;

namespace Infrastructure.Configurations
{
    public class CommandsConfiguration
    {
        public Dictionary<string, string> Commands { get; }
        public CommandsConfiguration(Dictionary<string, string> commands)
        {
            Commands = commands;
        }
    }
}