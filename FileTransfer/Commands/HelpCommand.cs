using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel.DataAnnotations;
using static FileTransfer.i18n.LanuageMananger;
namespace FileTransfer.Commands
{
    internal class HelpCommand : Command<HelpCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[ParamName]")]
            public string ParamName { get; set; }
        }


        public override int Execute(CommandContext context, HelpCommand.Settings settings)
        {
            if (String.IsNullOrEmpty(settings.ParamName))
            {
                var table = new Table().Border(TableBorder.Rounded)
                                       .AddColumn($"[bold]{GetString("CommandHeaderTitle")}[/]")
                                       .AddColumn($"[bold]{GetString("CommandUsageTitle")}[/]")
                                       .AddColumn($"[bold]{GetString("CommandDescriptionTitle")}[/]")
                                       .Expand();
                table.AddRow("cls/clean/clear", "cls", GetString("HelpCls"));
                table.AddRow("exit", "exit", GetString("ExitHelp"));
                table.AddRow("help", "help [[xxx]]", GetString("HelpHelp"));
                table.AddRow("lang", "lang <Language-Code>", GetString("LangHelp"));
                table.AddRow("server/sl", "server [[IpToConnect]]", GetString("ServerHelp"));
                AnsiConsole.Write(table);
            }
            else
            {
                var info = Commands.GetCommandInfo(settings.ParamName);
                AnsiConsole.MarkupLine($"{settings.ParamName}:[bold]{info ?? GetString("UnkwonHelp")}[/]");
            }
            return 0;
        }
    }
}
