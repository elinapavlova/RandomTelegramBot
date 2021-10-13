using System.Collections.Generic;

namespace Infrastructure.Options
{
    public class CommandsOptions
    {
        public const string Commands = "Commands";
        public Dictionary<string, string> Command { get; set; }
    }
}