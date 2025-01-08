using FileTransfer.Commands;
using FileTransfer.Core.SocketTool;
using Spectre.Console;
using static FileTransfer.i18n.LanuageMananger;
// Start
using (var mutex = new Mutex(true, "FileTransfer", out bool createdNew))
{
    if (createdNew)
    {
        try
        {
            _ = Task.Run(() =>
             {
                 SocketServer.Start();
             });
            Run();
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }
    else
    {
        Console.WriteLine(GetString("AlreadyStartupTip"));
        Console.ReadKey();
    }
}

static void Run()
{
    var app = Commands.Init();
    var alignedText = new Align(new Markup($"[bold]{GetString("WelcomeTitle")}[/]"), HorizontalAlignment.Center);
    AnsiConsole.Write(alignedText);
    AnsiConsole.MarkupLine("[bold]Copyright (c) 2024 BugLordI. All rights reserved.[/]");
    AnsiConsole.MarkupLine($"{GetString("VersionLabel")}：[bold]V1.0.0[/]");
    AnsiConsole.MarkupLine($"[bold]{GetString("HelpTip")}[/]");
    AnsiConsole.WriteLine();
    while (true)
    {
        Commands.RunCommand("<FileTransfer>");
    }
}