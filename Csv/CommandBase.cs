namespace Csv
{
    public abstract class CommandBase : ICommand
    {
        public abstract void Execute(string[] args);

        protected static bool ValidateFileExists(string fileName, IFileSystem fileSystem, IConsole console)
        {
            if (!fileSystem.FileExists(fileName))
            {
                console.Writeline(string.Format("There is no '{0}'", fileName));
                return false;
            }

            return true;
        }
    }
}