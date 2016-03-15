using System.IO;
using System.Text;
using Rhino.Mocks;
using SpecEasy;

namespace Csv.Test
{
    public class PrintRowCommandSpec : Spec<PrintRowCommand>
    {
        public void Execute()
        {
            string[] args = null;

            When("executing the print row command", () => SUT.Execute(args, Get<IFileSystem>(), Get<IConsole>()));

            Given("the file to print is 'file.csv' and the row is '2'", () => args = new[] { "file.csv", "2" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline("There is no 'file.csv'"))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(true)).Verify(() =>
                {
                    Given("'file.csv' contains 'the contents of file.csv'", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("the contents of file.csv")))).Verify(() =>
                        Then("output ''file.csv' does not contain row 2", () => AssertWasCalled<IConsole>(c => c.Writeline("'file.csv' does not contain row 2"))));

                    Given("'file.csv' contains 'row 1 of file.csv\nrow 2 of file.csv'", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1 of file.csv\nrow 2 of file.csv")))).Verify(() =>
                        Then("output 'row 2 of file.csv'", () => AssertWasCalled<IConsole>(c => c.Writeline("row 2 of file.csv"))));
                });
            });

            Given("the file to print is 'file.csv' and the row is 'a'", () => args = new[] { "file.csv", "a" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline("There is no 'file.csv'"))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(true)).Verify(() =>
                    Then("output ''a' is not a valid row'", () => AssertWasCalled<IConsole>(c => c.Writeline("'a' is not a valid row"))));
            });

        }
    }
}
