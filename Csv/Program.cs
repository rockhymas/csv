using System.Collections.Generic;

namespace Csv
{
    class Program
    {
        static void Main(string[] args)
        {
            var commands = new Dictionary<string, ICommand>
            {
                { "print", new PrintCommand() },
                { "printrow", new PrintRowCommand() },
            }; 

            new CsvController(new FileSystem(), new Console(), commands).Execute(args);
        }
    }
}
