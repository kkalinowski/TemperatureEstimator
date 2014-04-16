using System;
using CsvHelper.TypeConversion;

namespace TemperatureEstimator.Logic
{
    public class FailSafeCsvDoubleTypeConverter : ITypeConverter
    {
        /// <summary>
        /// Converts the object to a string.
        /// </summary>
        /// <param name="options">The options to use when converting.</param><param name="value">The object to convert to a string.</param>
        /// <returns>
        /// The string representation of the object.
        /// </returns>
        public string ConvertToString(TypeConverterOptions options, object value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts the string to an object.
        /// </summary>
        /// <param name="options">The options to use when converting.</param><param name="text">The string to convert to an object.</param>
        /// <returns>
        /// The object created from the string.
        /// </returns>
        public object ConvertFromString(TypeConverterOptions options, string text)
        {
            return text.IsNullOrEmpty() ? double.NaN : double.Parse(text);
        }

        /// <summary>
        /// Determines whether this instance [can convert from] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if this instance [can convert from] the specified type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConvertFrom(Type type)
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance [can convert to] the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if this instance [can convert to] the specified type; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConvertTo(Type type)
        {
            throw new NotImplementedException();
        }
    }
}