using System.IO;

namespace Csv
{
    class Program
    {
        static void Main(string[] args)
        {
            new CsvController(new FileSystem(), new Console()).Execute(args);
        }
    }

    internal class FileSystem : IFileSystem
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public Stream OpenFile(string fileName)
        {
            return new FileStream(fileName, FileMode.Open, FileAccess.Read);
        }
    }

    internal class Console : IConsole
    {
        public void Writeline(string output)
        {
            System.Console.WriteLine(output);
        }
    }
}
