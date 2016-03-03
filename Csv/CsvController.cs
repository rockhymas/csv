using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Csv
{
    public class CsvController
    {
        private readonly IFileSystem fileSystem;
        private readonly IConsole console;
        private Dictionary<string, Action<string[]>> commands;

        public CsvController(IFileSystem fileSystem, IConsole console)
        {
            this.fileSystem = fileSystem;
            this.console = console;
            commands = new Dictionary<string, Action<string[]>>
            {
                { "print", PrintCommand },
                { "printrow", PrintRowCommand },
            }; 
        }

        public void Execute(string[] args)
        {
            var commandName = args[0].ToLower();
            var commandArgs = args.Skip(1).ToArray();

            if (commands.ContainsKey(commandName))
            {
                var command = commands[commandName];
                command(commandArgs);
            }
            else
            {
                console.Writeline(string.Format("'{0}' is not a valid command", commandName));
            }
        }

        private void PrintRowCommand(string[] args)
        {
            if (!fileSystem.FileExists(args[0]))
            {
                console.Writeline(string.Format("There is no '{0}'", args[0]));
                return;
            }

            int row;
            if (!int.TryParse(args[1], out row))
            {
                console.Writeline(string.Format("'{0}' is not a valid row", args[1]));
                return;
            }

            using (var stream = fileSystem.OpenFile(args[0]))
            using (var streamReader = new StreamReader(stream))
            {
                while (row > 0 && !streamReader.EndOfStream)
                {
                    var rowContents = streamReader.ReadLine();
                    row--;
                    if (row == 0)
                    {
                        console.Writeline(rowContents);
                        return;
                    }
                }

                console.Writeline(string.Format("'{0}' does not contain row {1}", args[0], args[1]));
            }
        }

        private void PrintCommand(string[] args)
        {
            if (!fileSystem.FileExists(args[0]))
            {
                console.Writeline(string.Format("There is no '{0}'", args[0]));
                return;
            }

            using (var stream = fileSystem.OpenFile(args[0]))
            using (var streamReader = new StreamReader(stream))
            {
                console.Writeline(streamReader.ReadToEnd());
            }
        }
    }
}
