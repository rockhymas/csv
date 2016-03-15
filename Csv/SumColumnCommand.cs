using System.Globalization;
using System.IO;

namespace Csv
{
    public class SumColumnCommand : CommandBase
    {
        private readonly IFileSystem fileSystem;
        private readonly IConsole console;

        public SumColumnCommand(IFileSystem fileSystem, IConsole console)
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

            int column;
            if (!int.TryParse(args[1], out column))
            {
                console.Writeline(string.Format("'{0}' is not a valid column", args[1]));
                return;
            }

            var columnExists = false;
            var sum = 0;
            using (var stream = fileSystem.OpenFile(args[0]))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var rowContents = streamReader.ReadLine();
                    var cells = rowContents == null ? new string[0] : rowContents.Split(',');
                    if (cells.Length >= column)
                    {
                        columnExists = true;
                        int cellValue;
                        if (int.TryParse(cells[column - 1], out cellValue))
                        {
                            sum += cellValue;
                        }
                    }
                }
            }

            if (columnExists)
            {
                console.Writeline(sum.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                console.Writeline(string.Format("'{0}' does not contain column {1}", args[0], args[1]));
            }
        }
    }
}
