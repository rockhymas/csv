using System.IO;
using System.Text;
using Rhino.Mocks;
using SpecEasy;

namespace Csv.Test
{
    public class PrintCommandSpec : Spec<PrintCommand>
    {
        public void Execute()
        {
            string[] args = null;

            When("executing the print command", () => SUT.Execute(args, Get<IFileSystem>(), Get<IConsole>()));
            
            Given("the file to print is file.csv", () => args = new []{ "file.csv" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline("There is no 'file.csv'"))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(true)).Verify(() =>
                Given("'file.csv' contains 'the contents of file.csv'", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("the contents of file.csv")))).Verify(() =>
                    Then("output 'the contents of file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline("the contents of file.csv")))));
            });

            Given("the file to print is file2.csv", () => args = new[] { "file2.csv" }).Verify(() =>
            Given("there is no file called 'file2.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file2.csv")).Return(false)).Verify(() =>
                Then("output 'There is no 'file2.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline("There is no 'file2.csv'")))));

        }
    }
}
