using System.IO;

namespace Csv
{
    public class PrintRowCommand : CommandBase
    {
        public override void Execute(string[] args, IFileSystem fileSystem, IConsole console)
        {
            if (!ValidateFileExists(args[0], fileSystem, console)) return;

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
                    if (row != 0) continue;
                    console.Writeline(rowContents);
                    return;
                }

                console.Writeline(string.Format("'{0}' does not contain row {1}", args[0], args[1]));
            }
        }
    }
}