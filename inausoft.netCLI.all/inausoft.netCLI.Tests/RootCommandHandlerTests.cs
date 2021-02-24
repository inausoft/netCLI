using inausoft.netCLI.MateuszCommands;
using inausoft.netCLI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using static inausoft.netCLI.MateuszCommands.MateuszCommandHandler;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class RootCommandHandlerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Arrange
            var stringOptionValue = "sampleString";
            var pathOptionValue = @"C:\Program%Files";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "command1",
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--pathOption", pathOptionValue,
                                "--intOption", intOptionValue.ToString()
                            };

            //Act
            var mockCommandHandler = new MockCommand1Handler();
            var rootCommandHanlder = new RootCommandHandler(new ICommandHandler[] { mockCommandHandler });

            var result = rootCommandHanlder.Run(args);

            //Assert
            Assert.AreEqual(0, result);
            Assert.IsTrue(mockCommandHandler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);
            Assert.AreEqual(pathOptionValue, mockCommandHandler.LastRunParameters.PathOption);
            Assert.AreEqual(intOptionValue, mockCommandHandler.LastRunParameters.IntOption);

        }

        [TestMethod]
        public void TestMethod4()
        {
            //Arrange
            var stringOptionValue = "s";

            var args = new string[]
                            {
                                "command1",
                                "--stringOption", stringOptionValue
                            };

            //Act
            var mockCommandHandler = new MockCommand1Handler();
            var rootCommandHanlder = new RootCommandHandler(new ICommandHandler[] { mockCommandHandler });

            var result = rootCommandHanlder.Run(args);

            //Assert
            Assert.AreEqual(0, result);
            Assert.IsNull(mockCommandHandler.LastRunParameters.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, mockCommandHandler.LastRunParameters.StringOption);

        }

        [TestMethod]
        public void TestMethod2()
        {
            //Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOption", "true",
                            };

            //Act
            var mockCommand1Handler = new MockCommand1Handler();
            var mockCommand2Handler = new MockCommand2Handler();
            var rootCommandHanlder = new RootCommandHandler(new ICommandHandler[] { mockCommand2Handler, mockCommand1Handler });

            var result = rootCommandHanlder.Run(args);

            //Assert
            Assert.AreEqual(0, result);

            Assert.IsNull(mockCommand2Handler.LastRunParameters);

            Assert.IsTrue(mockCommand1Handler.LastRunParameters.BoolOption);
            Assert.IsNull(mockCommand1Handler.LastRunParameters.NotOptionProperty);
            Assert.IsNull(mockCommand1Handler.LastRunParameters.StringOption);
            Assert.IsNull(mockCommand1Handler.LastRunParameters.PathOption);
            Assert.AreEqual(0, mockCommand1Handler.LastRunParameters.IntOption);
        }

        [TestMethod]
        public void Command2WithoutParameters_ReturnsCorrectCommand()
        {
            //Arrange
            var args = new string[]
                            {
                                "command2",
                            };

            //Act
            var mockCommand1Handler = new MockCommand1Handler();
            var mockCommand2Handler = new MockCommand2Handler();
            var rootCommandHandler = new RootCommandHandler(new ICommandHandler[] { mockCommand2Handler, mockCommand1Handler });

            var result = rootCommandHandler.Run(args);

            //Assert
            Assert.AreEqual(0, result);

            Assert.IsNotNull(mockCommand2Handler.LastRunParameters);
        }

        [ExpectedException(typeof(InvalidCommandException))]
        [TestMethod]
        public void Command3WithoutRequiredParameter_ThrowsExcception()
        {
            //Arrange
            var args = new string[]
                            {
                                "command3",
                                  "--OptionalArgument", "true",
                            };

            //Act
            var mockCommand3Handler = new MockCommand3Handler();
            var rootCommandHandler = new RootCommandHandler(new ICommandHandler[] { mockCommand3Handler });

            var result = rootCommandHandler.Run(args);
            //Assert with exception
        }

        [TestMethod]
        public void Command3WithoutRequiredParameter_RequiredParameterShouldReturnNull()
        {
            //Arrange
            var args = new string[]
                            {
                                "command3",
                                  "--OptionalArgument", "true",
                            };

            //Act
            var mockCommand3Handler = new MockCommand3Handler();
            var rootCommandHandler = new RootCommandHandler(new ICommandHandler[] { mockCommand3Handler });

            var result = rootCommandHandler.Run(args);
            Assert.IsTrue(mockCommand3Handler.LastRunParameters.OptionalArgument);
            //Because of boolean value type, we still think that argument was set. Maybe we should use only nullable types? 
            Assert.IsNull(mockCommand3Handler.LastRunParameters.RequiredArgument);

        }

        [TestMethod]
        public void MateuszCommandHandlerTest()
        {
            //Arrange
            var args = new string[]
                            {
                                "command3",
                                  "--argument", "kokos",
                            };

            //Act
            var mateuszRootCommandHandler = new MateuszRootCommandHandler(new List<IMateuszCommandHandler<IMateuszCommand>> { new MateuszCommandHandler() });

            var result = mateuszRootCommandHandler.Run(args);
            Assert.IsNotNull(mateuszRootCommandHandler.CommandHandlers.First().GetCommand());
            Assert.IsNotNull((mateuszRootCommandHandler.CommandHandlers.First().GetCommand() as MateuszCommand).ExampleArgument);
            Assert.IsNull((mateuszRootCommandHandler.CommandHandlers.First().GetCommand() as MateuszCommand).OptionalParameter);


        }


        [ExpectedException(typeof(InvalidCommandException))]
        [TestMethod]
        public void TestMethod3()
        {
            //Arrange
            var args = new string[]
                            {
                                "command1",
                                "--boolOption", "true",
                            };

            //Act
            var rootCommandHanlder = new RootCommandHandler(new ICommandHandler[] { new MockCommand2Handler() });

            var result = rootCommandHanlder.Run(args);

            //Assert with exception
        }
    }
}
