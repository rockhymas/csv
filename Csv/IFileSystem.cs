using System.IO;

namespace Csv
{
    public interface IFileSystem
    {
        bool FileExists(string path);
        Stream OpenFile(string fileName);
    }
}
