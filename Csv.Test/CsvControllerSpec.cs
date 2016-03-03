using System.IO;
using System.Text;
using Rhino.Mocks;
using SpecEasy;

namespace Csv.Test
{
    public class CsvControllerSpec : Spec<CsvController>
    {
        public void Main()
        {
            string[] args = null;

            When("executing the csv controller", () => SUT.Execute(args));

            Given("the command to execute is 'notacommand file.csv'", () => args = new[] {"notacommand", "file.csv"}).Verify(() =>
                Then("output ''notacommand' is not a valid command'", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("'notacommand' is not a valid command")))));

            Given("the command to execute is 'print file.csv'", () => args = new []{ "print", "file.csv" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true)).Verify(() =>
                Given("'file.csv' contains 'the contents of file.csv'", () => Get<IFileSystem>().Stub(f => f.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return(new MemoryStream(ASCIIEncoding.Default.GetBytes("the contents of file.csv")))).Verify(() =>
                    Then("output 'the contents of file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("the contents of file.csv"))))));

            });

            Given("the command to execute is 'print file2.csv'", () => args = new[] { "print", "file2.csv" }).Verify(() =>
            Given("there is no file called 'file2.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file2.csv"))).Return(false)).Verify(() =>
                Then("output 'There is no 'file2.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file2.csv'"))))));

            Given("the command to execute is 'printrow file.csv 2'", () => args = new[] { "printrow", "file.csv", "2" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true)).Verify(() =>
                {
                    Given("'file.csv' contains 'the contents of file.csv'", () => Get<IFileSystem>().Stub(f => f.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return(new MemoryStream(ASCIIEncoding.Default.GetBytes("the contents of file.csv")))).Verify(() =>
                        Then("output ''file.csv' does not contain row 2", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("'file.csv' does not contain row 2")))));

                    Given("'file.csv' contains 'row 1 of file.csv\nrow 2 of file.csv'", () => Get<IFileSystem>().Stub(f => f.OpenFile(Arg<string>.Is.Equal("file.csv"))).Return(new MemoryStream(ASCIIEncoding.Default.GetBytes("row 1 of file.csv\nrow 2 of file.csv")))).Verify(() =>
                        Then("output 'row 2 of file.csv'", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("row 2 of file.csv")))));

                });
            });

            Given("the command to execute is 'printrow file.csv a'", () => args = new[] { "printrow", "file.csv", "a" }).Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(false)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")))));

                Given("there is a file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true)).Verify(() =>
                    Then("output ''a' is not a valid row'", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("'a' is not a valid row")))));
            });
        }
    }
}
