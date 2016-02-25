using System;
using Rhino.Mocks;
using SpecEasy;

namespace Csv.Test
{
    public class CsvControllerSpec : Spec<CsvController>
    {
        public void Main()
        {
            string fileName = null;

            When("executing the csv controller", () => SUT.Execute(new []{ "print", fileName }));

            Given("the command to execute is 'print file.csv'", () => fileName = "file.csv").Verify(() =>
            {
                Given("there is no file called 'file.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file.csv"))).Return(true)).Verify(() =>
                    Then("output 'There is no 'file.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file.csv'")))));
            });

            Given("the command to execute is 'print file2.csv'", () => fileName = "file2.csv").Verify(() =>
            Given("there is no file called 'file2.csv'", () => Get<IFileSystem>().Stub(f => f.FileExists(Arg<string>.Is.Equal("file2.csv"))).Return(true)).Verify(() =>
                Then("output 'There is no 'file2.csv''", () => AssertWasCalled<IConsole>(c => c.Writeline(Arg<string>.Is.Equal("There is no 'file2.csv'"))))));
        }
    }
}
