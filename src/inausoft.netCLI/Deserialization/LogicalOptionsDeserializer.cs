using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace inausoft.netCLI.Deserialization
{
    public class LogicalOptionsDeserializer : IOptionsDeserializer
    {
        public T Deserialize<T>(string[] args) where T : class
        {
            return Deserialize(typeof(T), args) as T;
        }

        public object Deserialize(Type type, string[] args)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Dictionary<string, string> options = new Dictionary<string, string>();

            foreach(var arg in args)
            {
                if (arg.Contains('-'))
                {
                    options.Add(arg.Replace("-", ""), null);
                }
                else
                {
                    var key = options.LastOrDefault(it => it.Value == null).Key;

                    if(key == null)
                    {
                        throw new DeserializationException(ErrorCode.InvalidOptionsFormat, $"Cannot deserialize into type {type} - specified args are invalid.");
                    }

                    options[key] = arg;
                }
            }

            var command = Activator.CreateInstance(type);

            var properties = type.GetProperties().Where(it => Attribute.IsDefined(it, typeof(OptionAttribute)));

            foreach (var optionType in properties.Select(it => Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute))
            {
                if (!optionType.IsOptional && !options.Keys.Contains(optionType.Name))
                {
                    throw new DeserializationException(ErrorCode.RequiredOptionMissing, $"Cannot deserialize into type {type} - option {optionType} is missing.");
                }
            }

            foreach (var option in options)
            {
                var optionName = option.Key;

                var property = properties.FirstOrDefault(it => (Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute).Name == optionName);

                if (property == null)
                {
                    throw new DeserializationException(ErrorCode.UnrecognizedOption, $"Cannot deserialize into type {type} - no option : {optionName} was declared.");
                }

                //if there is no value for an option. Ex. 'move --force' as 'opposed to --force true'
                if (string.IsNullOrEmpty(option.Value))
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
                    property.SetMethod.Invoke(command, new object[] { Convert.ChangeType(option.Value, property.PropertyType) });
                }
            }

            return command;
        }
    }
}
