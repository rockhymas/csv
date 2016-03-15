using System.Collections.Generic;

namespace Csv
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileSystem = new FileSystem();
            var console = new Console();
            var commands = new Dictionary<string, ICommand>
            {
                { "print", new PrintCommand(fileSystem, console) },
                { "printrow", new PrintRowCommand(fileSystem, console) },
                { "sumcolumn", new SumColumnCommand(fileSystem, console) },
            };

            new CsvController(console, commands).Execute(args);
        }
    }
}
