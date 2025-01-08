using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransfer.Commands
{
    internal class ClearCommand : Command<ClearCommand.Settings>
    {
        public class Settings : CommandSettings
        {
        }


        public override int Execute(CommandContext context, ClearCommand.Settings settings)
        {
            AnsiConsole.Clear();
            return 0;
        }
    }
}
