using inausoft.netCLI.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class netCLITests
    {
        [TestMethod]
        public void Executor_RunCLI_ShouldRunProperCommandHandler_ForMultipleOptions()
        {
            //Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[] 
                            { 
                                "command1",
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString()
                            };

            var mockCommandHandler = new MockCommand1Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>();

            //Act
            var result = Executor.RunCLI(config, args, mockCommandHandler);

            //Assert
            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);
            
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldRunProperCommandHandler_WhenSetupUsingDI()
        {
            //Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "command1",
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString()
                            };

            var services = new ServiceCollection();
            services.AddCLI(config => {
                config.Map<Command1, MockCommand1Handler>();
            });

            var provider = services.BuildServiceProvider();

            //Act
            var result = provider.RunCLI(args);

            //Assert
            var mockCommandHandler = provider.GetRequiredService<MockCommand1Handler>();

            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldRunOnlyProperCommandHandler_WhenMultipleCommandHandlersAreMaped()
        {
            //Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOption", "true",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var mockCommand2Handler = new MockCommand2Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>()
                                               .Map<Command2, MockCommand2Handler>();

            //Act
            var result = Executor.RunCLI(config, args, mockCommand1Handler, mockCommand2Handler);

            //Assert
            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");

            Assert.IsNull(mockCommand2Handler.LastRunParameters);

            Assert.IsTrue(mockCommand1Handler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommand1Handler.LastRunParameters.NotOptionProperty);
            Assert.IsNull(mockCommand1Handler.LastRunParameters.StringOption);
            Assert.AreEqual(0, mockCommand1Handler.LastRunParameters.IntOption);
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldThrowInvalidCommmandException_ForNotMapedCommand()
        {
            var invalidCommandName = "invalidCommandName";

            //Arrange
            var args = new string[]
                            {
                                invalidCommandName,
                                "--boolOption", "true",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>();

            //Act & Assert
            var ex = Assert.ThrowsException<InvalidCommandException>(() => Executor.RunCLI(config, args, mockCommand1Handler));

            Assert.AreEqual(invalidCommandName, ex.CommandName);
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldThrowInvalidCommmandException_ForEmptyArguments()
        {
            //Arrange
            var args = new string[] { };

            var mockCommand1Handler = new MockCommand1Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>();

            //Act & Assert
            Assert.ThrowsException<InvalidCommandException>(() => Executor.RunCLI(config, args, mockCommand1Handler));
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldThrowInvalidOptionException_ForInvalidOption()
        {
            //Arrange
            var invalidOptionName = "invalidOption";
            var commandName = "command1";

            var args = new string[]
                            {
                                commandName,
                                $"--{invalidOptionName}", "true",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>();

            //Act & Assert
            var ex = Assert.ThrowsException<InvalidOptionException>(() => Executor.RunCLI(config, args, mockCommand1Handler));

            Assert.AreEqual(commandName, ex.CommandName);
            Assert.AreEqual(invalidOptionName, ex.OptionName);
        }
    }
}
