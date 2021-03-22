using System;

namespace inausoft.netCLI.Deserialization
{
    public interface ICommandDeserializer
    {
        /// <summary>
        /// Deserializes array of args into specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns>An instance of specified <see cref="Type"/></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CommandDeserializationException"/>
        object Deserialize(Type type, string[] args);

        /// <summary>
        /// Deserializes array of args into specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns>An instance of specified <see cref="Type"/></returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CommandDeserializationException"/>
        T Deserialize<T>(string[] args) where T : class;
    }
}
