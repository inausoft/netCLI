using inausoft.netCLI.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class netCLITests
    {
        [TestMethod]
        public void CLIFlow_Run_ShouldRunProperCommandHandler_ForMultipleOptions()
        {
            // Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "command1",
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString(),
                            };

            var mockCommandHandler = new MockCommand1Handler();
            var mapping = new CommandMapping().Map<Command1>(mockCommandHandler);

            // Act
            var result = CLIFlow.Create().UseMapping(mapping).Run(args);

            // Assert
            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);

        }

        [TestMethod]
        public void CLIFlow_Run_ShouldRunProperCommandHandler_WhenSetupUsingDI()
        {
            // Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "command1",
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString(),
                            };

            var services = new ServiceCollection();
            services.ConfigureCLIFlow(mapping =>
            {
                mapping.Map<Command1, MockCommand1Handler>();
            });

            var provider = services.BuildServiceProvider();

            // Act
            var result = CLIFlow.Create().UseServiceProvider(provider).Run(args);

            // Assert
            var mockCommandHandler = provider.GetRequiredService<MockCommand1Handler>();

            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);
        }

        [TestMethod]
        public void CLIFlow_Run_ShouldRunOnlyProperCommandHandler_WhenMultipleCommandHandlersAreMapped()
        {
            // Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOption", "true",
                                "--stringOption", "stringOptionValue",
                                "--intOption", "1",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var mockCommand2Handler = new MockCommand2Handler();
            var mapping = new CommandMapping().Map<Command1>(mockCommand1Handler)
                                       .Map<Command2>(mockCommand2Handler);

            // Act
            var result = CLIFlow.Create().UseMapping(mapping).Run(args);

            // Assert
            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");

            Assert.IsNull(mockCommand2Handler.LastRunParameters);
            Assert.IsNotNull(mockCommand1Handler.LastRunParameters);
        }

        [TestMethod]
        public void CLIFlow_Run_ShouldThrowInvalidCommmandException_ForNotMappedCommand()
        {
            var invalidCommandName = "invalidCommandName";

            // Arrange
            var args = new string[]
                            {
                                invalidCommandName,
                                "--boolOption", "true",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var mapping = new CommandMapping().Map<Command1>(mockCommand1Handler);

            // Act
            var result = CLIFlow.Create().UseMapping(mapping).Run(args);

            // Assert
            Assert.AreEqual((int)ErrorCode.UnrecognizedCommand, result);
        }

        [TestMethod]
        public void CLIFlow_Run_ShouldThrowInvalidCommmandException_ForEmptyArguments()
        {
            // Arrange
            var args = new string[] { };

            var mockCommand1Handler = new MockCommand1Handler();
            var mapping = new CommandMapping().Map<Command1>(mockCommand1Handler);

            // Act
            var result = CLIFlow.Create().UseMapping(mapping).Run(args);

            // Assert
            Assert.AreEqual((int)ErrorCode.UnspecifiedCommand, result);
        }

        [TestMethod]
        public void CLIFlow_Run_ShouldThrowInvalidOptionException_ForInvalidOption()
        {
            // Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOption", "true",
                                "--boolOptionXX", "true",
                                "--stringOption", "stringOptionValue",
                                "--intOption", "1",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var mapping = new CommandMapping().Map<Command1>(mockCommand1Handler);

            // Act
            var result = CLIFlow.Create().UseMapping(mapping).Run(args);

            // Assert
            Assert.AreEqual((int)ErrorCode.UnrecognizedOption, result);
        }

        [TestMethod]
        public void CLIFlow_Run_ShouldRunDefaultCommand_WhenNoCommandWasSpecified()
        {
            // Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString(),
                            };

            var mockCommandHandler = new MockCommand1Handler();
            var mapping = new CommandMapping().MapDefault<Command1>(mockCommandHandler);

            // Act
            var result = CLIFlow.Create().UseMapping(mapping).Run(args);

            //Assert
            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);
        }

        [TestMethod]
        public void CLIFlow_Run_ShouldRunDefaultCommand_WhenNoCommandWasSpecified_WithDI()
        {
            // Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString(),
                            };


            var services = new ServiceCollection();
            services.ConfigureCLIFlow(mapping =>
            {
                mapping.MapDefault<Command1, MockCommand1Handler>();
            });

            var provider = services.BuildServiceProvider();

            // Act
            var result = CLIFlow.Create().UseServiceProvider(provider).Run(args);

            // Assert
            var mockCommandHandler = provider.GetRequiredService<MockCommand1Handler>();

            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);
        }

        [TestMethod]
        public void CLIFlow_Run_ShouldRunProperCommand_WhenCommandNameIncludeDash()
        {
            // Arrange
            var stringOptionValue = "sampleString";

            var args = new string[]
                            {
                                "command-sample",
                                "--stringOption", stringOptionValue,
                            };


            var mockCommandHandler = new MockCommand3Handler();
            var mapping = new CommandMapping().MapDefault<Command3>(mockCommandHandler);

            // Act
            var result = CLIFlow.Create().UseMapping(mapping).Run(args);

            // Assert
            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
        }
    }
}
