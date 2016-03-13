using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace Csv
{
    public class DistinctValuesCommand : CommandBase
    {
        public override void Execute(string[] args, IFileSystem fileSystem, IConsole console)
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
            
            try
            {
                var columnExists = false;
                var values = new HashSet<string>();
                using (var stream = fileSystem.OpenFile(args[0]))
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var rowContents = streamReader.ReadLine();
                        var cells = rowContents.Split(',');
                        if (cells.Length >= column)
                        {
                            columnExists = true;
                            values.Add(cells[column - 1]);
                        }
                    }
                }

                if (!columnExists)
                {
                    console.Writeline(string.Format("'{0}' does not contain column {1}", args[0], args[1]));
                    return;
                }

                foreach (var value in values)
                {
                    console.Writeline(string.IsNullOrEmpty(value) ? "(blank)" : value);
                }
            }
            catch (IOException)
            {
                console.Writeline(string.Format("Error reading '{0}'", args[0]));
                return;
            }
        }
    }
}
