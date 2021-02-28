using inausoft.netCLI.Deserialization;
using inausoft.netCLI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class RegexArgumentDeserializerTests
    {
        [TestMethod]
        public void RegexArgumentHandler_DeserializeProperCommand_ForMultipleOptions()
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

            var deserializer = new RegexOptionsDeserializer();

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
        public void RegexArgumentHandler_DeserializeProperCommand_ForStringOptions(string stringOptionValue)
        {
            //Arrange

            var args = new string[]
                            {
                                "--stringOption", stringOptionValue
                            };

            var deserializer = new RegexOptionsDeserializer();

            //Act
            var command = deserializer.Deserialize<Command1>(string.Join(" ", args));

            //Assert
            Assert.AreEqual(stringOptionValue, command.StringOption);
        }

        [ExpectedException(typeof(InvalidOptionException))]
        [TestMethod]
        public void RegexArgumentHandler_ThrowsInvalidOptionException_ForInvalidOption()
        {
            //Arrange
            var args = new string[]
                            {
                                "--boolOptionXX", "true",
                            };

            var deserializer = new RegexOptionsDeserializer();

            //Act
            deserializer.Deserialize<Command1>(string.Join(" ", args));

            //Assert with exception
        }

        [ExpectedException(typeof(FormatException))]
        [TestMethod]
        [DataRow("random_string_winth_no_options")]
        [DataRow("command --option optionvalue")]
        [DataRow("--optionX optionXvalue --optionY optionYvalue random_string_at_the_end")]
        public void RegexArgumentHandler_ThrowsArgumentException_ForInvalidInputArgs(string optionsExpression)
        {
            //Arrange
            var args = new string[]
                            {
                                optionsExpression,
                            };

            var deserializer = new RegexOptionsDeserializer();

            //Act
            deserializer.Deserialize<Command1>(string.Join(" ", args));

            //Assert with exception
        }
    }
}
