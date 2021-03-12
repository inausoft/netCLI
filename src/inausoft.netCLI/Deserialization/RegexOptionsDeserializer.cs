﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace inausoft.netCLI.Deserialization
{
    public class RegexOptionsDeserializer : IOptionsDeserializer
    {
        private const string OptionsPattern = @"--(\S+)\s?(\w\S*)*";

        private const string ValidationPattern = @"^(\s?--\S+(\s\w\S*)?)*$";

        public T Deserialize<T>(string[] args) where T : class
        {
            return Deserialize(typeof(T), args) as T;
        }

        public object Deserialize(Type type, string[] args)
        {
            return Deserialize(type, string.Join(" ", args));
        }

        public object Deserialize(Type type, string optionsExpression)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (optionsExpression == null)
            {
                throw new ArgumentNullException(nameof(optionsExpression));
            }

            if (!ValidateOptionsExpression(optionsExpression))
            {
                throw new DeserializationException(ErrorCode.InvalidOptionsFormat, $"Cannot deserialize into type {type} - specified option format : {OptionsPattern} is invalid.");
            }

            var command = Activator.CreateInstance(type);

            var options = new Regex(OptionsPattern).Matches(optionsExpression);

            var properties = type.GetProperties().Where(it => Attribute.IsDefined(it, typeof(OptionAttribute)));

            foreach (var optionType in properties.Select(it => Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute))
            {
                if (!optionType.IsOptional && !optionsExpression.Contains($"--{optionType.Name}"))
                {
                    throw new DeserializationException(ErrorCode.RequiredOptionMissing, $"Cannot deserialize into type {type} - option {optionType} is missing.");
                }
            }

            foreach (Match option in options)
            {
                var optionName = option.Groups[1].Value;

                var property = properties.FirstOrDefault(it => (Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute).Name == optionName);

                if (property == null)
                {
                    throw new DeserializationException(ErrorCode.UnrecognizedOption, $"Cannot deserialize into type {type} - no option : {optionName} was declared.");
                }

                //if there is no value for an option. Ex. 'move --force' as 'opposed to --force true'
                if (string.IsNullOrEmpty(option.Groups[2].Value))
                {
                    if (property.PropertyType == typeof(bool))
                    {
                        property.SetMethod.Invoke(command, new object[] { true });
                    }
                    else
                    {
                        throw new DeserializationException(ErrorCode.OptionValueMissing, $"No value was specified for option : {optionName}.");
                    }
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
