using System;
using System.Collections.Generic;
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

            var column = int.Parse(args[1]);

            var columnExists = false;
            var values = new HashSet<string>();
            using (var stream = fileSystem.OpenFile(args[0]))
            using (var streamReader = new StreamReader(stream))
            {
                while (!streamReader.EndOfStream)
                {
                    var row = streamReader.ReadLine();
                    var cells = row.Split(',');
                    if (cells.Length >= column)
                    {
                        columnExists = true;
                        var cell = cells[column - 1];
                        if (!values.Contains(cell))
                        {
                            console.Writeline(cell);
                        }
                        values.Add(cell);
                    }
                }

                if (!columnExists)
                {
                    console.Writeline(string.Format("'{0}' does not contain column {1}", args[0], args[1]));
                }
            }

        }
    }
}
