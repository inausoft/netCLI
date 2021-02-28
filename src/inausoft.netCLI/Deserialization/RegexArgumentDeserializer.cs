using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace inausoft.netCLI.Deserialization
{
    public class RegexArgumentDeserializer : IArgumentDeserializer
    {
        private const string OptionsPattern = @"--(\S+)\s?(\w\S*)*";

        private const string ValidationPattern = @"^(\s?--\S+(\s\w\S*)?)*$";

        public T Deserialize<T>(string argumentExpression) where T: class
        {
            return Deserialize(typeof(T), argumentExpression) as T;
        }

        public object Deserialize(Type type, string optionsExpression)
        {
            if (!ValidateOptionsExpression(optionsExpression))
            {
                throw new FormatException($"{nameof(optionsExpression)} has invalid format.");
            }

            var command = Activator.CreateInstance(type);

            var options = new Regex(OptionsPattern).Matches(optionsExpression);

            foreach (Match option in options)
            {
                var optionName = option.Groups[1].Value;

                var property = type.GetProperties().FirstOrDefault(it => Attribute.IsDefined(it, typeof(OptionAttribute))
                                                        && (Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute).Name == optionName);

                if (property == null)
                {
                    throw new InvalidOptionException(optionName, $"Option {optionName} was not defined for {type}");
                }

                //if there is no value for an option. Ex. 'move --force' as 'opposed to --force true'
                if (string.IsNullOrEmpty(option.Groups[2].Value))
                {
                    property.SetMethod.Invoke(command, new object[] { true });
                }
                else
                {
                    property.SetMethod.Invoke(command, new object[] { Convert.ChangeType(option.Groups[2].Value, property.PropertyType) });
                }
            }

            return command;
        }

        private bool ValidateOptionsExpression(string optionsExpression)
        {
            return new Regex(ValidationPattern).IsMatch(optionsExpression);
        }
    }
}
