using System.IO;

namespace Csv
{
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
}