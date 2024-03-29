﻿using inausoft.netCLI.Deserialization;
using inausoft.netCLI.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace inausoft.netCLI.Tests
{
    [TestClass]
    public class LogicalCommandDeserializerTests
    {
        ICommandDeserializer Deserializer { get; }

        public LogicalCommandDeserializerTests()
        {
            Deserializer = new LogicalCommandDeserializer();
        }

        [TestMethod]
        public void LogicalCommandDeserializer_DeserializeProperCommand_ForMultipleOptions()
        {
            // Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "--boolOption",
                                "--stringOption", stringOptionValue,
                                "--intOption", intOptionValue.ToString()
                            };

            // Act
            var command = Deserializer.Deserialize<Command1>(args);

            // Assert
            Assert.IsTrue(command.BoolOption);
            Assert.IsNull(command.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, command.StringOption);
            Assert.AreEqual(intOptionValue, command.IntOption);
        }

        [TestMethod]
        public void LogicalCommandDeserializer_DeserializeProperCommand_ForMultipleOptions_UsingShortNames()
        {
            // Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "-b",
                                "-s", stringOptionValue,
                                "-i", intOptionValue.ToString(),
                            };

            // Act
            var command = Deserializer.Deserialize<Command1>(args);

            // Assert
            Assert.IsTrue(command.BoolOption);
            Assert.IsNull(command.NotOptionProperty);
            Assert.AreEqual(stringOptionValue, command.StringOption);
            Assert.AreEqual(intOptionValue, command.IntOption);
        }

        [TestMethod]
        public void LogicalCommandDeserializer_ThrowsDeserializationException_WhenWrongOptionsWereSupplied_AndShortNameWasNotAssignedForRequiredOption()
        {
            // Arrange
            var stringOptionValue = "sampleString";
            var intOptionValue = 102;

            var args = new string[]
                            {
                                "-b",
                                "-s", stringOptionValue,
                                "-i", intOptionValue.ToString(),
                            };

            // Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command3>(args));

            // Assert
            Assert.AreEqual(ErrorCode.RequiredOptionMissing, exception.ErrorCode);
        }

        [TestMethod]
        [DataRow("s")]
        [DataRow(@"C:\ProgramFiles")]
        [DataRow(@"C:\Program Files")]
        public void LogicalCommandDeserializer_DeserializeProperCommand_ForStringOptions(string stringOptionValue)
        {
            // Arrange

            var args = new string[]
                            {
                                "--stringOption", stringOptionValue,
                            };

            // Act
            var command = Deserializer.Deserialize<Command2>(args);

            // Assert
            Assert.AreEqual(stringOptionValue, command.StringOption);
        }

        [TestMethod]
        public void LogicalCommandDeserializer_DeserializeProperCommand_WhenOptionalCommandWasNotSupplied()
        {
            // Arrange
            var args = new string[] { };

            // Act
            var command = Deserializer.Deserialize<Command2>(args);

            // Assert
            Assert.AreEqual(Command2.SomeDefaultValue, command.StringOption);
        }

        [TestMethod]
        public void LogicalCommandDeserializer_ThrowsDeserializationException_ForInvalidOption()
        {
            // Arrange
            var args = new string[]
                            {
                                "--boolOption", "true",
                                "--boolOptionXX", "true",
                                "--stringOption", "stringOptionValue",
                                "--intOption", "1",
                            };

            // Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            // Assert
            Assert.AreEqual(ErrorCode.UnrecognizedOption, exception.ErrorCode);
        }

        [TestMethod]
        public void LogicalCommandDeserializer_ThrowsDeserializationExceptionWithUnrecognizedOption_WhenOptionIsSuppliedWithOneHyphen()
        {
            // Arrange
            var args = new string[]
                            {
                                "-stringOption", "stringOptionValue",
                            };

            // Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command2>(args));

            // Assert
            Assert.AreEqual(ErrorCode.UnrecognizedOption, exception.ErrorCode);
        }

        [TestMethod]
        public void LogicalCommandDeserializer_Deserialize_ThrowsDeserializationExceptionWithOptionValueMissing_WhenNoValueWasSpecifiedForNotBooleanProperty()
        {
            // Arrange
            var args = new string[]
                            {
                                "--stringOption",
                                "--boolOption", "true",
                                "--intOption", "1",
                            };

            // Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            // Assert
            Assert.AreEqual(ErrorCode.OptionValueMissing, exception.ErrorCode);
        }

        [TestMethod]
        public void LogicalCommandDeserializer_ThrowsDeserializationExceptionWithRequiredOptionMissing_WhenNotAllRequiredOptionsWereSupplied()
        {
            // Arrange
            var args = new string[]
                            {
                                "--boolOption", "true",
                                "--intOption", "1",
                            };

            // Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            // Assert
            Assert.AreEqual(ErrorCode.RequiredOptionMissing, exception.ErrorCode);
        }

        [TestMethod]
        [DataRow(new string[] { "random_string_with_no_options" })]
        [DataRow(new string[] { "command", "--option", "optionvalue" })]
        [DataRow(new string[] { "op--tion", "optionvalue" })]
        [DataRow(new string[] { "--optionX", "optionXvalue", "--optionY", "optionYvalue", "random_string_at_the_end" })]
        public void LogicalCommandDeserializer_ThrowsDeserializationExceptionWithInvalidOptionsFormat_ForInvalidInputArgs(string[] args)
        {
            // Act
            var exception = Assert.ThrowsException<CommandDeserializationException>(() => Deserializer.Deserialize<Command1>(args));

            // Assert
            Assert.AreEqual(ErrorCode.InvalidOptionsFormat, exception.ErrorCode);
        }
    }
}
