using System.IO;

namespace Csv
{
    public class CsvController
    {
        private readonly IFileSystem fileSystem;
        private readonly IConsole console;

        public CsvController(IFileSystem fileSystem, IConsole console)
        {
            this.fileSystem = fileSystem;
            this.console = console;
        }

        public void Execute(string[] args)
        {
            if (fileSystem.FileExists(args[1]))
            {
                using (var stream = fileSystem.OpenFile(args[1]))
                using (var streamReader = new StreamReader(stream))
                {
                    console.Writeline(streamReader.ReadToEnd());
                }
            }
            else
            {
                console.Writeline(string.Format("There is no '{0}'", args[1]));
            }
        }
    }
}
