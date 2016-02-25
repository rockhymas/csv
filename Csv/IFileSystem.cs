using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csv
{
    public interface IConsole
    {
        void Writeline(string output);
    }

    public interface IFileSystem
    {
        bool FileExists(string path);
    }
}
