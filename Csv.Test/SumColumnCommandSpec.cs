using System.IO;
using System.Text;
using Rhino.Mocks;
using SpecEasy;

namespace Csv.Test
{
    public class SumColumnCommandSpec : Spec<SumColumnCommand>
    {
        public void Execute()
        {
            string[] args = null;

            When("executing the sum column command", () => SUT.Execute(args));

            Given("the file to read is 'file.csv' and the column is 2", () => args = new []{ "file.csv", "2" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline("There is no 'file.csv'"))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(true)).Verify(() =>
                {
                    Given("'file.csv' contains 1 row, 1 column", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("the contents of file.csv")))).Verify(() =>
                        Then("output ''file.csv' does not contain column 2", () => AssertWasCalled<IConsole>(c => c.Writeline("'file.csv' does not contain column 2"))));

                    Given("'file.csv' contains 2 rows, 2 columns, no numbers", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1, of file.csv\nrow 2, of file.csv")))).Verify(() =>
                        Then("output '0'", () => AssertWasCalled<IConsole>(c => c.Writeline("0"))));

                    Given("'file.csv' contains 2 rows with numbers in the 2nd column", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1, 1\nrow 2, 2")))).Verify(() =>
                        Then("output '3'", () => AssertWasCalled<IConsole>(c => c.Writeline("3"))));

                    Given("'file.csv' contains 2 rows, with numbers in some of the 2nd column", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1, 1\nrow 2, funnybone")))).Verify(() =>
                        Then("output '1'", () => AssertWasCalled<IConsole>(c => c.Writeline("1"))));

                    Given("'file.csv' contains 2 rows, 1 with 2 columns and 1 with 1 column", () => Get<IFileSystem>().Stub(f => f.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1, 1\nrow 2")))).Verify(() =>
                        Then("output '1'", () => AssertWasCalled<IConsole>(c => c.Writeline("1"))));
                });
            });

            Given("the file to read is 'file.csv' and the row is 'a'", () => args = new[] { "file.csv", "a" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline("There is no 'file.csv'"))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists("file.csv")).Return(true)).Verify(() =>
                    Then("output ''a' is not a valid column'", () => AssertWasCalled<IConsole>(c => c.Writeline("'a' is not a valid column"))));
            });
        }
    }
}
