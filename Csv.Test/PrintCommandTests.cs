using System.IO;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace Csv.Test
{
    [TestFixture]
    public class PrintCommandTests
    {
        [Test]
        public void NonexistentFileReportsError()
        {
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();
            var unit = new PrintCommand(fileSystem, console);

            unit.Execute(new []{ "file.csv" });

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")));
        }

        [Test]
        public void ExistingFileIsPrinted()
        {
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();
            var unit = new PrintCommand(fileSystem, console);

            fileSystem.Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true);
            fileSystem.Stub(fs => fs.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return((new MemoryStream(ASCIIEncoding.Default.GetBytes("the contents of file.csv"))));

            unit.Execute(new[] { "file.csv" });

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("the contents of file.csv")));
        }
    }
}
