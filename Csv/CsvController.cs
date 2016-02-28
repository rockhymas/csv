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
            if (args[0] == "print")
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
            else if (args[0] == "printrow")
            {
                if (fileSystem.FileExists(args[1]))
                {
                    var row = 0;
                    if (!int.TryParse(args[2], out row))
                    {
                        console.Writeline(string.Format("'{0}' is not a valid row", args[2]));
                        return;
                    }

                    using (var stream = fileSystem.OpenFile(args[1]))
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

                        console.Writeline(string.Format("'{0}' does not contain row {1}", args[1], args[2]));
                    }
                }
                else
                {
                    console.Writeline(string.Format("There is no '{0}'", args[1]));
                }
            }
        }
    }
}
