namespace Csv
{
    internal class Console : IConsole
    {
        public void Writeline(string output)
        {
            System.Console.WriteLine(output);
        }
    }
}