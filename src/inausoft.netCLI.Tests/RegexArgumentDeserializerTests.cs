using inausoft.netCLI.Deserialization;
using inausoft.netCLI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class RegexArgumentDeserializerTests
    {
        [TestMethod]
        public void RootCommandHandler_RunsProperCommandHandler_ForMultipleOptions()
        {
            //Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString()
                            };

            var deserializer = new RegexArgumentDeserializer();

            //Act
            var command = deserializer.Deserialize<Command1>(string.Join(" ", args));

            //Assert
            Assert.IsTrue(command.BoolOption);
            Assert.IsNull(command.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, command.StringOption);
            Assert.AreEqual(intOptionValue, command.IntOption);
        }

        [TestMethod]
        [DataRow("s")]
        [DataRow(@"C:\Program%Files")]
        public void RootCommandHandler_RunsProperCommandHandler_ForStringOptions(string stringOptionValue)
        {
            //Arrange

            var args = new string[]
                            {
                                "--stringOption", stringOptionValue
                            };

            var deserializer = new RegexArgumentDeserializer();

            //Act
            var command = deserializer.Deserialize<Command1>(string.Join(" ", args));

            //Assert
            Assert.AreEqual(stringOptionValue, command.StringOption);
        }
    }
}
