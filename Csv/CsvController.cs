using System.Collections.Generic;
using System.Linq;

namespace Csv
{
    public class CsvController
    {
        private readonly IFileSystem fileSystem;
        private readonly IConsole console;
        private Dictionary<string, ICommand> commands;

        public CsvController(IFileSystem fileSystem, IConsole console)
        {
            this.fileSystem = fileSystem;
            this.console = console;
            commands = new Dictionary<string, ICommand>
            {
                { "print", new PrintCommand() },
                { "printrow", new PrintRowCommand() },
            }; 
        }

        public void Execute(string[] args)
        {
            var commandName = args[0].ToLower();
            var commandArgs = args.Skip(1).ToArray();

            if (commands.ContainsKey(commandName))
            {
                var command = commands[commandName];
                command.Execute(commandArgs, fileSystem, console);
            }
            else
            {
                console.Writeline(string.Format("'{0}' is not a valid command", commandName));
            }
        }
    }
}
