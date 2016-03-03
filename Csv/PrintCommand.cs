using System.IO;

namespace Csv
{
    public class PrintCommand : ICommand
    {
        public void Execute(string[] args, IFileSystem fileSystem, IConsole console)
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