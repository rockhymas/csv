using System.IO;

namespace Csv
{
    public class PrintCommand : CommandBase
    {
        public override void Execute(string[] args, IFileSystem fileSystem, IConsole console)
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