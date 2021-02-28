using inausoft.netCLI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class RootCommandHandlerTests
    {
        [TestMethod]
        public void RootCommandHandler_RunsProperCommandHandler_ForMultipleOptions()
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
            var result = netCLI.RunCLI(config, args, mockCommandHandler);

            //Assert
            Assert.AreEqual(0, result);
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);
            
        }

        [TestMethod]
        public void RootCommandHandler_RunsProperCommandHandler_ForDifferentStringOptions()
        {
            //Arrange
            var stringOptionValue = "s";

            var args = new string[]
                            {
                                "command1",
                                "--stringOption", stringOptionValue
                            };

            var mockCommandHandler = new MockCommand1Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>();

            //Act
            var result = netCLI.RunCLI(config, args, mockCommandHandler);

            //Assert
            Assert.AreEqual(0, result);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);

        }

        [TestMethod]
        public void RootCommandHandler_RunsOnlyProperCommandHandler_WhenMultipleCommandHandlersAreRegistrated()
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
            var result = netCLI.RunCLI(config, args, mockCommand1Handler, mockCommand2Handler);

            //Assert
            Assert.AreEqual(0, result);

            Assert.IsNull(mockCommand2Handler.LastRunParameters);

            Assert.IsTrue(mockCommand1Handler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommand1Handler.LastRunParameters.NotOptionProperty);
            Assert.IsNull(mockCommand1Handler.LastRunParameters.StringOption);
            Assert.AreEqual(0, mockCommand1Handler.LastRunParameters.IntOption);
        }


        [ExpectedException(typeof(InvalidCommandException))]
        [TestMethod]
        public void RootCommandHandler_ThrowsInvalidCommmandException_ForNotRegistratedCommand()
        {
            //Arrange
            var args = new string[]
                            {
                                "commandXX",
                                "--boolOption", "true",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>();

            //Act
            netCLI.RunCLI(config, args, mockCommand1Handler, mockCommand1Handler);

            //Assert with exception
        }

        [ExpectedException(typeof(InvalidOptionException))]
        [TestMethod]
        public void RootCommandHandler_ThrowsInvalidOptionException_ForInvalidOption()
        {
            //Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOptionXX", "true",
                            };

            var mockCommand1Handler = new MockCommand1Handler();
            var config = new CLIConfiguration().Map<Command1, MockCommand1Handler>();

            //Act
            netCLI.RunCLI(config, args, mockCommand1Handler, mockCommand1Handler);

            //Assert with exception
        }
    }
}
