using System.IO;
using System.Text;
using Rhino.Mocks;
using SpecEasy;

namespace Csv.Test
{
    internal sealed class DistinctValuesCommandSpec : Spec<DistinctValuesCommand>
    {
        public void Execute()
        {
            string[] args = null;

            When("executing the command", () => SUT.Execute(args, Get<IFileSystem>(), Get<IConsole>()));

            Given("the file to read is 'file.csv' and the column is 2", () => args = new []{ "file.csv", "2" }).Verify(() =>
            {
                Given("the file 'file.csv' does not exist", () => Get<IFileSystem>().Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(false)).Verify(() =>
                    Then("the output is 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")))));

                Given("the file 'file.csv exists", () => Get<IFileSystem>().Stub(fs => fs.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true)).Verify(() =>
                {
                    Given("the file fails to open", () => Get<IFileSystem>().Stub(fs => fs.OpenFile(Arg<string>.Is.Equal("file.csv"))).Throw(new IOException())).Verify(() =>
                        Then("the output is 'Error reading 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("Error reading 'file.csv'")))));

                    Given("the file contains 1 row, 1 column", () =>  Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1")))).Verify(() =>
                        Then("the output is ''file.csv' does not contain column 2'", () => AssertWasCalled<IConsole>(c => c.Writeline("'file.csv' does not contain column 2"))));

                    Given("the file contains 2 rows, 2 columns, distinct values", () => Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1,column 2\nrow 2,col 2")))).Verify(() =>
                    {
                        Then("the 1st distinct value is output", () => AssertWasCalled<IConsole>(c => c.Writeline("column 2")));
                        Then("the 2nd distinct value is output", () => AssertWasCalled<IConsole>(c => c.Writeline("col 2")));
                        Then("there are no other values output", () => AssertWasCalled<IConsole>(c => c.Writeline(""), mo => mo.IgnoreArguments().Repeat.Twice()));
                    });

                    Given("the file contains 2 rows, 2 columns, same values", () => Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1,column 2\nrow 2,column 2")))).Verify(() =>
                    {
                        Then("the 1st distinct value is output", () => AssertWasCalled<IConsole>(c => c.Writeline("column 2")));
                        Then("there are no other values output", () => AssertWasCalled<IConsole>(c => c.Writeline(""), mo => mo.IgnoreArguments().Repeat.Once()));
                    });

                    Given("the file contains 2 rows, 2 columns, one cell is empty", () => Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1,column 2\nrow 2,")))).Verify(() =>
                    {
                        Then("the 1st distinct value is output", () => AssertWasCalled<IConsole>(c => c.Writeline("column 2")));
                        Then("the 2nd distinct value (blank) is output", () => AssertWasCalled<IConsole>(c => c.Writeline("(blank)")));
                        Then("there are no other values output", () => AssertWasCalled<IConsole>(c => c.Writeline(""), mo => mo.IgnoreArguments().Repeat.Twice()));
                    });

                });
            });

            Given("the file to read is 'file.csv' and the row is 'a'", () => args = new[] { "file.csv", "a" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true)).Verify(() =>
                    Then("output ''a' is not a valid column'", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("'a' is not a valid column")))));
            });
        }
    }
}
