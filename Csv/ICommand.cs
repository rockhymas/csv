namespace Csv
{
    public interface ICommand
    {
        void Execute(string[] args, IFileSystem fileSystem, IConsole console);
    }
}