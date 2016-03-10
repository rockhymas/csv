using System.IO;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace Csv.Test
{
    [TestFixture]
    public class SumColumnCommandTests
    {
        [Test]
        public void NonexistentFileAndValidColumnReportsFileError()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            unit.Execute(new[] { "file.csv", "2" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")));
        }

        [Test]
        public void NonexistentFileAndInvalidColumnReportsFileError()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            unit.Execute(new[] { "file.csv", "a" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")));
        }

        [Test]
        public void ExistingFileAndInvalidColumnReportsColumnError()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            fileSystem.Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true);
            
            unit.Execute(new[] { "file.csv", "a" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("'a' is not a valid column")));
        }

        [Test]
        public void ColumnOutOfBoundsReported()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            fileSystem.Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true);
            fileSystem.Stub(fs => fs.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return((new MemoryStream(ASCIIEncoding.Default.GetBytes("the contents of file.csv"))));

            unit.Execute(new[] { "file.csv", "2" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("'file.csv' does not contain column 2")));
        }

        [Test]
        public void ColumnWithoutNumbersSumsTo0()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            fileSystem.Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true);
            fileSystem.Stub(fs => fs.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return((new MemoryStream(ASCIIEncoding.Default.GetBytes("the contents, of file.csv"))));

            unit.Execute(new[] { "file.csv", "2" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("0")));
        }

        [Test]
        public void ColumnWithSomeNumbersSumsNumbers()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            fileSystem.Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true);
            fileSystem.Stub(fs => fs.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return((new MemoryStream(ASCIIEncoding.Default.GetBytes("row 1, a\nrow 2, 2"))));

            unit.Execute(new[] { "file.csv", "2" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("2")));
        }

        [Test]
        public void ColumnWithAllNumbersSumsNumbers()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            fileSystem.Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true);
            fileSystem.Stub(fs => fs.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return((new MemoryStream(ASCIIEncoding.Default.GetBytes("row 1, 1\nrow 2, 2"))));

            unit.Execute(new[] { "file.csv", "2" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("3")));
        }

        [Test]
        public void IncompleteColumnWithSomeNumbersSumsNumbers()
        {
            var unit = new SumColumnCommand();
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();

            fileSystem.Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true);
            fileSystem.Stub(fs => fs.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return((new MemoryStream(ASCIIEncoding.Default.GetBytes("row 1, 1\nrow 2"))));

            unit.Execute(new[] { "file.csv", "2" }, fileSystem, console);

            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("1")));
        }
    }
}
