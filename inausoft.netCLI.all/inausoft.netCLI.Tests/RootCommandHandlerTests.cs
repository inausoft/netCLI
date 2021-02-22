using inausoft.netCLI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var rootCommandHanlder = new RootCommandHandler( new ICommandHandler[] { mockCommandHandler });
            
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
