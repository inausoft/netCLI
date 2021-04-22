using inausoft.netCLI.Deserialization;
using inausoft.netCLI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class RegexArgumentDeserializerTests
    {
        ICommandDeserializer Deserializer { get; }

        public RegexArgumentDeserializerTests()
        {
            Deserializer = new LogicalCommandDeserializer();
        }

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

            //Act
            var command = Deserializer.Deserialize<Command1>(args);

            //Assert
            Assert.IsTrue(command.BoolOption);
            Assert.IsNull(command.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, command.StringOption);
            Assert.AreEqual(intOptionValue, command.IntOption);
        }

        [TestMethod]
        [DataRow("s")]
        [DataRow(@"C:\ProgramFiles")]
        [DataRow(@"C:\Program Files")]
        public void RegexArgumentHandler_DeserializeProperCommand_ForStringOptions(string stringOptionValue)
        {
            //Arrange

            var args = new string[]
                            {
                                "--stringOption", stringOptionValue
                            };

            //Act
            var command = Deserializer.Deserialize<Command2>(args);

            //Assert
            Assert.AreEqual(stringOptionValue, command.StringOption);
        }

        [TestMethod]
        public void RegexArgumentHandler_DeserializeProperCommand_WhenOptionalCommandWasNotSupplied()
        {
            //Arrange
            var args = new string[] { };

            //Act
            var command = Deserializer.Deserialize<Command2>(args);

            //Assert
            Assert.AreEqual(Command2.SomeDefaultValue, command.StringOption);
        }

        [TestMethod]
        public void RegexArgumentHandler_ThrowsDeserializationException_ForInvalidOption()
        {
            //Arrange
            var args = new string[]
                            {
                                "--boolOption", "true",
                                "--boolOptionXX", "true",
                                "--stringOption", "stringOptionValue",
                                "--intOption", "1"
                            };

            //Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            //Assert
            Assert.AreEqual(ErrorCode.UnrecognizedOption, exception.ErrorCode);
        }

        [TestMethod]
        public void RegexOptionsDeserializer_Deserialize_ThrowsDeserializationException_WhenNoValueWasSpecifiedForNotBooleanProperty()
        {
            //Arrange
            var args = new string[]
                            {
                                "--stringOption",
                                "--boolOption", "true",
                                "--intOption", "1"
                            };

            //Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            //Assert
            Assert.AreEqual(ErrorCode.OptionValueMissing, exception.ErrorCode);
        }

        [TestMethod]
        public void RegexArgumentHandler_ThrowsDeserializationException_WhenNotAllRequiredOptionsWereSupplied()
        {
            //Arrange
            var args = new string[]
                            {
                                "--boolOption", "true",
                                "--intOption", "1"
                            };

            //Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            //Assert
            Assert.AreEqual(ErrorCode.RequiredOptionMissing, exception.ErrorCode);
        }

        [TestMethod]
        [DataRow(new string[] { "random_string_with_no_options" })]
        [DataRow(new string[] { "command", "--option", "optionvalue" })]
        [DataRow(new string[] { "op--tion", "optionvalue" })]
        [DataRow(new string[] { "--optionX", "optionXvalue", "--optionY", "optionYvalue", "random_string_at_the_end" })]
        public void RegexArgumentHandler_ThrowsDeserializationException_ForInvalidInputArgs(string[] args)
        {
            //Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            //Assert
            Assert.AreEqual(ErrorCode.InvalidOptionsFormat, exception.ErrorCode);
        }
    }
}
