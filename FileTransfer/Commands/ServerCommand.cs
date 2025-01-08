﻿using FileTransfer.Core;
using FileTransfer.Core.SocketTool;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using static FileTransfer.i18n.LanuageMananger;
namespace FileTransfer.Commands
{
    internal class ServerCommand : Command<ServerCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "[ServerToConnect]")]
            public string ServerToConnect { get; set; }
        }


        public override int Execute(CommandContext context, ServerCommand.Settings settings)
        {
            if (String.IsNullOrEmpty(settings.ServerToConnect))
            {

            }
            else
            {
                SocketClient.Ping(settings.ServerToConnect, SocketServer.PORT, AnsiConsole.WriteLine);
            }
            return 0;
        }
    }
}