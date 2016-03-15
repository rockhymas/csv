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
                Given("'file.csv' does not exist", () => Get<IFileSystem>().Stub(fs => fs.FileExists("file.csv")).Return(false)).Verify(() =>
                    Then("the output is 'There is no 'file.csv'", () => AssertWasCalled<IConsole>(c => c.Writeline("There is no 'file.csv'"))));

                Given("'file.csv' exists", () => Get<IFileSystem>().Stub(fs => fs.FileExists("file.csv")).Return(true)).Verify(() =>
                {
                    Given("'file.csv' contains 1 row, 1 column", () => Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1")))).Verify(() =>
                        Then("output ''file.csv' does not contain column 2'", () => AssertWasCalled<IConsole>(c => c.Writeline("'file.csv' does not contain column 2"))));

                    Given("'file.csv' contains 1 row, 2 columns", () => Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1,column 2")))).Verify(() =>
                        Then("output 'column 2'", () => AssertWasCalled<IConsole>(c => c.Writeline("column 2"))));

                    Given("'file.csv' contains 2 row, 2 columns, same values", () => Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1,column 2\nrow 2,column 2")))).Verify(() =>
                        Then("output 'column 2'", () => AssertWasCalled<IConsole>(c => c.Writeline("column 2"), mo => mo.Repeat.Once())));

                    Given("'file.csv' contains 2 row, 2 columns, distinct values", () => Get<IFileSystem>().Stub(fs => fs.OpenFile("file.csv")).Return(new MemoryStream(Encoding.Default.GetBytes("row 1,column 2\nrow 2,col 2")))).Verify(() =>
                    {
                        Then("output 'column 2'", () => AssertWasCalled<IConsole>(c => c.Writeline("column 2"), mo => mo.Repeat.Once()));
                        Then("output 'col 2'", () => AssertWasCalled<IConsole>(c => c.Writeline("col 2"), mo => mo.Repeat.Once()));
                        Then("two distinct values are output", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Anything), mo => mo.Repeat.Twice()));
                    });
                });
            });
        }
    }
}
