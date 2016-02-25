using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csv
{
    public class CsvController
    {
        private readonly IFileSystem fileSystem;
        private readonly IConsole console;

        public CsvController(IFileSystem fileSystem, IConsole console)
        {
            this.fileSystem = fileSystem;
            this.console = console;
        }

        public void Execute(string[] args)
        {
            console.Writeline(string.Format("There is no '{0}'", args[1]));
        }
    }
}
