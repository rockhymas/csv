using System.Collections.Generic;
using SpecEasy;

namespace Csv.Test
{
    public class CsvControllerSpec : Spec<CsvController>
    {
        public void Execute()
        {
            string[] args = null;

            When("executing the csv controller", () => SUT.Execute(args));

            Given("the command to execute is 'notacommand file.csv'", () => args = new[] {"notacommand", "file.csv"}).Verify(() =>
                Then("output ''notacommand' is not a valid command'", () => AssertWasCalled<IConsole>(c => c.Writeline("'notacommand' is not a valid command"))));

            Given("a command is provided in the constructor", () => SUT = new CsvController(Get<IConsole>(), new Dictionary<string, ICommand> { {"command", Get<ICommand>()}})).Verify(() =>
            Given("the command is requested", () => args = new []{ "command", "arg1" }).Verify(() =>
                Then("the command is executed", () => AssertWasCalled<ICommand>(c => c.Execute(new [] { "arg1" })))));
        }
    }
}
