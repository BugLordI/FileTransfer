using FileTransfer.Commands;
using FileTransfer.Core.SocketTool;
using Spectre.Console;
using System.Diagnostics;
using static FileTransfer.i18n.LanuageMananger;

// SetLanguage
bool languageSet = false;
if (args.Length > 0)
{
    var langConfig = args.Where(e => e.Contains("-local:")).FirstOrDefault();
    if (!String.IsNullOrEmpty(langConfig))
    {
        var arr = langConfig.Split(':');
        if (arr.Length > 1)
        {
            SetLanguage(arr[1], true);
            languageSet = true;
        }
    }
}

if (!languageSet)
{
    LoadLanguage();
}

// Language changed listener
OnLanguageChanged(lang =>
{
    String result = AnsiConsole.Ask<String>($"{GetString("RestartAsk")}([red]y[/]/[yellow]n[/])");
    if (result.ToLower() == "y")
    {
        Process.Start(Process.GetCurrentProcess().MainModule.FileName);
        Environment.Exit(0);
    }
});

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
    Commands.Init();
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