using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Csv.Test
{
    [TestFixture]
    public class CsvControllerTests
    {
        [Test]
        public void InvalidCommandReportsError()
        {
            // Arrange
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();
            var unit = new CsvController(fileSystem, console, new Dictionary<string, ICommand>());

            // Act
            unit.Execute(new []{ "notacommand", "file.csv" });

            // Assert
            console.AssertWasCalled(c => c.Writeline(Arg<string>.Is.Equal("'notacommand' is not a valid command")));
        }

        [Test]
        public void ValidCommandIsExecuted()
        {
            // Arrange
            var fileSystem = MockRepository.GenerateStub<IFileSystem>();
            var console = MockRepository.GenerateStub<IConsole>();
            var command = MockRepository.GenerateStub<ICommand>();
            var unit = new CsvController(fileSystem, console, new Dictionary<string, ICommand> { { "command", command } });

            // Act
            unit.Execute(new[] { "command", "arg1" });

            // Assert
            command.AssertWasCalled(c => c.Execute(Arg<string[]>.Is.Equal(new[] { "arg1" }), Arg<IFileSystem>.Is.Equal(fileSystem), Arg<IConsole>.Is.Equal(console)));
        }
    }
}
