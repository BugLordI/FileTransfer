using FileTransfer.Core.SocketTool;
using Spectre.Console;
using Spectre.Console.Cli;
namespace FileTransfer.Commands
{
    internal class ExitCommand : Command<ExitCommand.Settings>
    {
        public class Settings : CommandSettings
        {
           
        }


        public override int Execute(CommandContext context, ExitCommand.Settings settings)
        {
            Environment.Exit(0);
            return 0;
        }
    }
}
