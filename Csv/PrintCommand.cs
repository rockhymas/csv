using System.IO;

namespace Csv
{
    public class PrintCommand : CommandBase
    {
        private readonly IFileSystem fileSystem;
        private readonly IConsole console;

        public PrintCommand(IFileSystem fileSystem, IConsole console)
        {
            this.fileSystem = fileSystem;
            this.console = console;
        }

        public override void Execute(string[] args)
        {
            if (!ValidateFileExists(args[0], fileSystem, console))
            {
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