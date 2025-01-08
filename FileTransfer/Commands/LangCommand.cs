using FileTransfer.Core.SocketTool;
using FileTransfer.i18n;
using Spectre.Console;
using Spectre.Console.Cli;
namespace FileTransfer.Commands
{
    internal class LangCommand : Command<LangCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<Lang>")]
            public string ParamName { get; set; } = "zh-cn";
        }


        public override int Execute(CommandContext context, LangCommand.Settings settings)
        {
            LanuageMananger.SetLanguage(settings.ParamName, true);
            return 0;
        }
    }
}
