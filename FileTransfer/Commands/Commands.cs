using Spectre.Console;
using Spectre.Console.Cli;
using System.Text;
using static FileTransfer.i18n.LanuageMananger;

namespace FileTransfer.Commands
{
    internal class Commands
    {
        private static List<String> commandsHistory = [];
        private static readonly CommandApp app = new();
        private static int index = -1;
        private static Dictionary<String, String> commandsHelp;

        static Commands()
        {
            commandsHelp = new Dictionary<string, string>()
            {
                {"cls",GetString("HelpCls") },
                {"clear",GetString("HelpCls") },
                {"clean",GetString("HelpCls") },
                {"exit",GetString("ExitHelp") },
                {"help",GetString("HelpHelp") },
                {"lang",GetString("LangHelp") },
                {"server",GetString("ServerHelp") },
                {"sl",GetString("ServerHelp") }
            };
        }

        #region commands help



        #endregion

        public static CommandApp Init()
        {
            app.Configure(config =>
            {
                config.SetApplicationName("FileTransferCLI");
                config.AddCommand<ClearCommand>("cls");
                config.AddCommand<ClearCommand>("clear");
                config.AddCommand<ClearCommand>("clean");
                config.AddCommand<HelpCommand>("help");
                config.AddCommand<ServerCommand>("server");
                config.AddCommand<ServerCommand>("sl");
                config.AddCommand<ExitCommand>("exit");
                config.AddCommand<LangCommand>("lang");
            });
            return app;
        }

        public static int RunCommand(String prompt)
        {
            Console.Write(prompt);
            var command = ReadKey(prompt);
            if (String.IsNullOrEmpty(command))
            {
                return -1;
            }
            if (commandsHistory.LastOrDefault() != command)
            {
                commandsHistory.Add(command);
            }
            index = commandsHistory.Count;
            var arr = command.Split(' ').Where(e => !String.IsNullOrEmpty(e));
            return app.Run(arr);
        }

        public static String ReadKey(String prompt)
        {
            int cursorPosition = 0;
            String line = "";
            ConsoleKeyInfo key;
            StringBuilder sb = new StringBuilder();
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow && index > 0)
                {
                    index--;
                    line = commandsHistory[index];
                    Console.Write($"\r{new string(' ', Console.WindowWidth)}\r");
                    Console.Write($"{prompt}{line}");
                    cursorPosition = prompt.Length + line.Length;
                    sb.Clear();
                    sb.Append(line);
                }
                else if (key.Key == ConsoleKey.DownArrow && index < commandsHistory.Count - 1)
                {
                    index++;
                    line = index < commandsHistory.Count ? commandsHistory[index] : "";
                    Console.Write($"\r{new string(' ', Console.WindowWidth)}\r");
                    Console.Write($"{prompt}{line}");
                    cursorPosition = prompt.Length + line.Length;
                    sb.Clear();
                    sb.Append(line);
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (cursorPosition > 0 && cursorPosition - prompt.Length > 0)
                    {
                        cursorPosition--;
                        Console.SetCursorPosition(cursorPosition, Console.CursorTop);
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (cursorPosition > 0 && cursorPosition < prompt.Length + sb.Length)
                    {
                        cursorPosition++;
                        Console.SetCursorPosition(cursorPosition, Console.CursorTop);
                    }
                }
                else if (key.Key == ConsoleKey.Backspace && sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    Console.Write(key.KeyChar);
                    sb.Append(key.KeyChar);
                    cursorPosition = sb.Length + prompt.Length;
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return sb.ToString();
        }

        public static String? GetCommandInfo(String command)
        {
            if (commandsHelp.TryGetValue(command, out var sb))
            {
                return sb.ToString();
            }
            return null;
        }
    }
}
