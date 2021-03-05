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
            var config = new Mapping().Map<Command1>(mockCommandHandler);

            //Act
            var result = CLFlow.Create().UseMapping(config).Run(args);

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
            services.ConfigureCLFlow(config =>
            {
                config.Map<Command1, MockCommand1Handler>();
            });

            var provider = services.BuildServiceProvider();

            //Act
            var result = CLFlow.Create().UseServiceProvider(provider).Run(args);

            //Assert
            var mockCommandHandler = provider.GetRequiredService<MockCommand1Handler>();

            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldRunOnlyProperCommandHandler_WhenMultipleCommandHandlersAreRegistrated()
        {
            //Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOption", "true",
                                "--stringOption", "stringOptionValue",
                                "--intOption", "1"
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var mockCommand2Handler = new MockCommand2Handler();
            var config = new Mapping().Map<Command1>(mockCommand1Handler)
                                      .Map<Command2>(mockCommand2Handler);

            //Act
            var result = CLFlow.Create().UseMapping(config).Run(args);

            //Assert
            Assert.AreEqual(0, result, "RunCLI method indicted error in returned exit code");

            Assert.IsNull(mockCommand2Handler.LastRunParameters);
            Assert.IsNotNull(mockCommand1Handler.LastRunParameters);
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldThrowInvalidCommmandException_ForNotRegistratedCommand()
        {
            //Arrange
            var args = new string[]
                            {
                                "commandXX",
                                "--boolOption", "true",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var config = new Mapping().Map<Command1>(mockCommand1Handler);

            //Act
            var result = CLFlow.Create().UseMapping(config).Run(args);

            //Assert
            Assert.AreEqual((int)ErrorCode.UnrecognizedCommand, result);
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldThrowInvalidCommmandException_ForEmptyArguments()
        {
            //Arrange
            var args = new string[] { };

            var mockCommand1Handler = new MockCommand1Handler();
            var config = new Mapping().Map<Command1>(mockCommand1Handler);

            //Act
            var result = CLFlow.Create().UseMapping(config).Run(args);

            //Assert
            Assert.AreEqual((int)ErrorCode.UnspecifiedCommand, result);
        }

        [TestMethod]
        public void Executor_RunCLI_ShouldThrowInvalidOptionException_ForInvalidOption()
        {
            //Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOption", "true",
                                "--boolOptionXX", "true",
                                "--stringOption", "stringOptionValue",
                                "--intOption", "1"
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var mapping = new Mapping().Map<Command1>(mockCommand1Handler);

            //Act
            var result = CLFlow.Create().UseMapping(mapping).Run(args);

            //Assert
            Assert.AreEqual((int)ErrorCode.UnrecognizedOption, result);
        }
    }
}
