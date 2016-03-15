using System.IO;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace Csv.Test
{
    [TestFixture]
    public class SumColumnCommandTests
    {
        private SumColumnCommand unit;
        private IFileSystem fileSystem;
        private IConsole console;

        [SetUp]
        public void Setup()
        {
            fileSystem = MockRepository.GenerateStub<IFileSystem>();
            console = MockRepository.GenerateStub<IConsole>();
            unit = new SumColumnCommand(fileSystem, console);
        }

        [Test]
        public void NonexistentFileAndValidColumnReportsFileError()
        {
            unit.Execute(new[] { "file.csv", "2" });

            console.AssertWasCalled(c => c.Writeline("There is no 'file.csv'"));
        }

        [Test]
        public void NonexistentFileAndInvalidColumnReportsFileError()
        {
            unit.Execute(new[] { "file.csv", "a" });

            console.AssertWasCalled(c => c.Writeline("There is no 'file.csv'"));
        }

        [Test]
        public void ExistingFileAndInvalidColumnReportsColumnError()
        {
            fileSystem.Stub(fs => fs.FileExists("file.csv")).Return(true);
            
            unit.Execute(new[] { "file.csv", "a" });

            console.AssertWasCalled(c => c.Writeline("'a' is not a valid column"));
        }

        [Test]
        public void ColumnOutOfBoundsReported()
        {
            fileSystem.Stub(fs => fs.FileExists("file.csv")).Return(true);
            fileSystem.Stub(fs => fs.OpenFile("file.csv")).Return((new MemoryStream(Encoding.Default.GetBytes("the contents of file.csv"))));

            unit.Execute(new[] { "file.csv", "2" });

            console.AssertWasCalled(c => c.Writeline("'file.csv' does not contain column 2"));
        }

        [Test]
        public void ColumnWithoutNumbersSumsTo0()
        {
            fileSystem.Stub(fs => fs.FileExists("file.csv")).Return(true);
            fileSystem.Stub(fs => fs.OpenFile("file.csv")).Return((new MemoryStream(Encoding.Default.GetBytes("the contents, of file.csv"))));

            unit.Execute(new[] { "file.csv", "2" });

            console.AssertWasCalled(c => c.Writeline("0"));
        }

        [Test]
        public void ColumnWithSomeNumbersSumsNumbers()
        {
            fileSystem.Stub(fs => fs.FileExists("file.csv")).Return(true);
            fileSystem.Stub(fs => fs.OpenFile("file.csv")).Return((new MemoryStream(Encoding.Default.GetBytes("row 1, a\nrow 2, 2"))));

            unit.Execute(new[] { "file.csv", "2" });

            console.AssertWasCalled(c => c.Writeline("2"));
        }

        [Test]
        public void ColumnWithAllNumbersSumsNumbers()
        {
            fileSystem.Stub(fs => fs.FileExists("file.csv")).Return(true);
            fileSystem.Stub(fs => fs.OpenFile("file.csv")).Return((new MemoryStream(Encoding.Default.GetBytes("row 1, 1\nrow 2, 2"))));

            unit.Execute(new[] { "file.csv", "2" });

            console.AssertWasCalled(c => c.Writeline("3"));
        }

        [Test]
        public void IncompleteColumnWithSomeNumbersSumsNumbers()
        {
            fileSystem.Stub(fs => fs.FileExists("file.csv")).Return(true);
            fileSystem.Stub(fs => fs.OpenFile("file.csv")).Return((new MemoryStream(Encoding.Default.GetBytes("row 1, 1\nrow 2"))));

            unit.Execute(new[] { "file.csv", "2" });

            console.AssertWasCalled(c => c.Writeline("1"));
        }
    }
}
